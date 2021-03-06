using System;
using System.IO;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class WorkSearchDesignCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        private string _entityName = string.Empty;

        public WorkSearchDesignCode(Type type, string projectName)
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
            writer.WriteLine("namespace {0}.WinUI.FormWorkSearch", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormWork{0}Search", this._entityName);
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
            writer.WriteLine("\t\t\tthis.btnSearch = new DevExpress.XtraEditors.SimpleButton();");

            //Begin Init
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlSearchCriteria)).BeginInit();");
            writer.WriteLine("\t\t\tthis.pnlSearchCriteria.SuspendLayout();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlContent)).BeginInit();");
            writer.WriteLine("\t\t\tthis.pnlContent.SuspendLayout();");
            writer.WriteLine("\t\t\tthis.SuspendLayout();");

            //pnlSearchCriteria
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// pnlSearchCriteria");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.pnlSearchCriteria.Controls.Add(this.btnSearch);");

            //btnSearch
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// btnSearch");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.btnSearch.Location = new System.Drawing.Point(300, 27);");
            writer.WriteLine("\t\t\tthis.btnSearch.Name = \"btnSearch\";");
            writer.WriteLine("\t\t\tthis.btnSearch.Size = new System.Drawing.Size(75, 23);");
            writer.WriteLine("\t\t\tthis.btnSearch.TabIndex = 0;");
            writer.WriteLine("\t\t\tthis.btnSearch.Text = \"Search\";");
            writer.WriteLine("\t\t\tthis.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);");

            //Form
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// FormWork{0}Search", this._entityName);
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);");
            writer.WriteLine("\t\t\tthis.Name = \"FormWork{0}Search\";", this._entityName);
            writer.WriteLine("\t\t\tthis.Text = \"{0} Search\";", this._entityName);
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlSearchCriteria)).EndInit();");
            writer.WriteLine("\t\t\tthis.pnlSearchCriteria.ResumeLayout(false);");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlContent)).EndInit();");
            writer.WriteLine("\t\t\tthis.pnlContent.ResumeLayout(false);");
            writer.WriteLine("\t\t\tthis.ResumeLayout(false);");
            writer.WriteLine("\t\t}");

            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteFields(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate DevExpress.XtraEditors.SimpleButton btnSearch;");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}
