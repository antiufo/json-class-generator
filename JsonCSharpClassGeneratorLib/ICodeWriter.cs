using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xamasoft.JsonCSharpClassGenerator
{
    public interface ICodeWriter
    {
        string FileExtension { get; }
        string GetTypeName(JsonType type, IJsonClassGeneratorConfig config);
        void WriteClass(IJsonClassGeneratorConfig config, StreamWriter sw, string className, FieldInfo[] fields, bool isRoot, bool hasSecondaryClasses);
    }
}
