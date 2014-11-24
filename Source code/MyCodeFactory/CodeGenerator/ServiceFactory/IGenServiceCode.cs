using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.ServiceFactory
{
    public class IGenServiceCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        public IGenServiceCode(Type type, string projectName)
        {
            this._type = type;
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
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            string serviceName = this._type.Name.Substring(0, this._type.Name.Length - 11) + "Service";

            writer.WriteLine("namespace {0}.IFacadeService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial interface I{0}", serviceName);
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            MethodInfo[] methods = this._type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo item in methods)
            {
                if (item.Name.StartsWith("set_") || item.Name.StartsWith("get_"))
                    continue;

                if (item.Name == "DeleteByPK" || item.Name == "GetRowVersion")
                    continue;

                writer.WriteLine("\t\t{0} {1}({2});", item.ReturnType.Name, item.Name, GetParas(item));
            }
        }


        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private string GetParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat(" {0} {1},", item.ParameterType.Name, item.Name);
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}