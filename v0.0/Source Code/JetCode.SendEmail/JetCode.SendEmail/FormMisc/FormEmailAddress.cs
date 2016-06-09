using System;
using System.Windows.Forms;
using JetCode.SendEmail.Helper;

namespace JetCode.SendEmail.FormMisc
{
    public partial class FormEmailAddress : FormBase
    {
        private string _emailAddress = string.Empty;

        public FormEmailAddress()
        {
            InitializeComponent();
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this._emailAddress = this.txtEmail.Text.Trim();
            if (this._emailAddress.Length == 0 || !HelperEmail.IsValidEmail(this._emailAddress))
            {
                this.txtEmail.Select();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
