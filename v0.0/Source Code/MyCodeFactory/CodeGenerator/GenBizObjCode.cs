using System;
using System.IO;
using System.Text;

namespace CodeGenerator
{
    public class GenBizObjCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;
        private string _objectName = string.Empty;

        public GenBizObjCode(Type type, string projectName)
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
            this.WriteSaveMethod(writer);
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
            writer.WriteLine("namespace {0}.BizObj", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[Serializable]");
            writer.WriteLine("\tpublic partial class {0} : {1}", this._objectName, this._type.Name);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic {0}({1} data)", this._objectName, this._type.Name);
            writer.WriteLine("\t\t\t: base(data)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteSaveMethod(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic override Result Save(string userid, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn Save(base.GetChanges(), userid, password);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static Result Save(BusinessBase entity, string userid, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn ServiceBuilder.GetOrderDataService(userid, password).Save(entity as {0});", this._type.Name);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static Result Save(BusinessCollectionBase list, string userid, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn ServiceBuilder.GetOrderDataService(userid, password).Save(list as {0}Collection);", this._type.Name);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}
