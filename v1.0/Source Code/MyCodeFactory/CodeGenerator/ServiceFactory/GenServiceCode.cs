using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.ServiceFactory
{
    public class GenServiceCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        public GenServiceCode(Type type, string projectName)
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
            writer.WriteLine("using {0}.IDataServiceEx;", this._projectName);
            writer.WriteLine("using {0}.IFacadeService;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            string className = this._type.Name.Substring(0, this._type.Name.Length - 11) + "Service";
            writer.WriteLine("namespace {0}.FacadeService", this._projectName);
            writer.WriteLine("{");
            //writer.WriteLine("\tpublic partial class {0} : ServiceBase, I{0}", className);
            writer.WriteLine("\tpublic partial class {0}", className);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic {0}(string userid, string password)", className);
            writer.WriteLine("\t\t\t: base(userid, password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate I{0}Ex DataService", this._type.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn base.DataServiceFactory.Get{0}(base.UserId, base.Password);", this._type.Name);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            MethodInfo[] methods =
                this._type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo item in methods)
            {
                if (item.Name.StartsWith("set_") || item.Name.StartsWith("get_"))
                    continue;

                if (item.Name == "DeleteByPK" || item.Name == "GetRowVersion")
                    continue;

                writer.WriteLine("\t\tpublic {0} {1}({2})", item.ReturnType.Name, item.Name, GetParas(item));
                writer.WriteLine("\t\t{");
                
                string paras = this.GetParaNameList(item);
                writer.WriteLine("\t\t\treturn this.DataService.{0}({1});", item.Name, paras);    
                
                writer.WriteLine("\t\t}");
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
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}