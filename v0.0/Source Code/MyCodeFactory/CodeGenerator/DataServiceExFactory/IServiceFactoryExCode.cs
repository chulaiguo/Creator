using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.DataServiceExFactory
{
    public class IServiceFactoryExCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public IServiceFactoryExCode(Assembly assembly, string projectName)
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
            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IDataService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic interface IDataServiceFactory");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("ServiceBase"))
                    continue;

                string name = item.Name.Substring(0, item.Name.Length - "Base".Length);
                writer.WriteLine("\t\tI{0} Get{0}(SecurityToken token);", name);
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