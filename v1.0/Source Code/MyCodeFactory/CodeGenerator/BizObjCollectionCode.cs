using System;
using System.IO;
using System.Text;

namespace CodeGenerator
{
    public class BizObjCollectionCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;
        private string _objectName = string.Empty;

        public BizObjCollectionCode(Type type, string projectName)
        {
            this._type = type;
            this._projectName = projectName;

            this._objectName = this._type.Name.Substring(0, this._type.Name.Length - 4);
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

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.BizObj", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[Serializable]");
            writer.WriteLine("\tpublic partial class {0}Collection : BusinessCollectionBase", this._objectName);
            writer.WriteLine("\t{");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}
