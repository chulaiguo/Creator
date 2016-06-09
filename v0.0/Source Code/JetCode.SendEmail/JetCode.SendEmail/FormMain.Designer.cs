namespace JetCode.SendEmail
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabSendTo = new System.Windows.Forms.TabPage();
            this.grpSendTo = new System.Windows.Forms.GroupBox();
            this.lstSendTo = new System.Windows.Forms.ListBox();
            this.pnlSendTo = new System.Windows.Forms.Panel();
            this.btnRemoveEmail = new System.Windows.Forms.Button();
            this.btnLoadEmail = new System.Windows.Forms.Button();
            this.btnAddEmail = new System.Windows.Forms.Button();
            this.tabContent = new System.Windows.Forms.TabPage();
            this.grpBody = new System.Windows.Forms.GroupBox();
            this.txtBody = new JetCode.SendEmail.UserCtrl.HtmlEditor();
            this.grpSubject = new System.Windows.Forms.GroupBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.tabAttachment = new System.Windows.Forms.TabPage();
            this.lstAttachment = new System.Windows.Forms.ListBox();
            this.pnlAttachment = new System.Windows.Forms.Panel();
            this.btnRemoveAttachment = new System.Windows.Forms.Button();
            this.btnAddAttachment = new System.Windows.Forms.Button();
            this.tabConfigure = new System.Windows.Forms.TabPage();
            this.lstSetting = new System.Windows.Forms.ListView();
            this.imgLarge = new System.Windows.Forms.ImageList(this.components);
            this.pnlSetting = new System.Windows.Forms.Panel();
            this.btnEditEmailSetting = new System.Windows.Forms.Button();
            this.btnRemoveEmailSetting = new System.Windows.Forms.Button();
            this.btnAddSetting = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoadMultiEmail = new System.Windows.Forms.Button();
            this.tabContainer.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tabSendTo.SuspendLayout();
            this.grpSendTo.SuspendLayout();
            this.pnlSendTo.SuspendLayout();
            this.tabContent.SuspendLayout();
            this.grpBody.SuspendLayout();
            this.grpSubject.SuspendLayout();
            this.tabAttachment.SuspendLayout();
            this.pnlAttachment.SuspendLayout();
            this.tabConfigure.SuspendLayout();
            this.pnlSetting.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tabContainer
            // 
            this.tabContainer.Controls.Add(this.tabMain);
            this.tabContainer.Controls.Add(this.tabSendTo);
            this.tabContainer.Controls.Add(this.tabContent);
            this.tabContainer.Controls.Add(this.tabAttachment);
            this.tabContainer.Controls.Add(this.tabConfigure);
            this.tabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContainer.Location = new System.Drawing.Point(0, 0);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedIndex = 0;
            this.tabContainer.Size = new System.Drawing.Size(809, 371);
            this.tabContainer.TabIndex = 9;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.grpLog);
            this.tabMain.Controls.Add(this.pnlMain);
            this.tabMain.Controls.Add(this.progressBar1);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(801, 345);
            this.tabMain.TabIndex = 2;
            this.tabMain.Text = "Main";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.txtLog);
            this.grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLog.Location = new System.Drawing.Point(3, 75);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(795, 233);
            this.grpLog.TabIndex = 6;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "Log";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 16);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(789, 214);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnCancel);
            this.pnlMain.Controls.Add(this.btnSend);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMain.Location = new System.Drawing.Point(3, 3);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(795, 72);
            this.pnlMain.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(350, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(257, 38);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(64, 15);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(257, 38);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Enabled = false;
            this.progressBar1.Location = new System.Drawing.Point(3, 308);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(795, 34);
            this.progressBar1.TabIndex = 5;
            // 
            // tabSendTo
            // 
            this.tabSendTo.Controls.Add(this.grpSendTo);
            this.tabSendTo.Controls.Add(this.pnlSendTo);
            this.tabSendTo.Location = new System.Drawing.Point(4, 22);
            this.tabSendTo.Name = "tabSendTo";
            this.tabSendTo.Padding = new System.Windows.Forms.Padding(3);
            this.tabSendTo.Size = new System.Drawing.Size(801, 345);
            this.tabSendTo.TabIndex = 5;
            this.tabSendTo.Text = "SendTo";
            this.tabSendTo.UseVisualStyleBackColor = true;
            // 
            // grpSendTo
            // 
            this.grpSendTo.Controls.Add(this.lstSendTo);
            this.grpSendTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSendTo.Location = new System.Drawing.Point(3, 75);
            this.grpSendTo.Name = "grpSendTo";
            this.grpSendTo.Size = new System.Drawing.Size(795, 267);
            this.grpSendTo.TabIndex = 1;
            this.grpSendTo.TabStop = false;
            this.grpSendTo.Text = "Send To";
            // 
            // lstSendTo
            // 
            this.lstSendTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSendTo.FormattingEnabled = true;
            this.lstSendTo.Location = new System.Drawing.Point(3, 16);
            this.lstSendTo.Name = "lstSendTo";
            this.lstSendTo.Size = new System.Drawing.Size(789, 248);
            this.lstSendTo.TabIndex = 3;
            // 
            // pnlSendTo
            // 
            this.pnlSendTo.Controls.Add(this.btnLoadMultiEmail);
            this.pnlSendTo.Controls.Add(this.btnRemoveEmail);
            this.pnlSendTo.Controls.Add(this.btnLoadEmail);
            this.pnlSendTo.Controls.Add(this.btnAddEmail);
            this.pnlSendTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSendTo.Location = new System.Drawing.Point(3, 3);
            this.pnlSendTo.Name = "pnlSendTo";
            this.pnlSendTo.Size = new System.Drawing.Size(795, 72);
            this.pnlSendTo.TabIndex = 0;
            // 
            // btnRemoveEmail
            // 
            this.btnRemoveEmail.Location = new System.Drawing.Point(593, 17);
            this.btnRemoveEmail.Name = "btnRemoveEmail";
            this.btnRemoveEmail.Size = new System.Drawing.Size(152, 38);
            this.btnRemoveEmail.TabIndex = 6;
            this.btnRemoveEmail.Text = "Remove Email";
            this.btnRemoveEmail.UseVisualStyleBackColor = true;
            this.btnRemoveEmail.Click += new System.EventHandler(this.btnRemoveEmail_Click);
            // 
            // btnLoadEmail
            // 
            this.btnLoadEmail.Location = new System.Drawing.Point(25, 17);
            this.btnLoadEmail.Name = "btnLoadEmail";
            this.btnLoadEmail.Size = new System.Drawing.Size(152, 38);
            this.btnLoadEmail.TabIndex = 4;
            this.btnLoadEmail.Text = "Load Excel ...";
            this.btnLoadEmail.UseVisualStyleBackColor = true;
            this.btnLoadEmail.Click += new System.EventHandler(this.btnLoadEmail_Click);
            // 
            // btnAddEmail
            // 
            this.btnAddEmail.Location = new System.Drawing.Point(435, 17);
            this.btnAddEmail.Name = "btnAddEmail";
            this.btnAddEmail.Size = new System.Drawing.Size(152, 38);
            this.btnAddEmail.TabIndex = 5;
            this.btnAddEmail.Text = "Add Email";
            this.btnAddEmail.UseVisualStyleBackColor = true;
            this.btnAddEmail.Click += new System.EventHandler(this.btnAddEmail_Click);
            // 
            // tabContent
            // 
            this.tabContent.Controls.Add(this.grpBody);
            this.tabContent.Controls.Add(this.grpSubject);
            this.tabContent.Location = new System.Drawing.Point(4, 22);
            this.tabContent.Name = "tabContent";
            this.tabContent.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
            this.tabContent.Size = new System.Drawing.Size(801, 345);
            this.tabContent.TabIndex = 3;
            this.tabContent.Text = "Content";
            this.tabContent.UseVisualStyleBackColor = true;
            // 
            // grpBody
            // 
            this.grpBody.Controls.Add(this.txtBody);
            this.grpBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBody.Location = new System.Drawing.Point(10, 68);
            this.grpBody.Name = "grpBody";
            this.grpBody.Size = new System.Drawing.Size(781, 266);
            this.grpBody.TabIndex = 15;
            this.grpBody.TabStop = false;
            this.grpBody.Text = "Body";
            // 
            // txtBody
            // 
            this.txtBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBody.Location = new System.Drawing.Point(3, 16);
            this.txtBody.Name = "txtBody";
            this.txtBody.Size = new System.Drawing.Size(775, 247);
            this.txtBody.TabIndex = 0;
            // 
            // grpSubject
            // 
            this.grpSubject.Controls.Add(this.txtSubject);
            this.grpSubject.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSubject.Location = new System.Drawing.Point(10, 11);
            this.grpSubject.Name = "grpSubject";
            this.grpSubject.Size = new System.Drawing.Size(781, 57);
            this.grpSubject.TabIndex = 14;
            this.grpSubject.TabStop = false;
            this.grpSubject.Text = "Subject";
            // 
            // txtSubject
            // 
            this.txtSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSubject.Location = new System.Drawing.Point(3, 16);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(775, 20);
            this.txtSubject.TabIndex = 10;
            // 
            // tabAttachment
            // 
            this.tabAttachment.Controls.Add(this.lstAttachment);
            this.tabAttachment.Controls.Add(this.pnlAttachment);
            this.tabAttachment.Location = new System.Drawing.Point(4, 22);
            this.tabAttachment.Name = "tabAttachment";
            this.tabAttachment.Padding = new System.Windows.Forms.Padding(3);
            this.tabAttachment.Size = new System.Drawing.Size(801, 345);
            this.tabAttachment.TabIndex = 4;
            this.tabAttachment.Text = "Attachment";
            this.tabAttachment.UseVisualStyleBackColor = true;
            // 
            // lstAttachment
            // 
            this.lstAttachment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAttachment.FormattingEnabled = true;
            this.lstAttachment.Location = new System.Drawing.Point(3, 75);
            this.lstAttachment.Name = "lstAttachment";
            this.lstAttachment.Size = new System.Drawing.Size(795, 267);
            this.lstAttachment.TabIndex = 3;
            // 
            // pnlAttachment
            // 
            this.pnlAttachment.Controls.Add(this.btnRemoveAttachment);
            this.pnlAttachment.Controls.Add(this.btnAddAttachment);
            this.pnlAttachment.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAttachment.Location = new System.Drawing.Point(3, 3);
            this.pnlAttachment.Name = "pnlAttachment";
            this.pnlAttachment.Size = new System.Drawing.Size(795, 72);
            this.pnlAttachment.TabIndex = 6;
            // 
            // btnRemoveAttachment
            // 
            this.btnRemoveAttachment.Location = new System.Drawing.Point(232, 17);
            this.btnRemoveAttachment.Name = "btnRemoveAttachment";
            this.btnRemoveAttachment.Size = new System.Drawing.Size(183, 38);
            this.btnRemoveAttachment.TabIndex = 5;
            this.btnRemoveAttachment.Text = "Remove Attachment";
            this.btnRemoveAttachment.UseVisualStyleBackColor = true;
            this.btnRemoveAttachment.Click += new System.EventHandler(this.btnRemoveAttachment_Click);
            // 
            // btnAddAttachment
            // 
            this.btnAddAttachment.Location = new System.Drawing.Point(42, 17);
            this.btnAddAttachment.Name = "btnAddAttachment";
            this.btnAddAttachment.Size = new System.Drawing.Size(162, 38);
            this.btnAddAttachment.TabIndex = 4;
            this.btnAddAttachment.Text = "Add Attachment";
            this.btnAddAttachment.UseVisualStyleBackColor = true;
            this.btnAddAttachment.Click += new System.EventHandler(this.btnAddAttachment_Click);
            // 
            // tabConfigure
            // 
            this.tabConfigure.Controls.Add(this.lstSetting);
            this.tabConfigure.Controls.Add(this.pnlSetting);
            this.tabConfigure.Location = new System.Drawing.Point(4, 22);
            this.tabConfigure.Name = "tabConfigure";
            this.tabConfigure.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfigure.Size = new System.Drawing.Size(801, 345);
            this.tabConfigure.TabIndex = 1;
            this.tabConfigure.Text = "Configure";
            this.tabConfigure.UseVisualStyleBackColor = true;
            // 
            // lstSetting
            // 
            this.lstSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSetting.FullRowSelect = true;
            this.lstSetting.GridLines = true;
            this.lstSetting.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstSetting.Location = new System.Drawing.Point(3, 75);
            this.lstSetting.MultiSelect = false;
            this.lstSetting.Name = "lstSetting";
            this.lstSetting.ShowGroups = false;
            this.lstSetting.Size = new System.Drawing.Size(795, 267);
            this.lstSetting.SmallImageList = this.imgLarge;
            this.lstSetting.TabIndex = 5;
            this.lstSetting.UseCompatibleStateImageBehavior = false;
            this.lstSetting.View = System.Windows.Forms.View.Details;
            // 
            // imgLarge
            // 
            this.imgLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgLarge.ImageSize = new System.Drawing.Size(32, 32);
            this.imgLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlSetting
            // 
            this.pnlSetting.Controls.Add(this.btnEditEmailSetting);
            this.pnlSetting.Controls.Add(this.btnRemoveEmailSetting);
            this.pnlSetting.Controls.Add(this.btnAddSetting);
            this.pnlSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSetting.Location = new System.Drawing.Point(3, 3);
            this.pnlSetting.Name = "pnlSetting";
            this.pnlSetting.Size = new System.Drawing.Size(795, 72);
            this.pnlSetting.TabIndex = 4;
            // 
            // btnEditEmailSetting
            // 
            this.btnEditEmailSetting.Location = new System.Drawing.Point(250, 13);
            this.btnEditEmailSetting.Name = "btnEditEmailSetting";
            this.btnEditEmailSetting.Size = new System.Drawing.Size(169, 43);
            this.btnEditEmailSetting.TabIndex = 3;
            this.btnEditEmailSetting.Text = "Edit Email Setting";
            this.btnEditEmailSetting.UseVisualStyleBackColor = true;
            this.btnEditEmailSetting.Click += new System.EventHandler(this.btnEditEmailSetting_Click);
            // 
            // btnRemoveEmailSetting
            // 
            this.btnRemoveEmailSetting.Location = new System.Drawing.Point(470, 13);
            this.btnRemoveEmailSetting.Name = "btnRemoveEmailSetting";
            this.btnRemoveEmailSetting.Size = new System.Drawing.Size(169, 43);
            this.btnRemoveEmailSetting.TabIndex = 2;
            this.btnRemoveEmailSetting.Text = "Remove Email Setting";
            this.btnRemoveEmailSetting.UseVisualStyleBackColor = true;
            this.btnRemoveEmailSetting.Click += new System.EventHandler(this.btnRemoveEmailSetting_Click);
            // 
            // btnAddSetting
            // 
            this.btnAddSetting.Location = new System.Drawing.Point(52, 13);
            this.btnAddSetting.Name = "btnAddSetting";
            this.btnAddSetting.Size = new System.Drawing.Size(170, 43);
            this.btnAddSetting.TabIndex = 1;
            this.btnAddSetting.Text = "Add Email Setting";
            this.btnAddSetting.UseVisualStyleBackColor = true;
            this.btnAddSetting.Click += new System.EventHandler(this.btnAddSetting_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(686, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 32);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 371);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.pnlBottom.Size = new System.Drawing.Size(809, 42);
            this.pnlBottom.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.Location = new System.Drawing.Point(10, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 32);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadMultiEmail
            // 
            this.btnLoadMultiEmail.Location = new System.Drawing.Point(183, 17);
            this.btnLoadMultiEmail.Name = "btnLoadMultiEmail";
            this.btnLoadMultiEmail.Size = new System.Drawing.Size(152, 38);
            this.btnLoadMultiEmail.TabIndex = 7;
            this.btnLoadMultiEmail.Text = "Load Multi-Excel ...";
            this.btnLoadMultiEmail.UseVisualStyleBackColor = true;
            this.btnLoadMultiEmail.Click += new System.EventHandler(this.btnLoadMultiEmail_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 413);
            this.ControlBox = false;
            this.Controls.Add(this.tabContainer);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send Email";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabContainer.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.tabSendTo.ResumeLayout(false);
            this.grpSendTo.ResumeLayout(false);
            this.pnlSendTo.ResumeLayout(false);
            this.tabContent.ResumeLayout(false);
            this.grpBody.ResumeLayout(false);
            this.grpSubject.ResumeLayout(false);
            this.grpSubject.PerformLayout();
            this.tabAttachment.ResumeLayout(false);
            this.pnlAttachment.ResumeLayout(false);
            this.tabConfigure.ResumeLayout(false);
            this.pnlSetting.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.TabPage tabConfigure;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TabPage tabContent;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.TabPage tabAttachment;
        private System.Windows.Forms.Button btnRemoveAttachment;
        private System.Windows.Forms.Button btnAddAttachment;
        private System.Windows.Forms.ListBox lstAttachment;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnRemoveEmailSetting;
        private System.Windows.Forms.Button btnAddSetting;
        private System.Windows.Forms.Button btnEditEmailSetting;
        private System.Windows.Forms.Panel pnlSetting;
        private System.Windows.Forms.Panel pnlAttachment;
        private System.Windows.Forms.GroupBox grpBody;
        private System.Windows.Forms.GroupBox grpSubject;
        private System.Windows.Forms.TabPage tabSendTo;
        private System.Windows.Forms.Panel pnlSendTo;
        private System.Windows.Forms.Button btnRemoveEmail;
        private System.Windows.Forms.Button btnLoadEmail;
        private System.Windows.Forms.Button btnAddEmail;
        private System.Windows.Forms.GroupBox grpSendTo;
        private System.Windows.Forms.ListBox lstSendTo;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lstSetting;
        private System.Windows.Forms.ImageList imgLarge;
        private UserCtrl.HtmlEditor txtBody;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoadMultiEmail;
    }
}

