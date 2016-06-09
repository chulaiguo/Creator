using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.DataServiceExFactory
{
    public class InnerServiceBuilderCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public InnerServiceBuilderCode(Assembly assembly, string projectName)
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
            writer.WriteLine("using System.Configuration;");
            writer.WriteLine("using {0}.DataService;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DataServiceEx", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class DataServiceBuilder : DataServiceFactory");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("Service"))
                    continue;

                writer.WriteLine("\t\tpublic override {0} Get{0}(string userid, string password)", item.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn new {0}Ex(ConfigurationManager.AppSettings[\"DB:{1}\"], userid, password);", item.Name, this._projectName);
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