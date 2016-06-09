using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class DataServiceHelp
    {
        private Type[] _types = null;
        private string _projectName = string.Empty;

        public DataServiceHelp(Type[] types, string projectName)
        {
            this._types = types;
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
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this._projectName);
            writer.WriteLine("using {0}.IDataCompressionService;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.UserCaseService", this._projectName);
            writer.WriteLine("{");
        }

        private void WriteContent(StringWriter writer)
        {
            foreach (Type item in _types)
            {
                string objName;
                if(item.Name.EndsWith("Ex"))
                {
                    objName = item.Name.Substring(1, item.Name.Length - "DataServiceEx".Length - 1);
                }
                else
                {
                    objName = item.Name.Substring(1, item.Name.Length - "DataService".Length - 1);
                }
                writer.WriteLine("\tinternal static partial class {0}Help", objName);
                writer.WriteLine("\t{");

                MethodInfo[] methods = item.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (MethodInfo method in methods)
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", method.ReturnType.Name, method.Name, GetParas(method));
                    writer.WriteLine("\t\t{");
                    if (method.ReturnType.Name.EndsWith("Collection"))
                    {
                        writer.WriteLine("\t\t\tbyte[] data = DataServiceBuilder.Get{0}DataService.{1}({2});", objName, method.Name,
                                         this.GetParaNameList(method));
                        writer.WriteLine("\t\t\treturn Cheke.Compression.DecompressToObject(data) as {0};", method.ReturnType.Name);
                    }
                    else
                    {
                        writer.WriteLine("\t\t\treturn DataServiceBuilder.Get{0}DataService.{1}({2});", objName, method.Name,
                                         this.GetParaNameList(method));
                    }
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
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

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }
    }
}