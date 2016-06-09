using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class ServiceBuilderCode
    {
        private string _projectName = string.Empty;

        public ServiceBuilderCode(string projectName)
        {
            this._projectName = projectName;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.IService", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.BizObj", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tinternal static partial class FacadeServiceBuilder");
            writer.WriteLine("\t{");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}