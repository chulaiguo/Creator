using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class ColumnsBuilderCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public ColumnsBuilderCode(Assembly assembly, string projectName)
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
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using DevExpress.XtraGrid.Columns;");
            writer.WriteLine("using DevExpress.XtraGrid.Views.Grid;");
            writer.WriteLine("using {0}.Schema;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WinUI.Utilities", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tinternal static class ColumnsBuilder");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();

            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("Data"))
                    continue;

                string objName = item.Name.Substring(0, item.Name.Length - 4);

                writer.WriteLine("\t\tinternal static void Setup{0}Columns(GridView view)", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tview.Columns.Clear();");

                PropertyInfo[] properties = item.GetProperties(BindingFlags.Public |
                                                               BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (PropertyInfo info in properties)
                {
                    if (info.Name == "IsDirty" || info.Name == "IsValid"
                        || info.Name == "LastModifiedBy" || info.Name == "LastModifiedAt"
                        || info.Name == "MarkAsDeleted" || info.Name == "Download")
                        continue;

                    if(!info.PropertyType.IsValueType && info.PropertyType != typeof(string))
                        continue;

                    writer.WriteLine();
                    writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", info.Name);
                    writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", info.Name);
                    writer.WriteLine("\t\t\tcol{0}.FieldName = {1}Schema.{0};", info.Name, objName);
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = view.Columns.Count;", info.Name);
                    writer.WriteLine("\t\t\tview.Columns.Add(col{0});", info.Name);
                }

                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}