using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class MFServiceWrapperCode
    {
        private Assembly _assembly = null;

        public MFServiceWrapperCode(Assembly assembly)
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
            writer.WriteLine("using Microsoft.SPOT;");
            writer.WriteLine("using chekeit.com.MicroData;");
            writer.WriteLine("using Dpws.Client;");
            writer.WriteLine("using Dpws.Client.Discovery;");
            writer.WriteLine("using Ws.Services.Faults;");
            writer.WriteLine("using Ws.Services.Binding;");
            writer.WriteLine("using Ws.Services;");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            string[] splits = this._assembly.GetName().Name.Split('.');
            if (splits.Length < 2)
                return;

            writer.WriteLine("namespace {0}.MicroObj", splits[0]);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic static class MicorServiceWrapper");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate static string _EndPointAddress = \"http://192.168.1.50:36882/MicroService\";");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate static IServiceFactoryClientProxy GetProxy()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tUri remoteEp = new Uri(_EndPointAddress);");
            writer.WriteLine("\t\t\tWS2007HttpBinding binding = new WS2007HttpBinding(new HttpTransportBindingConfig(remoteEp));");
            writer.WriteLine("\t\t\tProtocolVersion ver = new ProtocolVersion11();");
            writer.WriteLine("\t\t\tIServiceFactoryClientProxy proxy = new IServiceFactoryClientProxy(binding, ver);");
            writer.WriteLine("\t\t\tproxy.IgnoreRequestFromThisIP = true;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn proxy;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate static void Probe(IServiceFactoryClientProxy proxy)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tDpwsServiceTypes typeProbes = new DpwsServiceTypes();");
            writer.WriteLine("\t\t\ttypeProbes.Add(new DpwsServiceType(\"E3000.IMicroService.IServiceFactory\", \"http://chekeit.com/MicroData/\"));");
            writer.WriteLine("");
            writer.WriteLine("\t\t\tDpwsServiceDescriptions descs = proxy.DiscoveryClient.Probe(typeProbes, 1, 5000);");
            writer.WriteLine("\t\t\tif (descs.Count > 0)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t_EndPointAddress = descs[0].XAddrs[0];");
            writer.WriteLine("\t\t\t\tproxy.EndpointAddress = _EndPointAddress;");
            writer.WriteLine("\t\t\t}");
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
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo info in methods)
            {               
                if(info.ReturnType.IsValueType)
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tIServiceFactoryClientProxy proxy = GetProxy();");
                    if (info.ReturnType == typeof(bool))
                    {
                        writer.WriteLine("\t\t\tbool retVal = false;");
                    }
                    else if (info.ReturnType == typeof(DateTime))
                    {
                        writer.WriteLine("\t\t\tDateTime retVal = DateTime.Now;");
                    }
                    else if (info.ReturnType == typeof(int))
                    {
                        writer.WriteLine("\t\t\tint retVal = 0;");
                    }
                    else
                    {
                        writer.WriteLine("\t\t\t{0} retVal = 0;", info.ReturnType);
                    }
                }
                else if (info.ReturnType == typeof(string))
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2})", info.ReturnType, info.Name, this.GetParas(info));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tIServiceFactoryClientProxy proxy = GetProxy();");
                    writer.WriteLine("\t\t\tstring retVal = string.Empty;");
                }
                else
                {
                    if (info.ReturnType.IsArray)
                    {
                        string returnType = info.ReturnType.Name;
                        string mfData = returnType.Substring(0, returnType.Length - 6);

                        writer.WriteLine("\t\tpublic static {0}[] {1}({2})", mfData, info.Name, this.GetParas(info));
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tIServiceFactoryClientProxy proxy = GetProxy();");
                        writer.WriteLine("\t\t\t{0}[] retVal = null;", mfData);
                    }
                    else
                    {
                        string returnType = info.ReturnType.Name;
                        string mfData = returnType.Substring(0, returnType.Length - 4);

                        writer.WriteLine("\t\tpublic static {0} {1}({2})", mfData, info.Name, this.GetParas(info));
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tIServiceFactoryClientProxy proxy = GetProxy();");
                        if (mfData == "DateTime")
                        {
                            writer.WriteLine("\t\t\t{0} retVal = {0}.Now;", mfData);
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t{0} retVal = null;", mfData);
                        }
                    }
                }
                writer.WriteLine("\t\t\ttry");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t{0} request = new {0}();", info.Name);
                ParameterInfo[] paras = info.GetParameters();
                foreach (ParameterInfo para in paras)
                {
                    if (para.ParameterType.IsValueType || para.ParameterType == typeof(string))
                    {
                        writer.WriteLine("\t\t\t\trequest.{0} = {0};", para.Name);
                    }
                    else
                    {
                        if(para.ParameterType.Name == "DateTimeData")
                        {
                            writer.WriteLine("\t\t\t\trequest.{0} = new schemas.datacontract.org.E.MicroData.DateTimeData();", para.Name);
                            writer.WriteLine("\t\t\t\trequest.{0}.Year = (short){0}.Year;", para.Name);
                            writer.WriteLine("\t\t\t\trequest.{0}.Month = (byte){0}.Month;", para.Name);
                            writer.WriteLine("\t\t\t\trequest.{0}.Day = (byte){0}.Day;", para.Name);
                            writer.WriteLine("\t\t\t\trequest.{0}.Hour = (byte){0}.Hour;", para.Name);
                            writer.WriteLine("\t\t\t\trequest.{0}.Minute = (byte){0}.Minute;", para.Name);
                            writer.WriteLine("\t\t\t\trequest.{0}.Second = (byte){0}.Second;", para.Name);
                        }
                        else
                        {
                            Assembly paraAss = Assembly.LoadFrom(para.ParameterType.Assembly.Location);
                            PropertyInfo[] paraDataInfo = paraAss.GetType(para.ParameterType.ToString()).GetProperties();
                            foreach (PropertyInfo paraInfo in paraDataInfo)
                            {
                                if (!paraInfo.CanRead || !paraInfo.CanWrite)
                                    continue;

                                writer.WriteLine("\t\t\t\trequest.{0}.{1} = {0}.{1};", para.Name, paraInfo.Name);
                            }
                        }
                    }
                }
                writer.WriteLine();
                writer.WriteLine("\t\t\t\t{0}Response response = proxy.{0}(request);", info.Name);

                if (info.ReturnType.IsValueType || info.ReturnType == typeof(string))
                {
                     writer.WriteLine("\t\t\t\tretVal = response.{0}Result;", info.Name);
                }
                else
                {
                    if (info.ReturnType.IsArray)
                    {
                        string returnType = info.ReturnType.Name;
                        string mfData = returnType.Substring(0, returnType.Length - 6);
                        writer.WriteLine("\t\t\t\tint count = response.{0}Result.{1}Data.Length;", info.Name, mfData);
                        writer.WriteLine("\t\t\t\tretVal = new {0}[count];", mfData);
                        writer.WriteLine("\t\t\t\tfor (int i = 0; i < count; i++)");
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\t{0}Data item = response.{1}Result.{0}Data[i];", mfData, info.Name);
                        writer.WriteLine("\t\t\t\t\tretVal[i] = new {0}();", mfData);

                        Assembly dataAss = Assembly.LoadFrom(info.ReturnType.Assembly.Location);
                        string svrDataType = info.ReturnType.ToString().Substring(0, info.ReturnType.ToString().Length - 2);
                        PropertyInfo[] paraDataInfo = dataAss.GetType(svrDataType).GetProperties();
                        foreach (PropertyInfo paraInfo in paraDataInfo)
                        {
                            if (!paraInfo.CanRead || !paraInfo.CanWrite)
                                continue;

                            if(paraInfo.Name == "Holidays")//special holidays
                            {
                                writer.WriteLine("\t\t\t\t\tif(item.Holidays != null)");
                                writer.WriteLine("\t\t\t\t\t{");
                                writer.WriteLine("\t\t\t\t\t\tint holidayCount = item.Holidays.HolidayData.Length;");
                                writer.WriteLine("\t\t\t\t\t\tretVal[i].Holidays = new int[holidayCount];");
                                writer.WriteLine("\t\t\t\t\t\tfor (int j = 0; j < count; j++)");
                                writer.WriteLine("\t\t\t\t\t\t{");
                                writer.WriteLine("\t\t\t\t\t\t\tretVal[i].Holidays[j] = item.Holidays.HolidayData[j].Date;");
                                writer.WriteLine("\t\t\t\t\t\t}");
                                writer.WriteLine("\t\t\t\t\t}");
                            }
                            else
                            {
                                if (paraInfo.PropertyType.Name == "DateTimeData")
                                {
                                    writer.WriteLine("\t\t\t\t\tretVal[i].{0} = new DateTime(item.{0}.Year,item.{0}.Month,item.{0}.Day,item.{0}.Hour,item.{0}.Minute,item.{0}.Second);", paraInfo.Name);
                                }
                                else
                                {
                                    writer.WriteLine("\t\t\t\t\tretVal[i].{0} = item.{0};", paraInfo.Name);
                                }
                            }
                        }
                        writer.WriteLine("\t\t\t\t}");
                    }
                    else
                    {
                        string returnType = info.ReturnType.Name;
                        string mfData = returnType.Substring(0, returnType.Length - 4);
                        if (mfData == "DateTime")
                        {
                            writer.WriteLine("\t\t\t\tretVal = new DateTime(response.{0}Result.Year,response.{0}Result.Month,response.{0}Result.Day,response.{0}Result.Hour,response.{0}Result.Minute,response.{0}Result.Second);", info.Name);
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\t{0}Data item = response.{1}Result;", mfData, info.Name);
                            writer.WriteLine("\t\t\t\tif(item != null)");
                            writer.WriteLine("\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\tretVal = new {0}();", mfData);

                            Assembly dataAss = Assembly.LoadFrom(info.ReturnType.Assembly.Location);
                            PropertyInfo[] paraDataInfo = dataAss.GetType(info.ReturnType.ToString()).GetProperties();
                            foreach (PropertyInfo paraInfo in paraDataInfo)
                            {
                                if (!paraInfo.CanRead || !paraInfo.CanWrite)
                                    continue;

                                if (paraInfo.PropertyType.Name == "DateTimeData")
                                {
                                    writer.WriteLine(
                                        "\t\t\t\t\tretVal.{0} = new DateTime(item.{0}.Year,item.{0}.Month,item.{0}.Day,item.{0}.Hour,item.{0}.Minute,item.{0}.Second);",
                                        paraInfo.Name);
                                }
                                else
                                {
                                    writer.WriteLine("\t\t\t\t\tretVal.{0} = item.{0};", paraInfo.Name);
                                }
                            }
                            writer.WriteLine("\t\t\t\t}");
                        }
                    }
                }
               
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tcatch (WsFaultException)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tProbe(proxy);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tcatch (Exception ex)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tDebug.Print(ex.Message);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tfinally");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tproxy.Dispose();");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn retVal;");
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
                if (item.ParameterType.IsValueType || item.ParameterType == typeof(string))
                {
                    builder.AppendFormat("{0} {1},", item.ParameterType.FullName, item.Name);
                }
                else
                {
                    string returnType = item.ParameterType.Name;
                    string mfData = returnType.Substring(0, returnType.Length - 4);
                    builder.AppendFormat("{0} {1},", mfData, item.Name);
                }
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}