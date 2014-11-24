using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class BizServiceCode
    {
        private readonly Assembly _assembly = null;
        private readonly string _className = string.Empty;
        private readonly string _projectName = string.Empty;

        public BizServiceCode(Assembly assembly, string className, string projectName)
        {
            this._assembly = assembly;
            this._className = className;
            this._projectName = projectName;
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
            writer.WriteLine("using {0};", this._assembly.GetName().Name);
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.FacadeService", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class Biz{0} : ServiceBase", this._className);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic Biz{0}(Cheke.SecurityToken token)", this._className);
            writer.WriteLine("\t\t\t: base(token)");
            writer.WriteLine("\t\t{"); 
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic)
                    continue;

                this.WriteMethods(item, writer);
            }

            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private void WriteMethods(Type type, StringWriter writer)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo info in methods)
            {
                if (info.ReturnType == typeof(void))
                {
                    writer.WriteLine("\t\tpublic {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");

                    writer.WriteLine("\t\t\tnew {0}(base.OriginalToken).{1}({2});", type.Name, info.Name, this.GetParaNameList(info));

                    writer.WriteLine("\t\t}");
                }
                else
                {
                    writer.WriteLine("\t\tpublic {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn new {0}(base.OriginalToken).{1}({2});", type.Name, info.Name, this.GetParaNameList(info));
                    writer.WriteLine("\t\t}");

                }
                writer.WriteLine();
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

            return builder.ToString().TrimEnd(',');
        }
    }
}