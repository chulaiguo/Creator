using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.DataServiceExFactory
{
    public class IServiceExCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        public IServiceExCode(Type type, string projectName)
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
            this.WriteContent(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.Data;", this._projectName);


            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IDataService", this._projectName);
            writer.WriteLine("{");
            string name = this._type.Name.Substring(0, this._type.Name.Length - "Base".Length);
            writer.WriteLine("\tpublic partial interface I{0}", name);
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}