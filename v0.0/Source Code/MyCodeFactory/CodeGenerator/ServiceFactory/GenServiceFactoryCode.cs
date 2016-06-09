using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.ServiceFactory
{
    public class GenServiceFactoryCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public GenServiceFactoryCode(Assembly assembly, string projectName)
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
            writer.WriteLine("using {0}.IFacadeService;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.FacadeService", this._projectName);
            writer.WriteLine("{");
            //writer.WriteLine("\tpublic partial class ServiceFactory : MarshalByRefObject, IServiceFactory");
            writer.WriteLine("\tpublic partial class ServiceFactory");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("Service"))
                    continue;

                string serverName = item.Name.Substring(0, item.Name.Length - 11) + "Service";

                writer.WriteLine("\t\tpublic I{0} Get{0}(string userid, string password)", serverName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn new {0}(userid, password);", serverName);
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