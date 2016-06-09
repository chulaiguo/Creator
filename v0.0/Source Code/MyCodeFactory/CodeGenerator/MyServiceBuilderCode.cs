using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class MyServiceBuilderCode
    {
        private Assembly _assembly = null;

        public MyServiceBuilderCode(Assembly assembly)
        {
            this._assembly = assembly;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.WriteContent(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
        }

        private void WriteContent(StringWriter writer)
        {
            string[] splits = this._assembly.GetName().Name.Split('.');
            if(splits.Length < 2)
                return;

            writer.WriteLine("namespace {0}.{1}Wrapper", splits[0], splits[1].Substring(1));
            writer.WriteLine("{");
            writer.WriteLine("\tinternal static class ServiceBuilder");
            writer.WriteLine("\t{");

            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic)
                    continue;

                if (item.Name == "IServiceFactory")
                {
                    writer.WriteLine("\t\tprivate static {0} ServiceFactory", item.FullName);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget {{ return ({0})Cheke.ClassFactory.ClassBuilder.GetFactory(\"{1}.{2}Factory\"); }}", item.FullName, splits[0], splits[1].Substring(1));
                    writer.WriteLine("\t\t}");
                }
                else
                {
                    writer.WriteLine("\t\tinternal static {0} Get{1}()", item.FullName, item.Name.Substring(1));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn ServiceFactory.Get{0}();", item.Name.Substring(1));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
            }

            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}