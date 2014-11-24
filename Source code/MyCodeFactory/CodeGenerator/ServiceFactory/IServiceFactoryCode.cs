using System.IO;
using System.Text;

namespace CodeGenerator.ServiceFactory
{
    public class IServiceFactoryCode
    {
        private string _projectName = string.Empty;

        public IServiceFactoryCode(string projectName)
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
            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IFacadeService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial interface IServiceFactory");
            writer.WriteLine("\t{");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}