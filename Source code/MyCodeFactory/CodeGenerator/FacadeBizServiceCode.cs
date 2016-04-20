using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class FacadeBizServiceCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;
        private string _objectName = string.Empty;

        public FacadeBizServiceCode(Assembly assembly)
        {
            this._assembly = assembly;
            string[] splits = this._assembly.GetName().Name.Split('.');
            if (splits.Length < 2)
                return;

            this._projectName = splits[0];
            this._objectName =  splits[1].Substring(1);
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
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.IFacadeService;", this._projectName);
            writer.WriteLine("using {0}.{1}Wrapper;", this._projectName, this._objectName);
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.FacadeService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class Biz{0} : ServiceBase, IBiz{0}", this._objectName);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic Biz{0}(Cheke.SecurityToken token)", this._objectName);
            writer.WriteLine("\t\t\t: base(token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

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

                string wrapperName = item.Name.Replace("Service", "Wrapper").Substring(1);
                MethodInfo[] methods = item.GetMethods();
                foreach (MethodInfo info in methods)
                {
                    writer.WriteLine("\t\tpublic {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");

                    if (info.ReturnType == typeof(void))
                    {
                        writer.WriteLine("\t\t\t{0}.{1}({2});", wrapperName, info.Name, this.GetParaNameList(info));
                    }
                    else if (info.ReturnType == typeof(DateTime))
                    {
                        writer.WriteLine("\t\t\treturn {0}.{1}({2});", wrapperName, info.Name, this.GetParaNameList(info));
                    }
                    else
                    {
                        writer.WriteLine("\t\t\treturn {0}.{1}({2});", wrapperName, info.Name, this.GetParaNameList(info));
                    }
                    writer.WriteLine("\t\t}");
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

        private string GetParaNameList(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat(" {0},", item.Name);
            }

            builder.AppendFormat(" base.OriginalToken");
            return builder.ToString();
        }
    }
}