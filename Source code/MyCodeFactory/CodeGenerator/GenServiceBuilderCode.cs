using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class GenServiceBuilderCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public GenServiceBuilderCode(Assembly assembly, string projectName)
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
            writer.WriteLine("using Cheke.ClassFactory;");
            writer.WriteLine("using {0}.IService", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.BizObj", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tinternal static partial class FacadeServiceBuilder");
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

                writer.WriteLine("\t\tinternal static I{0} Get{0}()", serviceName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn FacadeServiceFactory.Get{0}(SecurityToken.UserId, SecurityToken.Password);", serviceName);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\tprivate static IServiceFactory FacadeServiceFactory");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn (IServiceFactory) ClassBuilder.GetFactory(\"FacadeServiceFactory\");");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}