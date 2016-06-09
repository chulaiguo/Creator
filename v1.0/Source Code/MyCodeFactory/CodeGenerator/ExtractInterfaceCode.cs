using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class ExtractInterfaceCode
    {
        private Assembly _assembly = null;

        public ExtractInterfaceCode(Assembly assembly)
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

            writer.WriteLine("namespace {0}.I{1}", splits[0], splits[1]);
            writer.WriteLine("{");

            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic)
                    continue;

                if (item.Name == "DataServiceFactory")
                    continue;

                writer.WriteLine("\tpublic partial interface I{0}", item.Name);
                writer.WriteLine("\t{");

                //self class
                MethodInfo[] methods = item.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                foreach (MethodInfo info in methods)
                {
                    if(info.IsVirtual)
                        continue;

                    string paras = this.GetParas(info);
                    string returnType = info.ReturnType.ToString();
                    if(info.ReturnType == typeof(void))
                    {
                        returnType = "void";
                    }
                    writer.WriteLine("\t\t{0} {1}({2});", returnType, info.Name, paras);
                }

                //base class
                Type baseType = item.BaseType;
                if (baseType != null && baseType != typeof(object))
                {
                    methods = baseType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                    foreach (MethodInfo info in methods)
                    {
                        if (info.Name.StartsWith("Save") || info.Name.StartsWith("Purge"))
                            continue;

                        string paras = this.GetParas(info);
                        string returnType = info.ReturnType.ToString();
                        if (info.ReturnType == typeof (void))
                        {
                            returnType = "void";
                        }
                        writer.WriteLine("\t\t{0} {1}({2});", returnType, info.Name, paras);
                    }
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
    }
}