namespace CodeGenerator
{
    partial class FormGroupDecorator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtLeftTypePK = new System.Windows.Forms.TextBox();
            this.cmbLeftType = new System.Windows.Forms.ComboBox();
            this.cmbRightType = new System.Windows.Forms.ComboBox();
            this.cmbParentType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "LeftType";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "RightType";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "LeftTypePK";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "ParentType";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(49, 191);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(165, 191);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtLeftTypePK
            // 
            this.txtLeftTypePK.Location = new System.Drawing.Point(88, 111);
            this.txtLeftTypePK.Name = "txtLeftTypePK";
            this.txtLeftTypePK.Size = new System.Drawing.Size(264, 20);
            this.txtLeftTypePK.TabIndex = 5;
            // 
            // cmbLeftType
            // 
            this.cmbLeftType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLeftType.FormattingEnabled = true;
            this.cmbLeftType.Location = new System.Drawing.Point(88, 28);
            this.cmbLeftType.Name = "cmbLeftType";
            this.cmbLeftType.Size = new System.Drawing.Size(264, 21);
            this.cmbLeftType.TabIndex = 10;
            this.cmbLeftType.SelectedIndexChanged += new System.EventHandler(this.cmbLeftType_SelectedIndexChanged);
            // 
            // cmbRightType
            // 
            this.cmbRightType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRightType.FormattingEnabled = true;
            this.cmbRightType.Location = new System.Drawing.Point(88, 71);
            this.cmbRightType.Name = "cmbRightType";
            this.cmbRightType.Size = new System.Drawing.Size(264, 21);
            this.cmbRightType.TabIndex = 11;
            // 
            // cmbParentType
            // 
            this.cmbParentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParentType.FormattingEnabled = true;
            this.cmbParentType.Location = new System.Drawing.Point(88, 153);
            this.cmbParentType.Name = "cmbParentType";
            this.cmbParentType.Size = new System.Drawing.Size(264, 21);
            this.cmbParentType.TabIndex = 12;
            // 
            // FormGroupDecorator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 244);
            this.Controls.Add(this.cmbParentType);
            this.Controls.Add(this.cmbRightType);
            this.Controls.Add(this.cmbLeftType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLeftTypePK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGroupDecorator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormGroupDecorator";
            this.Load += new System.EventHandler(this.FormGroupDecorator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtLeftTypePK;
        private System.Windows.Forms.ComboBox cmbLeftType;
        private System.Windows.Forms.ComboBox cmbRightType;
        private System.Windows.Forms.ComboBox cmbParentType;
    }
}