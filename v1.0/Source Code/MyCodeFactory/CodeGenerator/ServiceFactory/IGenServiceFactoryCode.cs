using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.ServiceFactory
{
    public class IGenServiceFactoryCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public IGenServiceFactoryCode(Assembly assembly, string projectName)
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
            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IFacadeService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial interface IServiceFactory");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("Service"))
                    continue;

                string serviceName = item.Name.Substring(0, item.Name.Length - 11) + "Service";

                writer.WriteLine("\t\tI{0} Get{0}(string userid, string password);", serviceName);
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