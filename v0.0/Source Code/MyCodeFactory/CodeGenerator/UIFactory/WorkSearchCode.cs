using System;
using System.IO;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class WorkSearchCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        private string _entityName = string.Empty;

        public WorkSearchCode(Type type, string projectName)
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
            this.WriteOverride(writer);
            this.WriteDataBinding(writer);
            this.WriteEvents(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Windows.Forms;");
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using {0}.ViewObj;", this._projectName);
            writer.WriteLine("using {0}.WinUI.GridDecorator;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WinUI.FormWorkSearch", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormWork{0}Search : FormWorkSearchBase", this._entityName);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate {0}Collection _list = null;", this._entityName);
            writer.WriteLine("\t\tprivate Grid{0}Decorator _decorator = null;", this._entityName);
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}Search()", this._entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}Search(string userId, Panel parent)", this._entityName);
            writer.WriteLine("\t\t\t: base(userId, parent)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteOverride(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void InitializeDecorator()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._decorator = new Grid{0}Decorator(base.UserId, base.GridControl);", this._entityName);
            writer.WriteLine("\t\t\tthis._decorator.Initialize();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override void InitializeForm()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.InitializeForm();");
            writer.WriteLine("\t\t\tthis.Caption = \"{0} Search\";", this._entityName);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void DataBinding()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t//this._list = {0}.Search(...);", this._entityName);
            writer.WriteLine("\t\t\tthis._list = {0}.GetAll();//for test", this._entityName);
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis._decorator.DataSource = this._list;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteEvents(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate void btnSearch_Click(object sender, EventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.Search();");
            writer.WriteLine("\t\t}");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}
