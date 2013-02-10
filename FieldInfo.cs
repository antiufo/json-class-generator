// Copyright © 2010 Xamasoft

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamasoft.JsonCSharpClassGenerator
{
    class FieldInfo
    {

        public FieldInfo(string jsonMemberName, JsonType type, bool usePascalCase)
        {
            this.JsonMemberName = jsonMemberName;
            this.MemberName = jsonMemberName;
            if (usePascalCase) MemberName = JsonClassGenerator.ToTitleCase(MemberName);
            this.Type = type;
        }
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
                    field.Type.GetCSharpType(false),
                    field.JsonMemberName,
                    innermost.GetReaderName(),
                    -1,
                    innermost.GetCSharpType(false),
                    field.Type.GetCSharpType(false)
                    );
            }
            else if (field.Type.Type == JsonTypeEnum.Dictionary)
            {
  
                return string.Format("({1})JsonClassHelper.ReadDictionary<{2}>(JsonClassHelper.GetJToken<JObject>({0}, \"{3}\"))",
                    jobject,
                    field.Type.GetCSharpType(false),
                    field.Type.InternalType.GetCSharpType(false),
                    field.JsonMemberName,
                    field.Type.GetCSharpType(false)
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
