using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class FacadeIBizServiceCode
    {
        private Assembly _assembly = null;

        public FacadeIBizServiceCode(Assembly assembly)
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

            writer.WriteLine("namespace {0}.IFacadeService", splits[0]);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic interface IBiz{0}", splits[1].Substring(1));
            writer.WriteLine("\t{");
            this.WriteMethods(writer);
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private void WriteMethods(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (item.Name == "IServiceFactory")
                    continue;

                MethodInfo[] methods = item.GetMethods();
                foreach (MethodInfo info in methods)
                {
                   writer.WriteLine("\t\t{0} {1}({2});", info.ReturnType, info.Name, this.GetParas(info));
                   writer.WriteLine();
                }
            }
        }

        private string GetParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat(" {0} {1},", item.ParameterType.FullName, item.Name);
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}