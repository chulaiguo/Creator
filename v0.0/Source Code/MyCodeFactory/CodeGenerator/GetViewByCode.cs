using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class GetViewByCode
    {
        private Assembly _assembly = null;

        public GetViewByCode(Assembly assembly)
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

            writer.WriteLine("namespace {0}.{1}", splits[0], splits[1]);
            writer.WriteLine("{");

            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic)
                    continue;

                if (item.Name == "DataServiceFactory")
                    continue;

                writer.WriteLine("\tpublic partial class {0}", item.Name);
                writer.WriteLine("\t{");

                MethodInfo[] methods = item.GetMethods(BindingFlags.DeclaredOnly|BindingFlags.Instance | BindingFlags.Public);
                foreach (MethodInfo info in methods)
                {
                    if (!info.IsPublic || info.IsStatic)
                        continue;

                    if (info.IsVirtual)
                        continue;

                    if (!info.Name.StartsWith("GetBy"))
                        continue;

                    string returnType = info.ReturnType.ToString();
                    if (!returnType.EndsWith("DataCollection"))
                        continue;

                    returnType = returnType.Substring(0, returnType.Length - "DataCollection".Length);
                    returnType = returnType + "ViewCollection";

                    string paras = this.GetParas(info);
                    string methodName = info.Name.Substring("GetBy".Length);
                    methodName = "GetViewBy" + methodName;

                    writer.WriteLine("\t\tpublic {0} {1}({2})", returnType, methodName, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn new {0}(this.{1}({2}));", returnType, info.Name, this.GetParaNameList(info));
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
            
            writer.WriteLine("}");
        }

        private string GetParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat(" {0} {1},", item.ParameterType.FullName, item.Name);
            }

            return builder.ToString().TrimEnd(',').Trim();
        }

        private string GetParaNameList(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat(" {0},", item.Name);
            }

            return builder.ToString().TrimEnd(',').Trim();
        }
    }
}