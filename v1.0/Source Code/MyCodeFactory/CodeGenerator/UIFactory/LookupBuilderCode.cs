using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class LookupBuilderCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public LookupBuilderCode(Assembly assembly, string projectName)
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
            this.WriteContent(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using DevExpress.XtraEditors.Controls;");
            writer.WriteLine("using DevExpress.XtraEditors.Repository;");
            writer.WriteLine("using {0}.Schema;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WinUI.Utils", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tinternal static class LookUpEditBuilder");
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

                writer.WriteLine("\t\tinternal static void Setup{0}(RepositoryItemLookUpEdit lookUpEdit, object dataSource, string valueMember)", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (lookUpEdit.Columns.Count == 0)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tLookUpColumnInfo col = new LookUpColumnInfo();");
                writer.WriteLine("\t\t\t\tcol.FieldName = \"XXX\";//{0}Schema.XXX;", objName);
                writer.WriteLine("\t\t\t\tcol.Caption = \"{0}\";", objName);
                writer.WriteLine("\t\t\t\tlookUpEdit.Columns.Add(col);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\t\tlookUpEdit.DropDownRows = 10;");
                writer.WriteLine("\t\t\tlookUpEdit.PopupWidth = 100;");
                writer.WriteLine();

                writer.WriteLine("\t\t\tlookUpEdit.ValueMember = valueMember;");
                writer.WriteLine("\t\t\tlookUpEdit.DisplayMember = \"XXX\";//{0}Schema.XXX;", objName);
                writer.WriteLine();

                writer.WriteLine("\t\t\tlookUpEdit.DataSource = dataSource;");
                writer.WriteLine("\t\t\tlookUpEdit.BestFit();");
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
