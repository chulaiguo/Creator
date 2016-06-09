namespace JetCode.SendEmail.FormMisc
{
    partial class FormEmailSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSmtpServer = new System.Windows.Forms.Label();
            this.lblSmtpPort = new System.Windows.Forms.Label();
            this.txtSmtpPort = new System.Windows.Forms.TextBox();
            this.lblSmtpUserName = new System.Windows.Forms.Label();
            this.txtSmtpUserName = new System.Windows.Forms.TextBox();
            this.lblSmtpPassword = new System.Windows.Forms.Label();
            this.txtSmtpPassword = new System.Windows.Forms.TextBox();
            this.lblFromDisplayName = new System.Windows.Forms.Label();
            this.txtFromDisplayName = new System.Windows.Forms.TextBox();
            this.lblFromAddress = new System.Windows.Forms.Label();
            this.txtFromAddress = new System.Windows.Forms.TextBox();
            this.cmbSmtpServer = new System.Windows.Forms.ComboBox();
            this.lblReplyToDisplayName = new System.Windows.Forms.Label();
            this.txtReplyToDisplayName = new System.Windows.Forms.TextBox();
            this.lblReplyToAddress = new System.Windows.Forms.Label();
            this.txtReplyToAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(147, 409);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(248, 409);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSmtpServer
            // 
            this.lblSmtpServer.AutoSize = true;
            this.lblSmtpServer.Location = new System.Drawing.Point(23, 22);
            this.lblSmtpServer.Name = "lblSmtpServer";
            this.lblSmtpServer.Size = new System.Drawing.Size(71, 12);
            this.lblSmtpServer.TabIndex = 3;
            this.lblSmtpServer.Text = "Smtp Server";
            // 
            // lblSmtpPort
            // 
            this.lblSmtpPort.AutoSize = true;
            this.lblSmtpPort.Location = new System.Drawing.Point(261, 22);
            this.lblSmtpPort.Name = "lblSmtpPort";
            this.lblSmtpPort.Size = new System.Drawing.Size(59, 12);
            this.lblSmtpPort.TabIndex = 5;
            this.lblSmtpPort.Text = "Smtp Port";
            // 
            // txtSmtpPort
            // 
            this.txtSmtpPort.Location = new System.Drawing.Point(261, 37);
            this.txtSmtpPort.Name = "txtSmtpPort";
            this.txtSmtpPort.Size = new System.Drawing.Size(93, 21);
            this.txtSmtpPort.TabIndex = 4;
            this.txtSmtpPort.Text = "25";
            // 
            // lblSmtpUserName
            // 
            this.lblSmtpUserName.AutoSize = true;
            this.lblSmtpUserName.Location = new System.Drawing.Point(21, 79);
            this.lblSmtpUserName.Name = "lblSmtpUserName";
            this.lblSmtpUserName.Size = new System.Drawing.Size(89, 12);
            this.lblSmtpUserName.TabIndex = 7;
            this.lblSmtpUserName.Text = "Smtp User Name";
            // 
            // txtSmtpUserName
            // 
            this.txtSmtpUserName.Location = new System.Drawing.Point(23, 94);
            this.txtSmtpUserName.Name = "txtSmtpUserName";
            this.txtSmtpUserName.Size = new System.Drawing.Size(328, 21);
            this.txtSmtpUserName.TabIndex = 6;
            this.txtSmtpUserName.TextChanged += new System.EventHandler(this.txtSmtpUserName_TextChanged);
            // 
            // lblSmtpPassword
            // 
            this.lblSmtpPassword.AutoSize = true;
            this.lblSmtpPassword.Location = new System.Drawing.Point(21, 133);
            this.lblSmtpPassword.Name = "lblSmtpPassword";
            this.lblSmtpPassword.Size = new System.Drawing.Size(83, 12);
            this.lblSmtpPassword.TabIndex = 9;
            this.lblSmtpPassword.Text = "Smtp Password";
            // 
            // txtSmtpPassword
            // 
            this.txtSmtpPassword.Location = new System.Drawing.Point(23, 148);
            this.txtSmtpPassword.Name = "txtSmtpPassword";
            this.txtSmtpPassword.Size = new System.Drawing.Size(328, 21);
            this.txtSmtpPassword.TabIndex = 8;
            // 
            // lblFromDisplayName
            // 
            this.lblFromDisplayName.AutoSize = true;
            this.lblFromDisplayName.Location = new System.Drawing.Point(21, 191);
            this.lblFromDisplayName.Name = "lblFromDisplayName";
            this.lblFromDisplayName.Size = new System.Drawing.Size(107, 12);
            this.lblFromDisplayName.TabIndex = 11;
            this.lblFromDisplayName.Text = "From Display Name";
            // 
            // txtFromDisplayName
            // 
            this.txtFromDisplayName.Location = new System.Drawing.Point(23, 206);
            this.txtFromDisplayName.Name = "txtFromDisplayName";
            this.txtFromDisplayName.Size = new System.Drawing.Size(328, 21);
            this.txtFromDisplayName.TabIndex = 10;
            // 
            // lblFromAddress
            // 
            this.lblFromAddress.AutoSize = true;
            this.lblFromAddress.Location = new System.Drawing.Point(23, 241);
            this.lblFromAddress.Name = "lblFromAddress";
            this.lblFromAddress.Size = new System.Drawing.Size(77, 12);
            this.lblFromAddress.TabIndex = 13;
            this.lblFromAddress.Text = "From Address";
            // 
            // txtFromAddress
            // 
            this.txtFromAddress.Location = new System.Drawing.Point(23, 256);
            this.txtFromAddress.Name = "txtFromAddress";
            this.txtFromAddress.Size = new System.Drawing.Size(328, 21);
            this.txtFromAddress.TabIndex = 12;
            // 
            // cmbSmtpServer
            // 
            this.cmbSmtpServer.FormattingEnabled = true;
            this.cmbSmtpServer.Location = new System.Drawing.Point(21, 38);
            this.cmbSmtpServer.Name = "cmbSmtpServer";
            this.cmbSmtpServer.Size = new System.Drawing.Size(229, 20);
            this.cmbSmtpServer.TabIndex = 14;
            this.cmbSmtpServer.SelectedIndexChanged += new System.EventHandler(this.cmbDefault_SelectedIndexChanged);
            // 
            // lblReplyToDisplayName
            // 
            this.lblReplyToDisplayName.AutoSize = true;
            this.lblReplyToDisplayName.Location = new System.Drawing.Point(21, 294);
            this.lblReplyToDisplayName.Name = "lblReplyToDisplayName";
            this.lblReplyToDisplayName.Size = new System.Drawing.Size(125, 12);
            this.lblReplyToDisplayName.TabIndex = 19;
            this.lblReplyToDisplayName.Text = "ReplyTo Display Name";
            // 
            // txtReplyToDisplayName
            // 
            this.txtReplyToDisplayName.Location = new System.Drawing.Point(21, 309);
            this.txtReplyToDisplayName.Name = "txtReplyToDisplayName";
            this.txtReplyToDisplayName.Size = new System.Drawing.Size(328, 21);
            this.txtReplyToDisplayName.TabIndex = 18;
            // 
            // lblReplyToAddress
            // 
            this.lblReplyToAddress.AutoSize = true;
            this.lblReplyToAddress.Location = new System.Drawing.Point(21, 344);
            this.lblReplyToAddress.Name = "lblReplyToAddress";
            this.lblReplyToAddress.Size = new System.Drawing.Size(95, 12);
            this.lblReplyToAddress.TabIndex = 17;
            this.lblReplyToAddress.Text = "ReplyTo Address";
            // 
            // txtReplyToAddress
            // 
            this.txtReplyToAddress.Location = new System.Drawing.Point(23, 359);
            this.txtReplyToAddress.Name = "txtReplyToAddress";
            this.txtReplyToAddress.Size = new System.Drawing.Size(328, 21);
            this.txtReplyToAddress.TabIndex = 16;
            // 
            // FormEmailSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 452);
            this.ControlBox = false;
            this.Controls.Add(this.lblReplyToDisplayName);
            this.Controls.Add(this.txtReplyToDisplayName);
            this.Controls.Add(this.lblReplyToAddress);
            this.Controls.Add(this.txtReplyToAddress);
            this.Controls.Add(this.cmbSmtpServer);
            this.Controls.Add(this.lblFromAddress);
            this.Controls.Add(this.txtFromAddress);
            this.Controls.Add(this.lblFromDisplayName);
            this.Controls.Add(this.txtFromDisplayName);
            this.Controls.Add(this.lblSmtpPassword);
            this.Controls.Add(this.txtSmtpPassword);
            this.Controls.Add(this.lblSmtpUserName);
            this.Controls.Add(this.txtSmtpUserName);
            this.Controls.Add(this.lblSmtpPort);
            this.Controls.Add(this.txtSmtpPort);
            this.Controls.Add(this.lblSmtpServer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormEmailSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Email Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSmtpServer;
        private System.Windows.Forms.Label lblSmtpPort;
        private System.Windows.Forms.TextBox txtSmtpPort;
        private System.Windows.Forms.Label lblSmtpUserName;
        private System.Windows.Forms.TextBox txtSmtpUserName;
        private System.Windows.Forms.Label lblSmtpPassword;
        private System.Windows.Forms.TextBox txtSmtpPassword;
        private System.Windows.Forms.Label lblFromDisplayName;
        private System.Windows.Forms.TextBox txtFromDisplayName;
        private System.Windows.Forms.Label lblFromAddress;
        private System.Windows.Forms.TextBox txtFromAddress;
        private System.Windows.Forms.ComboBox cmbSmtpServer;
        private System.Windows.Forms.Label lblReplyToDisplayName;
        private System.Windows.Forms.TextBox txtReplyToDisplayName;
        private System.Windows.Forms.Label lblReplyToAddress;
        private System.Windows.Forms.TextBox txtReplyToAddress;
    }
}