using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.TestFactory
{
    public class DataServiceBuilderCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public DataServiceBuilderCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
            this._projectName = projectName;
        }

        public string GenBuilderCode()
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
            writer.WriteLine("using {0}.IDataServiceEx;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Excel", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tinternal static class ServiceBuilder");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("Service"))
                    continue;

                writer.WriteLine("\t\tinternal static I{0}Ex {0}", item.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn ServiceFactory.Get{0}(UserId, Password);", item.Name);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\tprivate static IDataServiceFactoryEx ServiceFactory");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn (IDataServiceFactoryEx) ClassBuilder.GetFactory(\"DataServiceFactory\");");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");

            writer.WriteLine();
            writer.WriteLine("\t\tprivate static string UserId");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"admin\";");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");

            writer.WriteLine();
            writer.WriteLine("\t\tprivate static string Password");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"123\";");
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