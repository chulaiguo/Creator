using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.DataServiceExFactory
{
    public class ServiceExCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;
        private Assembly _dataAssembly = null;

        public ServiceExCode(Type type, string projectName, Assembly dataAssembly)
        {
            this._type = type;
            this._projectName = projectName;
            this._dataAssembly = dataAssembly;
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
            writer.WriteLine("using {0}.Data;", this._projectName);
            writer.WriteLine("using {0}.DataServiceBase;", this._projectName);
            writer.WriteLine("using {0}.IDataService;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            string name = this._type.Name.Substring(0, this._type.Name.Length - "Base".Length);
            writer.WriteLine("namespace {0}.DataService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class {0} : {0}Base, I{0}", name);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic {0}(string connectionString, SecurityToken token)", name);
            writer.WriteLine("\t\t\t: base(connectionString, token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\t#region Override Children Services");
            Type dataType = this.GetDataType();
            if (dataType != null)
            {
                PropertyInfo[] properties = dataType.GetProperties();
                foreach (PropertyInfo item in properties)
                {
                    if (!item.Name.EndsWith("List"))
                        continue;

                    string name = item.Name.Substring(0, item.Name.Length - 4) + "Data";
                    writer.WriteLine("\t\tprotected override {0}ServiceBase Get{0}ServiceBase(SecurityToken token)",
                                     name);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn DataServiceFactory.Create{0}Service(token);", name);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
            }
            writer.WriteLine("\t\t#endregion");

            writer.WriteLine();
            writer.WriteLine("\t\t#region Override Selectable & Editable & Deletable & Insertable");
            writer.WriteLine("\t\t#endregion");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private Type GetDataType()
        {
            Type[] types = this._dataAssembly.GetTypes();
            foreach(Type item in types)
            {
                if (!item.IsPublic)
                    continue;

                if (this._type.Name.Contains(item.Name))
                    return item;
            }

            return null;
        }
    }
}