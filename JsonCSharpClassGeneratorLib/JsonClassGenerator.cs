// Copyright © 2010 Xamasoft

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using Xamasoft.JsonCSharpClassGenerator.CodeWriters;


namespace Xamasoft.JsonCSharpClassGenerator
{
    public class JsonClassGenerator : IJsonClassGeneratorConfig
    {


        public string Example { get; set; }
        public string TargetFolder { get; set; }
        public string Namespace { get; set; }
        public string SecondaryNamespace { get; set; }
        public bool UseProperties { get; set; }
        public bool InternalVisibility { get; set; }
        public bool ExplicitDeserialization { get; set; }
        public bool NoHelperClass { get; set; }
        public string MainClass { get; set; }
        public bool UsePascalCase { get; set; }
        public bool UseNestedClasses { get; set; }
        public bool ApplyObfuscationAttributes { get; set; }
        public ICodeWriter CodeWriter { get; set; }

        private PluralizationService pluralizationService = PluralizationService.CreateService(new CultureInfo("en-us"));



        public void GenerateClasses()
        {

            if (CodeWriter == null) CodeWriter = new CSharpCodeWriter();
            if (ExplicitDeserialization && !(CodeWriter is CSharpCodeWriter)) throw new ArgumentException("Explicit deserialization is obsolete and is only supported by the C# provider.");
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
            var jsonFields = new Dictionary<string, JsonType>();

            var first = true;
        
            foreach (var obj in examples)
            {
                foreach (var prop in obj.Properties())
                {
                    JsonType fieldType;
                    var currentType = new JsonType(this, prop.Value);
                    var propName = prop.Name;
                    if (jsonFields.TryGetValue(propName, out fieldType))
                    {

                        var commonType = fieldType.GetCommonType(currentType);

                        jsonFields[propName] = commonType;
                    }
                    else
                    {
                        var commonType = currentType;
                        if (!first) commonType = commonType.GetCommonType(JsonType.GetNull(this));
                        jsonFields.Add(propName, commonType);
                    }
                }
                first = false;
            }

            if (UseNestedClasses)
            {
                foreach (var field in jsonFields)
                {
                    GeneratedNames.Add(field.Key.ToLower());
                }
            }

            foreach (var field in jsonFields)
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

            var fields = jsonFields.Select(x => new FieldInfo(this, x.Key, x.Value, UsePascalCase)).ToArray();

            var folder = TargetFolder;
            if (!UseNestedClasses && !isRoot && SecondaryNamespace != null)
            {
                var s = SecondaryNamespace;
                if (s.StartsWith(Namespace + ".")) s = s.Substring(Namespace.Length + 1);
                folder = Path.Combine(folder, s);
                Directory.CreateDirectory(folder);
            }

            using (var sw = new StreamWriter(Path.Combine(folder, (UseNestedClasses && !isRoot ? MainClass + "." : "") + className + CodeWriter.FileExtension), false, Encoding.UTF8))
            {
                CodeWriter.WriteClass(this, sw, className, fields, isRoot, hasSecondaryClasses);
            }


        }

        private HashSet<string> GeneratedNames = new HashSet<string>();

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
