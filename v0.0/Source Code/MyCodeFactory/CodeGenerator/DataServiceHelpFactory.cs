using System;
using System.IO;
using System.Text;

namespace CodeGenerator
{
    public class DataServiceHelpFactory
    {
        private Type[] _types = null;
        private string _projectName = string.Empty;

        public DataServiceHelpFactory(Type[] types, string projectName)
        {
            this._types = types;
            this._projectName = projectName;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteContent(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this._projectName);
            writer.WriteLine("using {0}.IDataCompressionService;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.UserCaseService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tinternal static class DataServiceBuilder");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static IDataCompressionServiceFactory ServiceFactory");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{return ((IDataCompressionServiceFactory)(Cheke.ClassFactory.ClassBuilder.GetFactory(\"DataCompressionServiceFactory\")));}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            foreach (Type item in this._types)
            {
                string interfaceName = item.Name.Substring(1, item.Name.Length - "DataService".Length - 1);;
                writer.WriteLine("\t\tpublic static I{0}CompressionDataService Get{1}(SecurityToken token)", interfaceName, item.Name.Substring(1));
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn ServiceFactory.Get{0}(token);", item.Name.Substring(1));
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}