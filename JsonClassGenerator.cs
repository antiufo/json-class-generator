// Copyright (c) 2010 Andrea Martinelli
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;


namespace JsonCSharpClassGenerator
{
    public class JsonClassGenerator
    {


        public string Example;
        public string TargetFolder;
        public string Namespace;
        public string SecondaryNamespace;
        public bool UseProperties;
        public bool InternalVisibility;
        public bool NoHelperClass;
        public string MainClass;
        public bool UsePascalCase;

        private PluralizationService pluralizationService = PluralizationService.CreateService(new CultureInfo("en-us"));


        private string NamespaceForSecondaryClasses
        {
            get
            {
                if (SecondaryNamespace != null) return SecondaryNamespace;
                return Namespace;
            }
        }
        private string Visibility
        {
            get
            {
                return InternalVisibility ? "internal" : "public";
            }
        }


        public void GenerateClasses()
        {

            if (!Directory.Exists(TargetFolder)) Directory.CreateDirectory(TargetFolder);
            
            var json = JObject.Parse(Example);

            var parentFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!NoHelperClass) File.Copy(Path.Combine(parentFolder, "JsonClassHelper.cs"), Path.Combine(TargetFolder, "JsonClassHelper.cs"), true);
            GeneratedNames.Add(MainClass.ToLower());
            GenerateClass(new JObject[] { json }, MainClass, true);

        }






        private void GenerateClass(JObject[] examples, string className, bool isRoot)
        {
            var hasSecondaryClasses = false;
            var fields = new Dictionary<string, JsonType>();

            var first = true;
            foreach (var obj in examples)
            {
                foreach (var prop in obj.Properties())
                {
                    JsonType fieldType;
                    var currentType = new JsonType(prop.Value);
                    var propName = prop.Name;
                    if (fields.TryGetValue(propName, out fieldType))
                    {

                        var commonType = fieldType.GetCommonType(currentType);

                        fields[propName] = commonType;
                    }
                    else
                    {
                        var commonType = currentType;
                        if (!first) commonType = commonType.GetCommonType(JsonType.Null);
                        fields.Add(propName, commonType);
                    }
                }
                first = false;
            }

        //    var totalMembers = fields.Count;
         //   var totalInstances = examples.Sum(x => x.Count);

            foreach (var field in fields)
            {
                var type = field.Value;
                if (type.Type == JsonTypeEnum.Object)
                {
                    var subexamples = new List<JObject>(examples.Length);
                    foreach (var obj in examples)
                    {
                        JToken value;
                        if (obj.TryGetValue(field.Key, out value))
                        {
                            if (value.Type == JTokenType.Object)
                            {
                                subexamples.Add((JObject)value);
                            }
                        }
                    }

                    field.Value.AssignName(CreateName(field.Key));
                    GenerateClass(subexamples.ToArray(), field.Value.AssignedName, false);
                    hasSecondaryClasses = true;
                }

                if (type.InternalType !=null && type.InternalType.Type == JsonTypeEnum.Object)
                {
                    var subexamples = new List<JObject>(examples.Length);
                    foreach (var obj in examples)
                    {
                        JToken value;
                        if (obj.TryGetValue(field.Key, out value))
                        {
                            if (value.Type == JTokenType.Array)
                            {
                                foreach (var item in (JArray)value)
                                {
                                    subexamples.Add((JObject)item);
                                }

                            }
                            else if (value.Type == JTokenType.Object)
                            {
                                foreach (var item in (JObject)value)
                                {
                                    subexamples.Add((JObject)item.Value);
                                }
                            }
                        }
                    }

                    field.Value.InternalType.AssignName(CreateNameFromPlural( field.Key));
                    GenerateClass(subexamples.ToArray(), field.Value.InternalType.AssignedName, false);
                    hasSecondaryClasses = true;
                }
            }

            var fieldsList = fields.Select(x => new FieldInfo(x.Key, x.Value, UsePascalCase)).ToArray();

            using (var sw = new StreamWriter(Path.Combine(TargetFolder, className + ".cs"), false, Encoding.UTF8))
            {

                sw.WriteLine("// JSON C# Class Generator");
                sw.WriteLine("// http://at-my-window.blogspot.com/?page=json-class-generator");
                sw.WriteLine();
                sw.WriteLine("using System;");
                sw.WriteLine("using Newtonsoft.Json.Linq;");
                sw.WriteLine("using JsonCSharpClassGenerator;");
                if (SecondaryNamespace != null && isRoot && hasSecondaryClasses)
                {
                    sw.WriteLine("using {0};", SecondaryNamespace);
                }
                sw.WriteLine();
                sw.WriteLine("namespace {0}", isRoot ? Namespace : NamespaceForSecondaryClasses);
                sw.WriteLine("{");
                sw.WriteLine();

                sw.WriteLine("    {0} class {1}", Visibility, className);
                sw.WriteLine("    {");
                sw.WriteLine();

                if (isRoot) WriteStringConstructor(sw, className);

                if (UseProperties) WriteClassWithProperties(sw, className, fieldsList, isRoot);
                else WriteClassWithFields(sw, className, fieldsList, isRoot);

                sw.WriteLine("    }");
                sw.WriteLine("}");





            }


        }

        private HashSet<string> GeneratedNames = new HashSet<string>();

        private string CreateName(string name)
        {
            name=ToTitleCase(name);

            var finalName=name;
            if (GeneratedNames.Contains(finalName.ToLower()))
            {
                var i = 2;
                do
                {
                    finalName = name + i.ToString();
                    i++;
                } while (GeneratedNames.Contains(finalName.ToLower()));
            }

            GeneratedNames.Add(finalName.ToLower());
            return finalName;
        }

        private string CreateNameFromPlural(string plural) {
            plural = ToTitleCase(plural);



            return CreateName(pluralizationService.Singularize(plural));
        }

        private void WriteStringConstructor(StreamWriter sw, string className)
        {
            sw.WriteLine("        public {1}(string json)", Visibility, className);
            sw.WriteLine("         : this(JObject.Parse(json))");
            sw.WriteLine("        {");
            sw.WriteLine("        }");
            sw.WriteLine();
        }

        private void WriteClassWithFields(StreamWriter sw, string className, FieldInfo[] fields, bool isRoot)
        {


            sw.WriteLine("        public {0}(JObject obj)", className);
            sw.WriteLine("        {");

            foreach (var field in fields)
            {
                sw.WriteLine("           this.{0} = {1};", field.MemberName, field.GetGenerationCode("obj"));

            }

            sw.WriteLine("        }");
            sw.WriteLine();



            foreach (var field in fields)
            {
                sw.WriteLine("        public readonly {0} {1};", field.Type.GetCSharpType(), field.MemberName);
            }



        }












        private void WriteClassWithProperties(StreamWriter sw, string className, FieldInfo[] fields, bool isRoot)
        {


            sw.WriteLine("        private JObject __jobject;");
            sw.WriteLine("        public {0}(JObject obj)", className);
            sw.WriteLine("        {");
            sw.WriteLine("            this.__jobject = obj;");
            sw.WriteLine("        }");
            sw.WriteLine();

            foreach (var field in fields)
            {

                string variable = null;
                if (field.Type.MustCache)
                {
                    variable = "_" + char.ToLower(field.MemberName[0]) + field.MemberName.Substring(1);
                    sw.WriteLine("        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]");
                    sw.WriteLine("        private {0} {1};", field.Type.GetCSharpType(), variable);
                }


                sw.WriteLine("        public {0} {1}", field.Type.GetCSharpType(), field.MemberName);
                sw.WriteLine("        {");
                sw.WriteLine("            get");
                sw.WriteLine("            {");
                if (field.Type.MustCache)
                {
                    sw.WriteLine("                if({0} == null)", variable);
                    sw.WriteLine("                    {0} = {1};", variable, field.GetGenerationCode("__jobject"));
                    sw.WriteLine("                return {0};", variable);
                }
                else
                {
                    sw.WriteLine("                return {0};", field.GetGenerationCode("__jobject"));
                }
                sw.WriteLine("            }");
                sw.WriteLine("        }");
                sw.WriteLine();

            }

        }





        internal static string ToTitleCase(string str)
        {
            var sb = new StringBuilder(str.Length);
            var flag = true;

            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(flag ? char.ToUpper(c) : c);
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }

            return sb.ToString();
        }





    }
}
