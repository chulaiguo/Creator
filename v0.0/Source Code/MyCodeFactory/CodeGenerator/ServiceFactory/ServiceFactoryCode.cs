using System.IO;
using System.Text;

namespace CodeGenerator.ServiceFactory
{
    public class ServiceFactoryCode
    {
        private string _projectName = string.Empty;

        public ServiceFactoryCode(string projectName)
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
            writer.WriteLine("using System.Configuration;");
            writer.WriteLine("using {0}.IFacadeService;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.FacadeService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class ServiceFactory : MarshalByRefObject, IServiceFactory");
            writer.WriteLine("\t{");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}