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

namespace JsonCSharpClassGenerator
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
