using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class TestIBizServiceCode
    {
        private readonly Assembly _assembly = null;

        public TestIBizServiceCode(Assembly assembly)
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
            writer.WriteLine("namespace D3000.IFacadeService");
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial interface IBizPanelService");
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
                if (!item.IsPublic)
                    continue;

                if (item.Name == "DataServiceFactory")
                    continue;

                string objectName = item.Name.Substring(0, item.Name.Length - "DataService".Length);
                MethodInfo[] methods = item.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public); 
                foreach (MethodInfo info in methods)
                {
                    string methodName = this.GetMethodName(objectName, info);
                    if (methodName.Length == 0)
                        continue;

                   writer.WriteLine("\t\t{0} {1}({2});", info.ReturnType, methodName, this.GetParas(info));
                   writer.WriteLine();
                }
            }
        }

        private string GetMethodName(string objectName, MethodInfo info)
        {
            if (info.ReturnType.Name.StartsWith("Save"))
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

            if (info.Name.StartsWith("GetPending"))
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
    }
}