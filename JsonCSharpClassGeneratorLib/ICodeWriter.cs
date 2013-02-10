using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xamasoft.JsonClassGenerator
{
    public interface ICodeWriter
    {
        string FileExtension { get; }
        string DisplayName { get; }
        string GetTypeName(JsonType type, IJsonClassGeneratorConfig config);
        void WriteClass(IJsonClassGeneratorConfig config, StreamWriter sw, JsonType type);
        void WriteFileStart(IJsonClassGeneratorConfig config, StreamWriter sw);
        void WriteFileEnd(IJsonClassGeneratorConfig config, StreamWriter sw);
        void WriteNamespaceStart(IJsonClassGeneratorConfig config, StreamWriter sw, bool root);
        void WriteNamespaceEnd(IJsonClassGeneratorConfig config, StreamWriter sw, bool root);
    }
}
