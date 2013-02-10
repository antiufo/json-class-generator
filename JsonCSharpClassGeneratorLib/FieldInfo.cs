// Copyright © 2010 Xamasoft

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamasoft.JsonCSharpClassGenerator
{
    public class FieldInfo
    {

        public FieldInfo(IJsonClassGeneratorConfig generator, string jsonMemberName, JsonType type, bool usePascalCase)
        {
            this.generator = generator;
            this.JsonMemberName = jsonMemberName;
            this.MemberName = jsonMemberName;
            if (usePascalCase) MemberName = JsonClassGenerator.ToTitleCase(MemberName);
            this.Type = type;
        }
        private IJsonClassGeneratorConfig generator;
        public string MemberName { get; private set; }
        public string JsonMemberName { get; private set; }
        public JsonType Type { get; private set; }

        public string GetGenerationCode(string jobject)
        {
            var field = this;
            if (field.Type.Type == JsonTypeEnum.Array)
            {
                var innermost = field.Type.GetInnermostType();
                return string.Format("({1})JsonClassHelper.ReadArray<{5}>(JsonClassHelper.GetJToken<JArray>({0}, \"{2}\"), JsonClassHelper.{3}, typeof({6}))",
                    jobject,
                    field.Type.GetCSharpType(),
                    field.JsonMemberName,
                    innermost.GetReaderName(),
                    -1,
                    innermost.GetCSharpType(),
                    field.Type.GetCSharpType()
                    );
            }
            else if (field.Type.Type == JsonTypeEnum.Dictionary)
            {
  
                return string.Format("({1})JsonClassHelper.ReadDictionary<{2}>(JsonClassHelper.GetJToken<JObject>({0}, \"{3}\"))",
                    jobject,
                    field.Type.GetCSharpType(),
                    field.Type.InternalType.GetCSharpType(),
                    field.JsonMemberName,
                    field.Type.GetCSharpType()
                    );
            }
            else
            {
                return string.Format("JsonClassHelper.{1}(JsonClassHelper.GetJToken<{2}>({0}, \"{3}\"))",
                    jobject,
                    field.Type.GetReaderName(),
                    field.Type.GetJTokenType(),
                    field.JsonMemberName);
            }

        }


    }
}
