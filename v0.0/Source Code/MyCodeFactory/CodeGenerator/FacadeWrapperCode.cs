using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class FacadeWrapperCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public FacadeWrapperCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
            this._projectName = projectName;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteFacadeService(writer);
            this.WriteFacadeBuilder(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Specialized;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.ClassFactory;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this._projectName);
            writer.WriteLine("using {0}.IFacadeService;", this._projectName);
            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ViewObj", this._projectName);
            writer.WriteLine("{");
        }

        private void WriteFacadeBuilder(StringWriter writer)
        {
            writer.WriteLine("\tinternal static class FacadeServiceBuilder");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate static IFacadeServiceFactory ServiceFactory");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget {{ return ((IFacadeServiceFactory)(ClassBuilder.GetFactory(\"{0}.FacadeServiceFactory\"))); }}", this._projectName);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (item.Name == "IFacadeServiceFactory")
                    continue;

                writer.WriteLine("\t\tinternal static {0} {1}", item.Name, item.Name.Substring(1));
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return ServiceFactory.Get{0}(Identity.Token); }}", item.Name.Substring(1));
                writer.WriteLine("\t\t}"); 
                writer.WriteLine();
            }

            writer.WriteLine("\t}");
        }

        private void WriteFacadeService(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if(item.Name == "IFacadeServiceFactory")
                    continue;

                string className = item.Name.Substring(1, item.Name.Length - 8);
                writer.WriteLine("\tpublic class {0}", className);
                writer.WriteLine("\t{");
                this.WriteFacadeMethods(item, writer);
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteFacadeMethods(Type type, StringWriter writer)
        {
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo info in methods)
            {
                if(info.ReturnType.Name.EndsWith("Data"))
                {
                    string nameWithoutData = info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - 4);
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", nameWithoutData, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");

                    writer.WriteLine("\t\t\t{3} data = FacadeServiceBuilder.{0}.{1}({2});", type.Name.Substring(1), info.Name, this.GetParaNameList(info), info.ReturnType);
                    writer.WriteLine("\t\t\tif (data == null)");
                    writer.WriteLine("\t\t\t\treturn null;");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t{0} entity = new {0}(data);", nameWithoutData);
                    writer.WriteLine("\t\t\treturn entity;");
                    writer.WriteLine("\t\t}");
                }
                else if(info.ReturnType.Name.EndsWith("DataCollection"))
                {
                    string nameWithoutData = info.ReturnType.Name.Substring(0, info.ReturnType.Name.Length - 14);
                    writer.WriteLine("\t\tpublic static {0}Collection {1}({2})", nameWithoutData, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");

                    writer.WriteLine("\t\t\t{3}Collection list = new {3}Collection(FacadeServiceBuilder.{0}.{1}({2}));", type.Name.Substring(1), info.Name, this.GetParaNameList(info), nameWithoutData);
                    writer.WriteLine("\t\t\tIdentity.ResetQueryParas();");
                    writer.WriteLine("\t\t\treturn list;");
                    writer.WriteLine("\t\t}");
                }
                else if(info.ReturnType == typeof(void))
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");

                    writer.WriteLine("\t\t\tFacadeServiceBuilder.{0}.{1}({2});", type.Name.Substring(1), info.Name, this.GetParaNameList(info));
                    writer.WriteLine("\t\t}");
                }
                else if (info.ReturnType == typeof(DateTime))
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");

                    writer.WriteLine("\t\t\treturn FacadeServiceBuilder.{0}.{1}({2});", type.Name.Substring(1), info.Name, this.GetParaNameList(info));
                    writer.WriteLine("\t\t}");
                }
                else
                {
                    if(info.Name == "Login")
                    {
                        writer.WriteLine("\t\tpublic static Result Login(string userId, string password)");
                        writer.WriteLine("\t\t{");

                        writer.WriteLine("\t\t\tIdentity.SetToken(userId, password);");
                        writer.WriteLine("\t\t\treturn FacadeServiceBuilder.{0}.Login();", type.Name.Substring(1));

                        writer.WriteLine("\t\t}");
                    }
                    else if (info.Name == "RecoverPassword")
                    {
                        writer.WriteLine("\t\tpublic static Result RecoverPassword(string userId)");
                        writer.WriteLine("\t\t{");

                        writer.WriteLine("\t\t\tIdentity.SetToken(userId, string.Empty);");
                        writer.WriteLine("\t\t\treturn FacadeServiceBuilder.{0}.RecoverPassword();", type.Name.Substring(1));

                        writer.WriteLine("\t\t}");
                    }
                    else if (info.Name == "ChangePassword")
                    {
                        writer.WriteLine("\t\tpublic static Result ChangePassword(string userId, string password, string newPassword)");
                        writer.WriteLine("\t\t{");

                        writer.WriteLine("\t\t\tResult result = FacadeServiceBuilder.{0}.ChangePassword(newPassword);", type.Name.Substring(1));
                        writer.WriteLine("\t\t\tif (result.OK)");
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\tIdentity.SetToken(userId, newPassword);");
                        writer.WriteLine("\t\t\t}");

                        writer.WriteLine();
                        writer.WriteLine("\t\t\treturn result;");
                        writer.WriteLine("\t\t}");
                    }
                    else
                    {
                        writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                        writer.WriteLine("\t\t{");

                        writer.WriteLine("\t\t\treturn FacadeServiceBuilder.{0}.{1}({2});", type.Name.Substring(1),
                                         info.Name, this.GetParaNameList(info));
                        writer.WriteLine("\t\t}");
                    }
                    
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
                if (item.ParameterType.Name.EndsWith("Data"))
                {
                    builder.AppendFormat(" new {1}({0}),", item.Name, item.ParameterType.Name);
                }
                else
                {

                    builder.AppendFormat(" {0},", item.Name);
                }
            }

            return builder.ToString().TrimEnd(',');
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }
    }
}