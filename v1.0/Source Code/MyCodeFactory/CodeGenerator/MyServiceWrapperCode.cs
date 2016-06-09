using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class MyServiceWrapperCode
    {
        private Type _itemType = null;

        public MyServiceWrapperCode(Type type)
        {
            this._itemType = type;
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
            string[] splits = this._itemType.Namespace.Split('.');
            if (splits.Length < 2)
                return;

            writer.WriteLine("namespace {0}.{1}Wrapper", splits[0], splits[1].Substring(1));
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class {0}Wrapper", this._itemType.Name.Substring(1, this._itemType.Name.Length - 8));
            writer.WriteLine("\t{");
            this.WriteMethods(this._itemType, writer);
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private void WriteMethods(Type type, StringWriter writer)
        {
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo info in methods)
            {
                if(info.ReturnType == typeof(void))
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");

                    writer.WriteLine("\t\t\tServiceBuilder.Get{0}().{1}({2});", type.Name.Substring(1), info.Name, this.GetParaNameList(info));

                    writer.WriteLine("\t\t}");
                }
                else
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn ServiceBuilder.Get{0}().{1}({2});", type.Name.Substring(1),
                                     info.Name, this.GetParaNameList(info));
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

            return builder.ToString().Trim(',');
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