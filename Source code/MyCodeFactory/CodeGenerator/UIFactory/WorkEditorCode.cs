using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class WorkEditorCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        private string _entityName = string.Empty;
        private List<PropertyInfo> _propertyList = null;

        public WorkEditorCode(Type type, string projectName)
        {
            this._type = type;
            this._projectName = projectName;

            this._entityName = this._type.Name.Substring(0, this._type.Name.Length - 4);
            this._propertyList = this.GetProperties();
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteOverride(writer);
            this.WriteEvents(writer);
            this.WriteDataBinding(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Windows.Forms;");
            writer.WriteLine("using DevExpress.XtraTab;");
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using {0}.ViewObj;", this._projectName);
            writer.WriteLine("using {0}.Schema;", this._projectName);
            writer.WriteLine("using {0}.WinUI.Utils;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WinUI.FormWorkEditor", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormWork{0} : FormWorkEditorBase", this._entityName);
            writer.WriteLine("\t{");

            this.WriteLookupData(writer);
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}()", this._entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}(string userId, Panel parent, {0} entity)", this._entityName);
            writer.WriteLine("\t\t\t: base(userId, parent, entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate {0} {0}", this._entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn base.Entity as {0};", this._entityName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteLookupData(StringWriter writer)
        {
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType != typeof(Guid))
                    continue;

                string lowerItemName = string.Format("{0}{1}", item.Name.Substring(0, 1).ToLower(),
                  item.Name.Substring(1, item.Name.Length - 3));

                writer.WriteLine("\t\tprivate {0}Collection _{1}List = null;", item.Name.Substring(0, item.Name.Length - 2), lowerItemName);
            }
        }

        private void WriteOverride(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void UpdateUI(bool isDirty)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.UpdateUI(isDirty);");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (this.{0}.IsNew)", this._entityName);
            writer.WriteLine("\t\t\t{");
            string special = "{0}";
            writer.WriteLine("\t\t\t\tthis.Caption = string.Format(\"New {0} Created At {1}\", DateTime.Now.ToLongTimeString());", this._entityName, special);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Caption = \"{0}\";//{0}Schema.XXX", this._entityName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void WriteEvents(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (e.Page == this.tab{0})", this._entityName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.ShowDeleteButton = true;");
            writer.WriteLine("\t\t\t\tthis.ShowNewButton = true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.ShowDeleteButton = false;");
            writer.WriteLine("\t\t\t\tthis.ShowNewButton = false;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.DataBinding();");
            writer.WriteLine("\t\t}");
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate void tab{0}_Enter(object sender, EventArgs e)", this._entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (this.tab{0}.Tag == null)", this._entityName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.tab{0}.Tag = true;", this._entityName);
            writer.WriteLine();
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType == typeof(Guid))
                    continue;

                if (item.PropertyType == typeof (bool))
                {
                    writer.WriteLine("\t\t\t\tthis.chk{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, this._entityName);
                }
                else if (item.PropertyType == typeof (DateTime))
                {
                    writer.WriteLine("\t\t\t\tthis.date{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, this._entityName);
                }
                else
                {
                    writer.WriteLine("\t\t\t\tthis.txt{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, this._entityName);
                }
            }

            writer.WriteLine();
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType != typeof (Guid))
                    continue;

                string lowerItemName =
                    string.Format("{0}{1}", item.Name.Substring(0, 1).ToLower(),
                                  item.Name.Substring(1, item.Name.Length - 3));

                writer.WriteLine("\t\t\t\t//{0}", item.Name);
                writer.WriteLine("\t\t\t\tif (this._{0}List == null || base.IsRefreshData)", lowerItemName);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tthis._{0}List = {1}.GetAll();", lowerItemName,
                                 item.Name.Substring(0, item.Name.Length - 2)); 
                writer.WriteLine("\t\t\t\t\tLookUpEditBuilder.Setup{0}(this.cmb{0}.Properties, this._{1}List, string.Empty);",
                                 item.Name.Substring(0, item.Name.Length - 2), lowerItemName);
                writer.WriteLine("\t\t\t\t}");

                writer.WriteLine("\t\t\t\tthis.cmb{0}.BindingData(this.{1}, {1}Schema.{0});",
                                 item.Name.Substring(0, item.Name.Length - 2),
                                 this._entityName);

                writer.WriteLine();
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private List<PropertyInfo> GetProperties()
        {
            List<PropertyInfo> list = new List<PropertyInfo>();

            PropertyInfo[] properties = this._type.GetProperties(BindingFlags.Public |
                                                     BindingFlags.Instance |
                                                     BindingFlags.DeclaredOnly);

            foreach (PropertyInfo item in properties)
            {
                if (item.PropertyType == typeof(byte[]))
                    continue;

                if (item.PropertyType.Name.EndsWith("Collection"))
                    continue;

                if (item.Name == string.Format("{0}PK", this._entityName))
                    continue;

                if (item.Name == "RowVersion" || item.Name == "IsDirty" || item.Name == "IsValid"
                    || item.Name == "PKString" || item.Name == "MarkAsDeleted" || item.Name == "TableName")
                    continue;

                if (item.Name == "CreatedOn" || item.Name == "CreatedBy" || item.Name == "ModifiedOn"
                    || item.Name == "ModifiedBy" || item.Name == "LastModifiedAt" || item.Name == "LastModifiedBy")
                    continue;

                list.Add(item);
            }

            return list;
        }
    }
}
