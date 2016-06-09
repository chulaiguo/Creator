using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class DBSchemaCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public DBSchemaCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
            this._projectName = projectName;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.BeginWrite(writer);
            this.WriteContent(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Schema", this._projectName);
            writer.WriteLine("{");
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type type in types)
            {
                if(!type.IsClass || !type.IsPublic)
                    continue;

                if(type.IsAbstract || type.IsInterface || type.IsNested)
                    continue;

                if (!type.Name.EndsWith("Data"))
                    continue;

                string tableName = type.Name.Substring(0, type.Name.Length - 4);
                writer.WriteLine("\tpublic partial class {0}Schema", tableName);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tpublic const string TableName = \"{0}\";", tableName);
                writer.WriteLine("\t\tpublic const string TableAlias = \"{0}\";", tableName);
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }
    }
}
