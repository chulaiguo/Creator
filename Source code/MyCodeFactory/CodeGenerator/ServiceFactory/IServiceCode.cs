using System;
using System.IO;
using System.Text;

namespace CodeGenerator.ServiceFactory
{
    public class IServiceCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        public IServiceCode(Type type, string projectName)
        {
            this._type = type;
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
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            string serviceName = this._type.Name.Substring(0, this._type.Name.Length - 11) + "Service";

            writer.WriteLine("namespace {0}.IFacadeService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial interface I{0}", serviceName);
            writer.WriteLine("\t{");
        }


        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}