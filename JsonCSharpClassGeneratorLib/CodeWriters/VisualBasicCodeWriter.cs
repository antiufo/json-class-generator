using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xamasoft.JsonCSharpClassGenerator.CodeWriters
{
    public class VisualBasicCodeWriter : ICodeWriter
    {
        public string FileExtension
        {
            get { return ".vb"; }
        }

        public string DisplayName
        {
            get { return "Visual Basic .NET"; }
        }

        private const string NoRenameAttribute = "<Obfuscation(Feature:=\"renaming\", Exclude:=true)>";
        private const string NoPruneAttribute = "<Obfuscation(Feature:=\"trigger\", Exclude:=false)>";

        public string GetTypeName(JsonType type, IJsonClassGeneratorConfig config)
        {
            var arraysAsLists = config.ExplicitDeserialization;

            switch (type.Type)
            {
                case JsonTypeEnum.Anything: return "Object";
                case JsonTypeEnum.Array: return arraysAsLists ? "IList(Of " + GetTypeName(type.InternalType, config) + ")" : GetTypeName(type.InternalType, config) + "()";
                case JsonTypeEnum.Dictionary: return "Dictionary(Of String, " + GetTypeName(type.InternalType, config) + ")";
                case JsonTypeEnum.Boolean: return "Boolean";
                case JsonTypeEnum.Float: return "Double";
                case JsonTypeEnum.Integer: return "Integer";
                case JsonTypeEnum.Long: return "Long";
                case JsonTypeEnum.Date: return "DateTime";
                case JsonTypeEnum.NonConstrained: return "Object";
                case JsonTypeEnum.NullableBoolean: return "Boolean?";
                case JsonTypeEnum.NullableFloat: return "Double?";
                case JsonTypeEnum.NullableInteger: return "Integer?";
                case JsonTypeEnum.NullableLong: return "Long?";
                case JsonTypeEnum.NullableDate: return "DateTime?";
                case JsonTypeEnum.NullableSomething: return "Object";
                case JsonTypeEnum.Object: return type.AssignedName;
                case JsonTypeEnum.String: return "String";
                default: throw new System.NotSupportedException("Unsupported json type");
            }
        }


        public void WriteClass(IJsonClassGeneratorConfig config, StreamWriter sw, string className, FieldInfo[] fields, bool isRoot, bool hasSecondaryClasses)
        {
            var visibility = config.InternalVisibility ? "Friend" : "Public";
            var applyNoRenamingAttribute = config.ApplyObfuscationAttributes && !config.UsePascalCase;
            var applyNoPruneAttribute = config.ApplyObfuscationAttributes && config.UseProperties;

            sw.WriteLine("' JSON C# Class Generator");
            sw.WriteLine("' http://www.xamasoft.com/json-csharp-class-generator");
            sw.WriteLine();
            sw.WriteLine("Imports System");
            sw.WriteLine("Imports System.Collections.Generic");
            if (applyNoRenamingAttribute || applyNoPruneAttribute)
                sw.WriteLine("Imports System.Reflection");
            if (config.UsePascalCase)
                sw.WriteLine("Imports Newtonsoft.Json");
            sw.WriteLine("Imports Newtonsoft.Json.Linq");
            if (config.SecondaryNamespace != null && isRoot && hasSecondaryClasses && !config.UseNestedClasses)
            {
                sw.WriteLine("Imports {0}", config.SecondaryNamespace);
            }
            sw.WriteLine();
            sw.WriteLine("Namespace Global.{0}", isRoot && !config.UseNestedClasses ? config.Namespace : (config.SecondaryNamespace ?? config.Namespace));
            sw.WriteLine();

            if (config.UseNestedClasses)
            {
                sw.WriteLine("    {0} Partial Class {1}", visibility, config.MainClass);
                if (!isRoot)
                {
                    if (applyNoRenamingAttribute) sw.WriteLine("        " + NoRenameAttribute);
                    if (applyNoPruneAttribute) sw.WriteLine("        " + NoPruneAttribute);
                    sw.WriteLine("        {0} Class {1}", visibility, className);
                }
            }
            else
            {
                if (applyNoRenamingAttribute) sw.WriteLine("    " + NoRenameAttribute);
                if (applyNoPruneAttribute) sw.WriteLine("    " + NoPruneAttribute);
                sw.WriteLine("    {0} Class {1}", visibility, className);
            }

            var prefix = config.UseNestedClasses && !isRoot ? "            " : "        ";

            WriteClassMembers(config, sw, fields, prefix);

            if (config.UseNestedClasses && !isRoot)
                sw.WriteLine("        End Class");

            sw.WriteLine("    End Class");


            sw.WriteLine("End Namespace");


        }


        private void WriteClassMembers(IJsonClassGeneratorConfig config, StreamWriter sw, FieldInfo[] fields, string prefix)
        {
            foreach (var field in fields)
            {
                if (config.UsePascalCase)
                {
                    sw.WriteLine();
                    sw.WriteLine(prefix + "<JsonProperty(\"{0}\")>", field.JsonMemberName);
                }

                if (config.UseProperties)
                {
                    sw.WriteLine(prefix + "Public Property {1} As {0}", field.Type.GetCSharpType(), field.MemberName);
                }
                else
                {
                    sw.WriteLine(prefix + "Public {1} As {0}", field.Type.GetCSharpType(), field.MemberName);
                }
            }

        }



    }
}