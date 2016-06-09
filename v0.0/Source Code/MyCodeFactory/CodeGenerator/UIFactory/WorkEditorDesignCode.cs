using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class WorkEditorDesignCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        private string _entityName = string.Empty;
        private List<PropertyInfo> _propertyList = null;

        public WorkEditorDesignCode(Type type, string projectName)
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
            this.WriteInitializeComponent(writer);
            this.WriteFields(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WinUI.FormWorkEditor", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormWork{0}", this._entityName);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Required designer variable.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\tprivate System.ComponentModel.IContainer components = null;");
            writer.WriteLine();

            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Clean up any resources being used.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\t/// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>");
            writer.WriteLine("\t\tprotected override void Dispose(bool disposing)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (disposing && (components != null))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tcomponents.Dispose();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbase.Dispose(disposing);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInitializeComponent(StringWriter writer)
        {
            writer.WriteLine("\t\t#region Windows Form Designer generated code");
            writer.WriteLine();
            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Required method for Designer support - do not modify");
            writer.WriteLine("\t\t/// the contents of this method with the code editor.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\tprivate void InitializeComponent()");
            writer.WriteLine("\t\t{");

            //Write Fields Initialize
            writer.WriteLine("\t\t\tthis.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();");
            writer.WriteLine("\t\t\tthis.tab{0} = new DevExpress.XtraTab.XtraTabPage();", this._entityName);
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\t\tthis.chk{0} = new Cheke.WinCtrl.Common.CheckEditEx();", item.Name);
                }
                else if (item.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\tthis.date{0} = new Cheke.WinCtrl.Common.DateEditEx();", item.Name);
                }
                else if (item.PropertyType == typeof(Guid))
                {
                    writer.WriteLine("\t\t\tthis.cmb{0} = new Cheke.WinCtrl.Common.LookUpEditEx();", item.Name.Substring(0, item.Name.Length - 2));
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.txt{0} = new Cheke.WinCtrl.Common.TextEditEx();", item.Name);
                }
            }

            //Begin Init
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlContent)).BeginInit();");
            writer.WriteLine("\t\t\tthis.pnlContent.SuspendLayout();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.SuspendLayout();");
            writer.WriteLine("\t\t\tthis.tab{0}.SuspendLayout();", this._entityName);
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.chk{0}.Properties)).BeginInit();", item.Name);
                }
                else if (item.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.date{0}.Properties.VistaTimeProperties)).BeginInit();", item.Name);
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.date{0}.Properties)).BeginInit();", item.Name);
                }
                else if (item.PropertyType == typeof(Guid))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.cmb{0}.Properties)).BeginInit();", item.Name.Substring(0, item.Name.Length - 2));
                }
                else
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.txt{0}.Properties)).BeginInit();", item.Name);
                }
            }
            writer.WriteLine("\t\t\tthis.SuspendLayout();");

            //pnlContent
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// pnlContent");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.pnlContent.Controls.Add(this.xtraTabControl1);");

            //xtraTabControl1
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// xtraTabControl1");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.Location = new System.Drawing.Point(2, 2);");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.Name = \"xtraTabControl1\";");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.SelectedTabPage = this.tab{0};", this._entityName);
            writer.WriteLine("\t\t\tthis.xtraTabControl1.TabIndex = 0;");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[]");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.tab{0}", this._entityName);
            writer.WriteLine("\t\t\t});");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.Text = \"xtraTabControl1\";");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);");

            //tabEntity
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// tab{0}", this._entityName);
            writer.WriteLine("\t\t\t//");
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\t\tthis.tab{0}.Controls.Add(this.chk{1});", this._entityName, item.Name);
                }
                else if (item.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\tthis.tab{0}.Controls.Add(this.date{1});", this._entityName, item.Name);
                }
                else if (item.PropertyType == typeof(Guid))
                {
                    writer.WriteLine("\t\t\tthis.tab{0}.Controls.Add(this.cmb{1});", this._entityName, item.Name.Substring(0, item.Name.Length - 2));
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.tab{0}.Controls.Add(this.txt{1});", this._entityName, item.Name);
                }
            }
            writer.WriteLine("\t\t\tthis.tab{0}.Name = \"tab{0}\";", this._entityName);
            writer.WriteLine("\t\t\tthis.tab{0}.Text = \"{0}\";", this._entityName);
            writer.WriteLine("\t\t\tthis.tab{0}.Enter += new System.EventHandler(this.tab{0}_Enter);", this._entityName);

            //Item Detail
            int tabIndex = -1;
            int xPos = 5;
            int yPos = -35;
            foreach (PropertyInfo item in this._propertyList)
            {
                tabIndex++;
                yPos += 40;
                if(yPos >= 400)
                {
                    yPos = 0;
                    xPos += 155;
                }

                if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\t// chk{0}", item.Name);
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\tthis.chk{0}.Location = new System.Drawing.Point({1}, {2});", item.Name, xPos, yPos + 16);
                    writer.WriteLine("\t\t\tthis.chk{0}.Name = \"chk{0}\";", item.Name);
                    writer.WriteLine("\t\t\tthis.chk{0}.TabIndex = {1};", item.Name, tabIndex);
                    writer.WriteLine("\t\t\tthis.chk{0}.Properties.Caption = \"{0}\";", item.Name);
                }
                else if (item.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\t// date{0}", item.Name);
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\tthis.date{0}.EditValue =  new System.DateTime(2007, 12, 3, 0, 0, 0, 0);", item.Name);
                    writer.WriteLine("\t\t\tthis.date{0}.Location = new System.Drawing.Point({1}, {2});", item.Name, xPos, yPos);
                    writer.WriteLine("\t\t\tthis.date{0}.Name = \"date{0}\";", item.Name);
                   
                    writer.WriteLine("\t\t\tthis.date{0}.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[]", item.Name);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\tnew DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)");
                    writer.WriteLine("\t\t\t\t});");

                    writer.WriteLine("\t\t\tthis.date{0}.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[]", item.Name);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\tnew DevExpress.XtraEditors.Controls.EditorButton()");
                    writer.WriteLine("\t\t\t\t});");

                    writer.WriteLine("\t\t\tthis.date{0}.TabIndex = {1};", item.Name, tabIndex);
                    writer.WriteLine("\t\t\tthis.date{0}.Title = \"{0}\";", item.Name);
                }
                else if (item.PropertyType == typeof(Guid))
                {
                    string itemName = item.Name.Substring(0, item.Name.Length - 2);

                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\t// cmb{0}", itemName);
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\tthis.cmb{0}.Properties.NullText = \"N/A\";", itemName);
                    writer.WriteLine("\t\t\tthis.cmb{0}.Location = new System.Drawing.Point({1}, {2});", itemName, xPos, yPos);
                    writer.WriteLine("\t\t\tthis.cmb{0}.Name = \"cmb{0}\";", itemName);

                    writer.WriteLine("\t\t\tthis.cmb{0}.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] ", itemName);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\tnew DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)");
                    writer.WriteLine("\t\t\t\t});");

                    writer.WriteLine("\t\t\tthis.cmb{0}.TabIndex = {1};", itemName, tabIndex);
                    writer.WriteLine("\t\t\tthis.cmb{0}.Title = \"{0}\";", itemName);
                }
                else
                {
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\t// txt{0}", item.Name);
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\tthis.txt{0}.EditValue = \"\";", item.Name);
                    writer.WriteLine("\t\t\tthis.txt{0}.Location = new System.Drawing.Point({1}, {2});", item.Name, xPos, yPos);
                    writer.WriteLine("\t\t\tthis.txt{0}.Name = \"txt{0}\";", item.Name);
                    writer.WriteLine("\t\t\tthis.txt{0}.TabIndex = {1};", item.Name, tabIndex);
                    writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}\";", item.Name);
                }
            }

            //Form
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// FormWork{0}", this._entityName);
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);");
            writer.WriteLine("\t\t\tthis.Name = \"FormWork{0}\";", this._entityName);
            writer.WriteLine("\t\t\tthis.Text = \"{0}\";", this._entityName);
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlContent)).EndInit();");
            writer.WriteLine("\t\t\tthis.pnlContent.ResumeLayout(false);");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.ResumeLayout(false);");
            writer.WriteLine("\t\t\tthis.tab{0}.ResumeLayout(false);", this._entityName);
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.chk{0}.Properties)).EndInit();", item.Name);
                }
                else if (item.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.date{0}.Properties.VistaTimeProperties)).EndInit();", item.Name);
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.date{0}.Properties)).EndInit();", item.Name);
                }
                else if (item.PropertyType == typeof(Guid))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.cmb{0}.Properties)).EndInit();", item.Name.Substring(0, item.Name.Length - 2));
                }
                else
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.txt{0}.Properties)).EndInit();", item.Name);
                }
            }
            writer.WriteLine("\t\t\tthis.ResumeLayout(false);");
            writer.WriteLine("\t\t}");

            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteFields(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate DevExpress.XtraTab.XtraTabControl xtraTabControl1;");
            writer.WriteLine("\t\tprivate DevExpress.XtraTab.XtraTabPage tab{0};", this._entityName);
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.CheckEditEx chk{0};", item.Name);
                }
                else if (item.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.DateEditEx date{0};", item.Name);
                }
                else if (item.PropertyType == typeof(Guid))
                {
                    writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.LookUpEditEx cmb{0};", item.Name.Substring(0, item.Name.Length - 2));
                }
                else
                {
                    writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.TextEditEx txt{0};", item.Name);
                }
            }
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
