using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.DataServiceExFactory
{
    public class ServiceFactoryExCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public ServiceFactoryExCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
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
            writer.WriteLine("using System.Configuration;");
            writer.WriteLine("using {0}.IDataService;", this._projectName);
            writer.WriteLine("using {0}.DataService.Utils;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DataService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class DataServiceFactory : MarshalByRefObject, IDataServiceFactory");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tinternal static readonly string _ConnectionString = ConfigurationManager.AppSettings[\"DB:{0}\"];", this._projectName);
            writer.WriteLine();
            this.WriteInterfaceImpl(writer);

            writer.WriteLine("\t\t#region static Service builder");
            this.WriteStaticFactory(writer);
            writer.WriteLine("\t\t#endregion");
        }

        private void WriteInterfaceImpl(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("ServiceBase"))
                    continue;

                string name = item.Name.Substring(0, item.Name.Length - "Base".Length);
                writer.WriteLine("\t\tpublic I{0} Get{0}(SecurityToken token)", name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (!Authentication.IsAnonymousUser(token) && !Authentication.IsAuthenticated(token))");
                writer.WriteLine("\t\t\t\treturn null;");
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn Create{0}(token);", name);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        private void WriteStaticFactory(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("ServiceBase"))
                    continue;

                string name = item.Name.Substring(0, item.Name.Length - "Base".Length);
                writer.WriteLine("\t\tinternal static {0} Create{0}(SecurityToken token)", name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn new {0}(_ConnectionString, token);", name);
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