using System;
using System.Windows.Forms;

namespace JetCode.SendEmail.FormMisc
{
    public partial class FormEmailSetting : FormBase
    {
        private MailSetting _setting = null;

        public FormEmailSetting()
        {
            InitializeComponent();
        }


        public FormEmailSetting(MailSetting setting)
        {
            InitializeComponent();

            this._setting = setting;
        }

        public MailSetting Setting
        {
            get { return _setting; }
        }

        protected override void InitializeForm()
        {
            base.InitializeForm();

            this.cmbSmtpServer.Items.Add("smtp.163.com");
            this.cmbSmtpServer.Items.Add("smtp.sohu.com");
            this.cmbSmtpServer.Items.Add("smtp.sina.com.cn");
            this.cmbSmtpServer.Items.Add("smtp.gmail.com");
            this.cmbSmtpServer.Items.Add("smtp.126.com");
            this.cmbSmtpServer.Items.Add("smtp.sogou.com");
            this.cmbSmtpServer.Items.Add("smtp.qq.com");
            this.cmbSmtpServer.Items.Add("mail.gmx.com");
            this.cmbSmtpServer.Items.Add("smtp.139.com");
            this.cmbSmtpServer.Items.Add("smtp.tianya.cn");
            this.cmbSmtpServer.Items.Add("smtp.aol.com");
            this.cmbSmtpServer.Items.Add("smtp.gawab.com");
            this.cmbSmtpServer.Items.Add("smtp.foxmail.com");
            this.cmbSmtpServer.Items.Add("smtp.live.com");

            if(this._setting != null)
            {
                this.cmbSmtpServer.Enabled = false;
                this.txtSmtpUserName.Enabled = false;

                this.cmbSmtpServer.Text = this._setting.SmtpServer;
                this.txtSmtpPort.Text = string.Format("{0}", this._setting.SmtpPort);
                this.txtSmtpUserName.Text = this._setting.SmtpUserName;
                this.txtSmtpPassword.Text = this._setting.SmtpPassword;
                this.txtFromAddress.Text = this._setting.FromAddress;
                this.txtFromDisplayName.Text = this._setting.FromDisplayName;
                this.txtReplyToAddress.Text = this._setting.ReplyToAddress;
                this.txtReplyToDisplayName.Text = this._setting.ReplyToDisplayName;
            }
            else
            {
                this.cmbSmtpServer.SelectedIndex = 0;
            }
        }


        private void cmbDefault_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.cmbSmtpServer.Enabled)
                return;

            if(this.cmbSmtpServer.SelectedText == "smtp.gmail.com")
            {
                this.txtSmtpPort.Text = "587";
            }
            else
            {
                this.txtSmtpPort.Text = "25";
            }

            this.txtSmtpUserName.Text = "userName";
            this.txtSmtpPassword.Text = "password";
        }

        private void txtSmtpUserName_TextChanged(object sender, EventArgs e)
        {
            if (!this.txtSmtpUserName.Enabled)
                return;

            string userName = this.txtSmtpUserName.Text;
            this.txtFromAddress.Text = string.Format("{0}@{1}", userName, this.cmbSmtpServer.Text.Replace("smtp.", ""));
            this.txtFromDisplayName.Text = userName;

            this.txtReplyToAddress.Text = this.txtFromAddress.Text;
            this.txtReplyToDisplayName.Text = this.txtFromDisplayName.Text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this._setting == null)
            {
                this._setting = new MailSetting();
                this._setting.SmtpServer = this.cmbSmtpServer.Text;
                this._setting.SmtpUserName = this.txtSmtpUserName.Text.Trim();
            }
           
            int port;
            if(int.TryParse(this.txtSmtpPort.Text.Trim(), out port))
            {
                this._setting.SmtpPort = port;
            }
            
            this._setting.SmtpPassword = this.txtSmtpPassword.Text.Trim();

            this._setting.FromDisplayName = this.txtFromDisplayName.Text.Trim();
            this._setting.FromAddress = this.txtFromAddress.Text.Trim();

            this._setting.ReplyToDisplayName = this.txtReplyToDisplayName.Text.Trim();
            this._setting.ReplyToAddress = this.txtReplyToAddress.Text.Trim();

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
