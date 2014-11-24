using System;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class FormControlName : Form
    {
        private string _selectedType = string.Empty;

        public FormControlName()
        {
            InitializeComponent();
        }

        public string SelectedType
        {
            get { return _selectedType; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this._selectedType = this.textBox1.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.btnOK.Enabled = this.textBox1.Text.Trim().Length > 0;
        }
    }
}