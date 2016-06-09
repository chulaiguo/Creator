using System;
using System.Reflection;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class FormGroupDecorator : Form
    {
        private Assembly _assembly = null;

        private Type _leftType = null;
        private Type _rightType = null;
        private Type _parentType = null;
        private string _leftTypePK = string.Empty;

        public FormGroupDecorator()
        {
            InitializeComponent();
        }

        public FormGroupDecorator(Assembly assembly)
        {
            InitializeComponent();

            this._assembly = assembly;
        }

        public Type LeftType
        {
            get { return _leftType; }
        }

        public Type RightType
        {
            get { return _rightType; }
        }

        public Type ParentType
        {
            get { return _parentType; }
        }

        public string LeftTypePK
        {
            get { return _leftTypePK; }
        }

        private void FormGroupDecorator_Load(object sender, EventArgs e)
        {
            this.FillCombox();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this._leftType = this.cmbLeftType.SelectedItem as Type;
            this._rightType = this.cmbRightType.SelectedItem as Type;
            this._parentType = this.cmbParentType.SelectedItem as Type;
            this._leftTypePK = this.txtLeftTypePK.Text;
        }

        private void FillCombox()
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic || !item.Name.EndsWith("Data"))
                    continue;

                this.cmbLeftType.Items.Add(item);
                this.cmbRightType.Items.Add(item);
                this.cmbParentType.Items.Add(item);
            }

            this.cmbLeftType.SelectedIndex = 0;
            this.cmbRightType.SelectedIndex = 0;
            this.cmbParentType.SelectedIndex = 0;
        }

        private void cmbLeftType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Type leftType = this.cmbLeftType.SelectedItem as Type;
            if(leftType == null)
                return;

            this.txtLeftTypePK.Text = leftType.Name.Substring(0, leftType.Name.Length - 4) + "PK";
        }
    }
}