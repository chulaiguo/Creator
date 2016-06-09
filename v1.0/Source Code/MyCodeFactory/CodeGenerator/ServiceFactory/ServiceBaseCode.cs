using System.IO;
using System.Text;

namespace CodeGenerator.ServiceFactory
{
    public class ServiceBaseCode
    {
        private string _projectName = string.Empty;

        public ServiceBaseCode(string projectName)
        {
            this._projectName = projectName;
        }

        public string GenServiceBaseCode()
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
            writer.WriteLine("namespace {0}.FacadeService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class ServiceBase : MarshalByRefObject");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate string _userId = string.Empty;");
            writer.WriteLine("\t\tprivate string _password = string.Empty;");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic ServiceBase(string userid, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._userId = userid;");
            writer.WriteLine("\t\t\tthis._password = password;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected string UserId");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn _userId;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();


            writer.WriteLine("\t\tprotected string Password");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn _password;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected IDataServiceFactoryEx DataServiceFactory");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn (IDataServiceFactoryEx)ClassBuilder.GetFactory(\"DataServiceFactory\");");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}