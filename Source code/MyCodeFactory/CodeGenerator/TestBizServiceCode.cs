using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class TestBizServiceCode
    {
        private readonly Assembly _assembly = null;
       
        public TestBizServiceCode(Assembly assembly)
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
            writer.WriteLine("using System;");
            writer.WriteLine("using D3000.IFacadeService;");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("namespace D3000.FacadeService");
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class BizPanelService : ServiceBase, IBizPanelService");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic BizPanelService(Cheke.SecurityToken token)");
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
                if(!item.IsPublic)
                    continue;

                if(item.Name == "DataServiceFactory")
                    continue;

                string objectName = item.Name.Substring(0, item.Name.Length - "DataService".Length);
                MethodInfo[] methods = item.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public); 
                foreach (MethodInfo info in methods)
                {
                    string methodName = this.GetMethodName(objectName, info);
                    if(methodName.Length == 0)
                        continue;

                    writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType, methodName, this.GetParas(info));
                    writer.WriteLine("\t\t{");

                    //ParameterInfo[] paras = info.GetParameters();
                    //foreach (ParameterInfo para in paras)
                    //{
                    //    if(para.ParameterType != typeof(DateTime))
                    //        continue;

                    //    writer.WriteLine("\t\t\tDateTime {0}_utc = new DateTime({0}.Ticks, DateTimeKind.Utc);", para.Name);
                    //    writer.WriteLine("\t\t\t{0} = {0}_utc.ToLocalTime();", para.Name);
                    //}

                    //if(info.ReturnType.IsValueType)
                    //{
                    //    if (info.ReturnType == typeof(bool))
                    //    {
                    //        writer.WriteLine("\t\t\treturn false;");
                    //    }
                    //    else
                    //    {
                    //        writer.WriteLine("\t\t\treturn 0;");
                    //    }
                    //}
                    //else
                    //{
                    //    writer.WriteLine("\t\t\treturn null;");
                    //}

                    writer.WriteLine("\t\t\treturn E150.ViewObj.{0}.{1}({2});", objectName, info.Name, this.GetParaNameList(info));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
            }
        }

        private string GetMethodName(string objectName, MethodInfo info)
        {
            if (info.Name.StartsWith("Save") || info.Name == "ClearDownloadFlag")
                return string.Empty;

            if (info.ReturnType.Name.EndsWith("Collection") || info.ReturnType.Name.EndsWith("Data"))
                return string.Empty;

            if (info.Name == "TryDeleteByPK" || info.Name == "UpdateTenant" || info.Name == "ExecuteNonQuerySql"
                || info.Name == "GetCurrentTime")
                return string.Empty;

            if (info.Name.StartsWith("GetPendingTo"))
            {
                return info.Name;
            }

            if (info.Name.StartsWith("IsPending"))
            {
                return "Is" + objectName + info.Name.Substring("Is".Length);
            }

            if(info.Name.StartsWith("GetPending"))
            {
                return "Get" + objectName + info.Name.Substring("Get".Length);
            }

            if (info.Name.StartsWith("Redownload"))
            {
                return "Redownload" + objectName + info.Name.Substring("Redownload".Length);
            }

            return info.Name;
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