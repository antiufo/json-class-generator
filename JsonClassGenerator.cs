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
        public bool ExplicitDeserialization;
        public bool NoHelperClass;
        public string MainClass;
        public bool UsePascalCase;
        public bool UseNestedClasses;
        public bool ApplyObfuscationAttributes;

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
            if (!NoHelperClass && ExplicitDeserialization) File.WriteAllBytes(Path.Combine(TargetFolder, "JsonClassHelper.cs"), Properties.Resources.JsonClassHelper);
            GeneratedNames.Add(MainClass.ToLower());
            GenerateClass(new JObject[] { json }, MainClass, true);

        }






        private void GenerateClass(JObject[] examples, string className, bool isRoot)
        {
            var hasSecondaryClasses = false;
            var fields = new Dictionary<string, JsonType>();

            var first = true;
            var applyNoRenamingAttribute = ApplyObfuscationAttributes && !ExplicitDeserialization && !UsePascalCase;
            var applyNoPruneAttribute = ApplyObfuscationAttributes && !ExplicitDeserialization && UseProperties;
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

            if (UseNestedClasses)
            {
                foreach (var field in fields)
                {
                    GeneratedNames.Add(field.Key.ToLower());
                }
            }

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

                if (type.InternalType != null && type.InternalType.Type == JsonTypeEnum.Object)
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

                    field.Value.InternalType.AssignName(CreateNameFromPlural(field.Key));
                    GenerateClass(subexamples.ToArray(), field.Value.InternalType.AssignedName, false);
                    hasSecondaryClasses = true;
                }
            }

            var fieldsList = fields.Select(x => new FieldInfo(x.Key, x.Value, UsePascalCase)).ToArray();

            var shouldSuppressWarning = InternalVisibility && !UseProperties && !ExplicitDeserialization;

            var folder = TargetFolder;
            if (!UseNestedClasses && !isRoot && SecondaryNamespace != null)
            {
                var s = SecondaryNamespace;
                if (s.StartsWith(Namespace + ".")) s = s.Substring(Namespace.Length + 1);
                folder = Path.Combine(folder, s);
                Directory.CreateDirectory(folder);
            }

            using (var sw = new StreamWriter(Path.Combine(folder, (UseNestedClasses && !isRoot ? MainClass + "." : "") + className + ".cs"), false, Encoding.UTF8))
            {

                sw.WriteLine("// JSON C# Class Generator");
                sw.WriteLine("// http://at-my-window.blogspot.com/?page=json-class-generator");
                sw.WriteLine();
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                if (applyNoRenamingAttribute || applyNoPruneAttribute)
                    sw.WriteLine("using System.Reflection;");
                if (!ExplicitDeserialization && UsePascalCase)
                    sw.WriteLine("using Newtonsoft.Json;");
                sw.WriteLine("using Newtonsoft.Json.Linq;");
                if (ExplicitDeserialization)
                    sw.WriteLine("using JsonCSharpClassGenerator;");
                if (SecondaryNamespace != null && isRoot && hasSecondaryClasses && !UseNestedClasses)
                {
                    sw.WriteLine("using {0};", SecondaryNamespace);
                }
                sw.WriteLine();
                sw.WriteLine("namespace {0}", isRoot && !UseNestedClasses ? Namespace : NamespaceForSecondaryClasses);
                sw.WriteLine("{");
                sw.WriteLine();

                if (UseNestedClasses)
                {
                    sw.WriteLine("    {0} partial class {1}", Visibility, MainClass);
                    sw.WriteLine("    {");
                    if (!isRoot)
                    {
                        if (applyNoRenamingAttribute) sw.WriteLine("        " + NoRenameAttribute);
                        if (applyNoPruneAttribute) sw.WriteLine("        " + NoPruneAttribute);
                        sw.WriteLine("        {0} class {1}", Visibility, className);
                        sw.WriteLine("        {");
                    }
                }
                else
                {
                    if (applyNoRenamingAttribute) sw.WriteLine("    " + NoRenameAttribute);
                    if (applyNoPruneAttribute) sw.WriteLine("    " + NoPruneAttribute);
                    sw.WriteLine("    {0} class {1}", Visibility, className);
                    sw.WriteLine("    {");
                }

                var prefix = UseNestedClasses && !isRoot ? "            " : "        ";


                if (shouldSuppressWarning)
                {
                    sw.WriteLine("#pragma warning disable 0649");
                    if (!UsePascalCase) sw.WriteLine();
                }

                if (isRoot && ExplicitDeserialization) WriteStringConstructor(sw, className, prefix);

                if (ExplicitDeserialization)
                {
                    if (UseProperties) WriteClassWithPropertiesExplicitDeserialization(sw, className, fieldsList, isRoot, prefix);
                    else WriteClassWithFieldsExplicitDeserialization(sw, className, fieldsList, isRoot, prefix);
                }
                else
                {
                    WriteClassMembers(sw, fieldsList, prefix);
                }

                if (shouldSuppressWarning)
                {
                    sw.WriteLine();
                    sw.WriteLine("#pragma warning restore 0649");
                    sw.WriteLine();
                }


                if (UseNestedClasses && !isRoot)
                    sw.WriteLine("        }");

                sw.WriteLine("    }");


                sw.WriteLine("}");





            }


        }

        private HashSet<string> GeneratedNames = new HashSet<string>();
        private const string NoRenameAttribute = "[Obfuscation(Feature = \"renaming\", Exclude = true)]";
        private const string NoPruneAttribute = "[Obfuscation(Feature = \"trigger\", Exclude = false)]";

        private string CreateName(string name)
        {
            name = ToTitleCase(name);

            var finalName = name;
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

        private string CreateNameFromPlural(string plural)
        {
            plural = ToTitleCase(plural);



            return CreateName(pluralizationService.Singularize(plural));
        }

        private void WriteStringConstructor(StreamWriter sw, string className, string prefix)
        {
            sw.WriteLine();
            sw.WriteLine(prefix + "public {1}(string json)", Visibility, className);
            sw.WriteLine(prefix + "    : this(JObject.Parse(json))");
            sw.WriteLine(prefix + "{");
            sw.WriteLine(prefix + "}");
            sw.WriteLine();
        }

        private void WriteClassWithFieldsExplicitDeserialization(StreamWriter sw, string className, FieldInfo[] fields, bool isRoot, string prefix)
        {


            sw.WriteLine(prefix + "public {0}(JObject obj)", className);
            sw.WriteLine(prefix + "{");

            foreach (var field in fields)
            {
                sw.WriteLine(prefix + "    this.{0} = {1};", field.MemberName, field.GetGenerationCode("obj"));

            }

            sw.WriteLine(prefix + "}");
            sw.WriteLine();



            foreach (var field in fields)
            {
                sw.WriteLine(prefix + "public readonly {0} {1};", field.Type.GetCSharpType(false), field.MemberName);
            }



        }


        private void WriteClassWithProperties(StreamWriter sw, FieldInfo[] fields)
        {

            foreach (var field in fields)
            {
            }

        }


        private void WriteClassMembers(StreamWriter sw, FieldInfo[] fields, string prefix)
        {
            foreach (var field in fields)
            {
                if (UsePascalCase)
                {
                    sw.WriteLine();
                    sw.WriteLine(prefix + "[JsonProperty(\"{0}\")]", field.JsonMemberName);
                }

                if (UseProperties)
                {
                    sw.WriteLine(prefix + "public {0} {1} {{ get; set; }}", field.Type.GetCSharpType(true), field.MemberName);
                }
                else
                {
                    sw.WriteLine(prefix + "public {0} {1};", field.Type.GetCSharpType(true), field.MemberName);
                }
            }

        }








        private void WriteClassWithPropertiesExplicitDeserialization(StreamWriter sw, string className, FieldInfo[] fields, bool isRoot, string prefix)
        {

            sw.WriteLine(prefix + "private JObject __jobject;");
            sw.WriteLine(prefix + "public {0}(JObject obj)", className);
            sw.WriteLine(prefix + "{");
            sw.WriteLine(prefix + "    this.__jobject = obj;");
            sw.WriteLine(prefix + "}");
            sw.WriteLine();

            foreach (var field in fields)
            {

                string variable = null;
                if (field.Type.MustCache)
                {
                    variable = "_" + char.ToLower(field.MemberName[0]) + field.MemberName.Substring(1);
                    sw.WriteLine(prefix + "[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]");
                    sw.WriteLine(prefix + "private {0} {1};", field.Type.GetCSharpType(false), variable);
                }


                sw.WriteLine(prefix + "public {0} {1}", field.Type.GetCSharpType(false), field.MemberName);
                sw.WriteLine(prefix + "{");
                sw.WriteLine(prefix + "    get");
                sw.WriteLine(prefix + "    {");
                if (field.Type.MustCache)
                {
                    sw.WriteLine(prefix + "        if ({0} == null)", variable);
                    sw.WriteLine(prefix + "            {0} = {1};", variable, field.GetGenerationCode("__jobject"));
                    sw.WriteLine(prefix + "        return {0};", variable);
                }
                else
                {
                    sw.WriteLine(prefix + "        return {0};", field.GetGenerationCode("__jobject"));
                }
                sw.WriteLine(prefix + "    }");
                sw.WriteLine(prefix + "}");
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
