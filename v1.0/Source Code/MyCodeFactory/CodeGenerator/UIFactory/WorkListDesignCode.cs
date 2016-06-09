using System;
using System.IO;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class WorkListDesignCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        private string _entityName = string.Empty;

        public WorkListDesignCode(Type type, string projectName)
        {
            this._type = type;
            this._projectName = projectName;

            this._entityName = this._type.Name.Substring(0, this._type.Name.Length - 4);
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
            writer.WriteLine("namespace {0}.WinUI.FormWorkList", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormWork{0}List", this._entityName);
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
            writer.WriteLine("\t\t\tthis.gridControl1 = new DevExpress.XtraGrid.GridControl();");
            writer.WriteLine("\t\t\tthis.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlContent)).BeginInit();");
            writer.WriteLine("\t\t\tthis.pnlContent.SuspendLayout();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();");
            writer.WriteLine("\t\t\tthis.SuspendLayout();");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// pnlContent");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.pnlContent.Controls.Add(this.gridControl1);");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// gridControl1");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;");
            writer.WriteLine("\t\t\tthis.gridControl1.EmbeddedNavigator.Name = \"\";");
            writer.WriteLine("\t\t\tthis.gridControl1.Location = new System.Drawing.Point(2, 2);");
            writer.WriteLine("\t\t\tthis.gridControl1.MainView = this.gridView1;");
            writer.WriteLine("\t\t\tthis.gridControl1.Name = \"gridControl1\";");
            writer.WriteLine("\t\t\tthis.gridControl1.TabIndex = 0;");
            writer.WriteLine("\t\t\tthis.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {");
            writer.WriteLine("\t\t\tthis.gridView1});");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// gridView1");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.gridView1.GridControl = this.gridControl1;");
            writer.WriteLine("\t\t\tthis.gridView1.Name = \"gridView1\";");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// FormWork{0}List", this._entityName);
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);");
            writer.WriteLine("\t\t\tthis.Name = \"FormWork{0}List\";", this._entityName);
            writer.WriteLine("\t\t\tthis.Text = \"{0} List\";", this._entityName);
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlContent)).EndInit();");
            writer.WriteLine("\t\t\tthis.pnlContent.ResumeLayout(false);");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();");
            writer.WriteLine("\t\t\tthis.ResumeLayout(false);");
            writer.WriteLine();
            writer.WriteLine("\t\t}");

            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteFields(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate DevExpress.XtraGrid.GridControl gridControl1;");
            writer.WriteLine("\t\tprivate DevExpress.XtraGrid.Views.Grid.GridView gridView1;");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}
