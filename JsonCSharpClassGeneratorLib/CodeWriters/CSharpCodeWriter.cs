using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xamasoft.JsonCSharpClassGenerator.CodeWriters
{
    public class CSharpCodeWriter : ICodeWriter
    {
        public string FileExtension
        {
            get { return ".cs"; }
        }

        public string DisplayName
        {
            get { return "C#"; }
        }


        private const string NoRenameAttribute = "[Obfuscation(Feature = \"renaming\", Exclude = true)]";
        private const string NoPruneAttribute = "[Obfuscation(Feature = \"trigger\", Exclude = false)]";

        public string GetTypeName(JsonType type, IJsonClassGeneratorConfig config)
        {
            var arraysAsLists = config.ExplicitDeserialization;

            switch (type.Type)
            {
                case JsonTypeEnum.Anything: return "object";
                case JsonTypeEnum.Array: return arraysAsLists ? "IList<" + GetTypeName(type.InternalType, config) + ">" : GetTypeName(type.InternalType, config) + "[]";
                case JsonTypeEnum.Dictionary: return "Dictionary<string, " + GetTypeName(type.InternalType, config) + ">";
                case JsonTypeEnum.Boolean: return "bool";
                case JsonTypeEnum.Float: return "double";
                case JsonTypeEnum.Integer: return "int";
                case JsonTypeEnum.Long: return "long";
                case JsonTypeEnum.Date: return "DateTime";
                case JsonTypeEnum.NonConstrained: return "object";
                case JsonTypeEnum.NullableBoolean: return "bool?";
                case JsonTypeEnum.NullableFloat: return "double?";
                case JsonTypeEnum.NullableInteger: return "int?";
                case JsonTypeEnum.NullableLong: return "long?";
                case JsonTypeEnum.NullableDate: return "DateTime?";
                case JsonTypeEnum.NullableSomething: return "object";
                case JsonTypeEnum.Object: return type.AssignedName;
                case JsonTypeEnum.String: return "string";
                default: throw new System.NotSupportedException("Unsupported json type");
            }
        }


        public void WriteClass(IJsonClassGeneratorConfig config, StreamWriter sw, string className, FieldInfo[] fields, bool isRoot, bool hasSecondaryClasses)
        {
            var visibility = config.InternalVisibility ? "internal" : "public";
            var applyNoRenamingAttribute = config.ApplyObfuscationAttributes && !config.ExplicitDeserialization && !config.UsePascalCase;
            var applyNoPruneAttribute = config.ApplyObfuscationAttributes && !config.ExplicitDeserialization && config.UseProperties;

            sw.WriteLine("// JSON C# Class Generator");
            sw.WriteLine("// http://www.xamasoft.com/json-csharp-class-generator");
            sw.WriteLine();
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            if (applyNoRenamingAttribute || applyNoPruneAttribute)
                sw.WriteLine("using System.Reflection;");
            if (!config.ExplicitDeserialization && config.UsePascalCase)
                sw.WriteLine("using Newtonsoft.Json;");
            sw.WriteLine("using Newtonsoft.Json.Linq;");
            if (config.ExplicitDeserialization)
                sw.WriteLine("using JsonCSharpClassGenerator;");
            if (config.SecondaryNamespace != null && isRoot && hasSecondaryClasses && !config.UseNestedClasses)
            {
                sw.WriteLine("using {0};", config.SecondaryNamespace);
            }
            sw.WriteLine();
            sw.WriteLine("namespace {0}", isRoot && !config.UseNestedClasses ? config.Namespace : (config.SecondaryNamespace ?? config.Namespace));
            sw.WriteLine("{");
            sw.WriteLine();

            if (config.UseNestedClasses)
            {
                sw.WriteLine("    {0} partial class {1}", visibility, config.MainClass);
                sw.WriteLine("    {");
                if (!isRoot)
                {
                    if (applyNoRenamingAttribute) sw.WriteLine("        " + NoRenameAttribute);
                    if (applyNoPruneAttribute) sw.WriteLine("        " + NoPruneAttribute);
                    sw.WriteLine("        {0} class {1}", visibility, className);
                    sw.WriteLine("        {");
                }
            }
            else
            {
                if (applyNoRenamingAttribute) sw.WriteLine("    " + NoRenameAttribute);
                if (applyNoPruneAttribute) sw.WriteLine("    " + NoPruneAttribute);
                sw.WriteLine("    {0} class {1}", visibility, className);
                sw.WriteLine("    {");
            }

            var prefix = config.UseNestedClasses && !isRoot ? "            " : "        ";


            var shouldSuppressWarning = config.InternalVisibility && !config.UseProperties && !config.ExplicitDeserialization;
            if (shouldSuppressWarning)
            {
                sw.WriteLine("#pragma warning disable 0649");
                if (!config.UsePascalCase) sw.WriteLine();
            }

            if (isRoot && config.ExplicitDeserialization) WriteStringConstructorExplicitDeserialization(config, sw, className, prefix);

            if (config.ExplicitDeserialization)
            {
                if (config.UseProperties) WriteClassWithPropertiesExplicitDeserialization(sw, className, fields, prefix, isRoot);
                else WriteClassWithFieldsExplicitDeserialization(sw, className, fields, prefix, isRoot);
            }
            else
            {
                WriteClassMembers(config, sw, fields, prefix);
            }

            if (shouldSuppressWarning)
            {
                sw.WriteLine();
                sw.WriteLine("#pragma warning restore 0649");
                sw.WriteLine();
            }


            if (config.UseNestedClasses && !isRoot)
                sw.WriteLine("        }");

            sw.WriteLine("    }");


            sw.WriteLine("}");


        }





        private void WriteClassMembers(IJsonClassGeneratorConfig config, StreamWriter sw, FieldInfo[] fields, string prefix)
        {
            foreach (var field in fields)
            {
                if (config.UsePascalCase)
                {
                    sw.WriteLine();
                    sw.WriteLine(prefix + "[JsonProperty(\"{0}\")]", field.JsonMemberName);
                }

                if (config.UseProperties)
                {
                    sw.WriteLine(prefix + "public {0} {1} {{ get; set; }}", field.Type.GetCSharpType(), field.MemberName);
                }
                else
                {
                    sw.WriteLine(prefix + "public {0} {1};", field.Type.GetCSharpType(), field.MemberName);
                }
            }

        }







        #region Code for (obsolete) explicit deserialization
        private void WriteClassWithPropertiesExplicitDeserialization(StreamWriter sw, string className, FieldInfo[] fields, string prefix, bool isRoot)
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
                    sw.WriteLine(prefix + "private {0} {1};", field.Type.GetCSharpType(), variable);
                }


                sw.WriteLine(prefix + "public {0} {1}", field.Type.GetCSharpType(), field.MemberName);
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


        private void WriteStringConstructorExplicitDeserialization(IJsonClassGeneratorConfig config, StreamWriter sw, string className, string prefix)
        {
            sw.WriteLine();
            sw.WriteLine(prefix + "public {1}(string json)", config.InternalVisibility ? "internal" : "public", className);
            sw.WriteLine(prefix + "    : this(JObject.Parse(json))");
            sw.WriteLine(prefix + "{");
            sw.WriteLine(prefix + "}");
            sw.WriteLine();
        }

        private void WriteClassWithFieldsExplicitDeserialization(StreamWriter sw, string className, FieldInfo[] fields, string prefix, bool isRoot)
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
                sw.WriteLine(prefix + "public readonly {0} {1};", field.Type.GetCSharpType(), field.MemberName);
            }
        }
        #endregion

    }
}
