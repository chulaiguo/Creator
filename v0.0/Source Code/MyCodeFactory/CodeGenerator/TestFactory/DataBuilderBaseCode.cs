using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.TestFactory
{
    public class DataBuilderBaseCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public DataBuilderBaseCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
            this._projectName = projectName;
        }

        public string GenBuilderCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteContent(writer);
            this.WriteSaveCode(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System.Reflection;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using Cheke.TestData;");
            writer.WriteLine("using {0}.Data;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Excel", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic abstract class BuilderBase : DataBuilder");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprotected static string _Assembly = \"{0}.Data\";", this._projectName);
            writer.WriteLine();
            writer.WriteLine("\t\tpublic BuilderBase(string path) : base(_Assembly, path)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic abstract Result InsertAll();");
            writer.WriteLine("\t\tpublic abstract Result DeleteAll();");
            writer.WriteLine("\t\tpublic abstract Result InsertParent();");
            writer.WriteLine("\t\tpublic abstract Result DeleteParent();");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected Result InsertCollection(BusinessCollectionBase list)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (list == null || list.Count == 0)");
            writer.WriteLine("\t\t\t\treturn new Result(true);");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn this.Save(list);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected Result DeleteCollection(BusinessCollectionBase list)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (list == null || list.Count == 0)");
            writer.WriteLine("\t\t\t\treturn new Result(true);");
            writer.WriteLine();
            writer.WriteLine("\t\t\tPropertyInfo markAsDeleted = list.GetItemType().GetProperty(\"MarkAsDeleted\");");
            writer.WriteLine("\t\t\tif (markAsDeleted != null)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tforeach (BusinessBase item in list)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tmarkAsDeleted.SetValue(item, true, null);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tlist.Clear();");
            writer.WriteLine("\t\t\treturn this.Save(list);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteSaveCode(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate Result Save(BusinessCollectionBase list)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring serviceName = list.GetItemType().Name + \"Service\";");
            writer.WriteLine("\t\t\tswitch(serviceName)");
            writer.WriteLine("\t\t\t{");

            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("Service"))
                    continue;

                string listTypeName = item.Name.Replace("Service", "Collection");
                writer.WriteLine("\t\t\t\tcase \"{0}\":", item.Name);
                writer.WriteLine("\t\t\t\t\treturn ServiceBuilder.{0}.Save(list as {1});", item.Name, listTypeName);
            }

            writer.WriteLine("\t\t\t\tdefault:");
            writer.WriteLine("\t\t\t\t\treturn new Result(\"The data service does not exist.\");");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}