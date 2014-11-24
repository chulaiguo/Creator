using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class ViewMethodCode
    {
        private readonly Assembly _assembly = null;
        private readonly string _projectName = string.Empty;

        public ViewMethodCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
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
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this._projectName);
            writer.WriteLine("using {0}.BasicServiceWrapper;", this._projectName);
           
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ViewObj", this._projectName);
            writer.WriteLine("{");

            writer.WriteLine("\tinternal static class NetworkExceptionHelper");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic static bool IsNetworkException(Exception ex)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif(ex.Message.StartsWith(\"The input stream is not a valid binary format\")");
            writer.WriteLine("\t\t\t\t|| ex.Message.StartsWith(\"The underlying connection was closed\")");
            writer.WriteLine("\t\t\t\t|| ex.Message.StartsWith(\"An existing connection was forcibly closed\"))");
            writer.WriteLine("\t\t\t\treturn true;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic static ApplicationException CreateNetworkException()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn new ApplicationException(\"The network is not available. Please wait a minute and try again.\");");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine();

            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic)
                    continue;

                if (item.Name == "BasicServiceBuilder")
                    continue;

                string className = item.Name.Substring(0, item.Name.Length - "Wrapper".Length);
                writer.WriteLine("\tpublic partial class {0}", className);
                writer.WriteLine("\t{");

                bool hasSaveMethod = false;
                MethodInfo[] methods = item.GetMethods(BindingFlags.DeclaredOnly|BindingFlags.Static | BindingFlags.Public);
                foreach (MethodInfo info in methods)
                {
                    if (!info.IsPublic)
                        continue;

                    string returnType = info.ReturnType.ToString();
                    string methodName = info.Name;
                    string paras = this.GetParas(info);
                    if(methodName.StartsWith("Save"))
                    {
                        hasSaveMethod = true;
                        continue;
                    }

                    if(returnType.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\tpublic static {0}Collection {1}({2})", className, methodName, paras);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tException innerEx = null;");
                        writer.WriteLine("\t\t\tfor (int i = 0; i < 3; i++)");
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\ttry");
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\t{0}DataCollection data = {0}Wrapper.{1}({2});", className, methodName, this.GetInputPara(info));
                        writer.WriteLine("\t\t\t\t\treturn new {0}Collection(data);", className);
                        writer.WriteLine("\t\t\t\t}");
                        writer.WriteLine("\t\t\t\tcatch (Exception ex)");
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\tif(NetworkExceptionHelper.IsNetworkException(ex))");
                        writer.WriteLine("\t\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\t\tinnerEx = ex;");
                        writer.WriteLine("\t\t\t\t\t}");
                        writer.WriteLine("\t\t\t\t\telse");
                        writer.WriteLine("\t\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\t\tthrow;");
                        writer.WriteLine("\t\t\t\t\t}");
                        writer.WriteLine("\t\t\t\t}");
                        writer.WriteLine();
                        writer.WriteLine("\t\t\t\tSystem.Threading.Thread.Sleep(10000);");
                        writer.WriteLine("\t\t\t}");
                        writer.WriteLine();
                        writer.WriteLine("\t\t\tIdentity.ResetQueryParas();");
                        writer.WriteLine("\t\t\tthrow innerEx ?? NetworkExceptionHelper.CreateNetworkException();");
                        writer.WriteLine("\t\t}");
                    }
                    else if (returnType.EndsWith("Data"))
                    {
                        writer.WriteLine("\t\tpublic static {0} {1}({2})", className, methodName, paras);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tException innerEx = null;");
                        writer.WriteLine("\t\t\tfor (int i = 0; i < 3; i++)");
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\ttry");
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\t{0}Data data = {0}Wrapper.{1}({2});", className, methodName, this.GetInputPara(info));
                        writer.WriteLine("\t\t\t\t\tif(data == null)");
                        writer.WriteLine("\t\t\t\t\t\treturn null;");
                        writer.WriteLine();
                        writer.WriteLine("\t\t\t\t\treturn new {0}(data);", className);
                        writer.WriteLine("\t\t\t\t}");
                        writer.WriteLine("\t\t\t\tcatch (Exception ex)");
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\tif(NetworkExceptionHelper.IsNetworkException(ex))");
                        writer.WriteLine("\t\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\t\tinnerEx = ex;");
                        writer.WriteLine("\t\t\t\t\t}");
                        writer.WriteLine("\t\t\t\t\telse");
                        writer.WriteLine("\t\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\t\tthrow;");
                        writer.WriteLine("\t\t\t\t\t}");
                        writer.WriteLine("\t\t\t\t}");
                        writer.WriteLine();
                        writer.WriteLine("\t\t\t\tSystem.Threading.Thread.Sleep(10000);");
                        writer.WriteLine("\t\t\t}");
                        writer.WriteLine();
                        writer.WriteLine("\t\t\tIdentity.ResetQueryParas();");
                        writer.WriteLine("\t\t\tthrow innerEx ?? NetworkExceptionHelper.CreateNetworkException();");
                        writer.WriteLine("\t\t}");
                    }
                    else
                    {
                        if(info.ReturnType == typeof(void))
                        {
                            writer.WriteLine("\t\tpublic static void {0}({1})", methodName, paras);
                            writer.WriteLine("\t\t{");
                            writer.WriteLine("\t\t\tException innerEx = null;");
                            writer.WriteLine("\t\t\tfor (int i = 0; i < 3; i++)");
                            writer.WriteLine("\t\t\t{");
                            writer.WriteLine("\t\t\t\ttry");
                            writer.WriteLine("\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\t{0}Wrapper.{1}({2}));", className, methodName, this.GetInputPara(info));
                            writer.WriteLine("\t\t\t\t}");
                            writer.WriteLine("\t\t\t\tcatch (Exception ex)");
                            writer.WriteLine("\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\tif(NetworkExceptionHelper.IsNetworkException(ex))");
                            writer.WriteLine("\t\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\t\tinnerEx = ex;");
                            writer.WriteLine("\t\t\t\t\t}");
                            writer.WriteLine("\t\t\t\t\telse");
                            writer.WriteLine("\t\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\t\tthrow;");
                            writer.WriteLine("\t\t\t\t\t}");
                            writer.WriteLine("\t\t\t\t}");
                            writer.WriteLine();
                            writer.WriteLine("\t\t\t\tSystem.Threading.Thread.Sleep(10000);");
                            writer.WriteLine("\t\t\t}");
                            writer.WriteLine();
                            writer.WriteLine("\t\t\tthrow innerEx ?? NetworkExceptionHelper.CreateNetworkException();");
                            writer.WriteLine("\t\t}");
                        }
                        else
                        {
                            writer.WriteLine("\t\tpublic static {0} {1}({2})", returnType, methodName, paras);
                            writer.WriteLine("\t\t{");
                            writer.WriteLine("\t\t\tException innerEx = null;");
                            writer.WriteLine("\t\t\tfor (int i = 0; i < 3; i++)");
                            writer.WriteLine("\t\t\t{");
                            writer.WriteLine("\t\t\t\ttry");
                            writer.WriteLine("\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\treturn {0}Wrapper.{1}({2});", className, methodName, this.GetInputPara(info));
                            writer.WriteLine("\t\t\t\t}");
                            writer.WriteLine("\t\t\t\tcatch (Exception ex)");
                            writer.WriteLine("\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\tif(NetworkExceptionHelper.IsNetworkException(ex))");
                            writer.WriteLine("\t\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\t\tinnerEx = ex;");
                            writer.WriteLine("\t\t\t\t\t}");
                            writer.WriteLine("\t\t\t\t\telse");
                            writer.WriteLine("\t\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\t\tthrow;");
                            writer.WriteLine("\t\t\t\t\t}");
                            writer.WriteLine("\t\t\t\t}");
                            writer.WriteLine();
                            writer.WriteLine("\t\t\t\tSystem.Threading.Thread.Sleep(10000);");
                            writer.WriteLine("\t\t\t}");
                            writer.WriteLine();
                            writer.WriteLine("\t\t\tthrow innerEx ?? NetworkExceptionHelper.CreateNetworkException();");
                            writer.WriteLine("\t\t}");
                        }
                    }

                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
                writer.WriteLine();

                //Save Method
                if(!hasSaveMethod)
                    continue;

                writer.WriteLine("\tpublic partial class {0}: Cheke.BusinessEntity.IPersist", className);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tpublic Cheke.BusinessEntity.Result Save()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (!this.IsDirty)");
                writer.WriteLine("\t\t\t\treturn new Result(true);");
                writer.WriteLine();
                writer.WriteLine("\t\t\t{0}Data data = new {0}Data(this);", className);
                writer.WriteLine("\t\t\tdata.GetChanges();");
                writer.WriteLine();
                writer.WriteLine("\t\t\tException innerEx = null;");
                writer.WriteLine("\t\t\tfor (int i = 0; i < 3; i++)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\ttry");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn {0}Wrapper.Save(data, Cheke.Identity.Token);", className);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\tcatch (Exception ex)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tif(NetworkExceptionHelper.IsNetworkException(ex))");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tinnerEx = ex;");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tthrow;");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tSystem.Threading.Thread.Sleep(10000);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthrow innerEx ?? NetworkExceptionHelper.CreateNetworkException();");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
                writer.WriteLine();

                writer.WriteLine("\tpublic partial class {0}Collection: Cheke.BusinessEntity.IPersist", className);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tpublic Cheke.BusinessEntity.Result Save()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (!this.IsDirty)");
                writer.WriteLine("\t\t\t\treturn new Result(true);");
                writer.WriteLine();
                writer.WriteLine("\t\t\t{0}DataCollection dataList = new {0}DataCollection(this);", className);
                writer.WriteLine("\t\t\tdataList.GetChanges();");
                writer.WriteLine();
                writer.WriteLine("\t\t\tException innerEx = null;");
                writer.WriteLine("\t\t\tfor (int i = 0; i < 3; i++)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\ttry");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn {0}Wrapper.Save(dataList, Cheke.Identity.Token);", className);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\tcatch (Exception ex)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tif(NetworkExceptionHelper.IsNetworkException(ex))");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tinnerEx = ex;");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tthrow;");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tSystem.Threading.Thread.Sleep(10000);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthrow innerEx ?? NetworkExceptionHelper.CreateNetworkException();");
                writer.WriteLine("\t\t}");
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
                if (item.ParameterType.Name == "SecurityToken")
                    continue;

                builder.AppendFormat(" {0} {1},", item.ParameterType.FullName, item.Name);
            }

            return builder.ToString().TrimEnd(',').Trim();
        }

        private string GetInputPara(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                if (item.ParameterType.Name == "SecurityToken")
                    continue;

                builder.AppendFormat("{0}, ", item.Name);
            }
            builder.AppendFormat("Cheke.Identity.Token");

            return builder.ToString();
        }
    }
}