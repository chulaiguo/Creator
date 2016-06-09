using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class ViewObjOverrideCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public ViewObjOverrideCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
            this._projectName = projectName;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            writer.WriteLine("namespace {0}.ViewObj", this._projectName);
            writer.WriteLine("{");
            this.WriteUsing(writer);
            this.WriteOverrideCode(writer);
            writer.WriteLine("}");

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("\tusing System;");
            writer.WriteLine("\tusing System.Data;");
            writer.WriteLine("\tusing Cheke;");
            writer.WriteLine("\tusing Cheke.BusinessEntity;");
            writer.WriteLine("\tusing {0}.BizObj;", this._projectName);

            writer.WriteLine();
        }

        private void WriteOverrideCode(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("Data"))
                    continue;

                string className = item.Name.Substring(0, item.Name.Length - 4);
                this.BeginWrite(writer, className);
                this.WriteContent(writer, item);
                this.EndWrite(writer);
                writer.WriteLine();
            }
        }


        private void BeginWrite(StringWriter writer, string className)
        {
            writer.WriteLine("\tpublic partial class {0}", className);
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer, Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                if (!item.CanRead || !item.CanWrite || item.PropertyType != typeof(string))
                    continue;

                if (item.Name == "PKString" || item.Name == "TableName" || item.Name == "BrokenRulesString"
                    || item.Name == "DBTableName" || item.Name == "Password" || item.Name == "KeyName")
                    continue;

                writer.WriteLine("\t\tpublic override string {0}", item.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn base.{0}.ToUpper();", item.Name);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbase.{0} = value;", item.Name);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
        }
    }
}
