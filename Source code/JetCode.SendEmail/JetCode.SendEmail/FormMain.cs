using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using JetCode.SendEmail.FormMisc;
using JetCode.SendEmail.Helper;

namespace JetCode.SendEmail
{
    public partial class FormMain : FormBase
    {
        private readonly SortedList<string, string> _emailList = new SortedList<string, string>();
        private readonly SortedList<string, string> _sentEmailList = new SortedList<string, string>();

        private readonly MailSettingCollection _usedSettingList = new MailSettingCollection();
        private readonly SortedList<string, int> _sendErrorIndex = new SortedList<string, int>();

        public FormMain()
        {
            InitializeComponent();
        }

        protected override void InitializeForm()
        {
            //Mail data
            string fileName = string.Format("{0}\\MailData.mal", Application.StartupPath);
            MailData data = base.Deserialize(fileName) as MailData;
            if(data != null)
            {
                this.txtSubject.Text = data.Subject;
                this.txtBody.Text = data.Body;
                foreach (string item in data.AttachmentList)
                {
                    this.lstAttachment.Items.Add(item);
                }
            }

            //Setting data
            this.lstSetting.Columns.Add("Server", 200);
            this.lstSetting.Columns.Add("Port", 100);
            this.lstSetting.Columns.Add("User", 200);
            this.lstSetting.Columns.Add("Password", 200);
            fileName = string.Format("{0}\\MailSetting.mal", Application.StartupPath);
            MailSettingCollection settingList = base.Deserialize(fileName) as MailSettingCollection;
            if (settingList != null)
            {
                foreach (MailSetting item in settingList)
                {
                    this.AddSetting(item);
                }
            }

            //Unsend Emails
            fileName = string.Format("{0}\\UnsendEmails.csv", Application.StartupPath);
            if (File.Exists(fileName))
            {
                this.LoadEmailAddress(fileName);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

           this.btnSave.PerformClick();

            //Save unsend emails
            string fileName = string.Format("{0}\\UnsendEmails.csv", Application.StartupPath);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in this._emailList)
            {
                if(this._sentEmailList.ContainsKey(pair.Key))
                    continue;

                builder.AppendLine(string.Format("{0};", pair.Value));
            }
            string text = builder.ToString();
            if (text.Length > 0)
            {
                File.WriteAllText(fileName, builder.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Save mail data
            MailData data = new MailData();
            data.Subject = this.txtSubject.Text.Trim();
            data.Body = this.txtBody.Text.Trim();
            foreach (object item in this.lstAttachment.Items)
            {
                data.AttachmentList.Add(item.ToString());
            }
            string fileName = string.Format("{0}\\MailData.mal", Application.StartupPath);
            base.Serialize(fileName, data);

            //Save setting data
            fileName = string.Format("{0}\\MailSetting.mal", Application.StartupPath);
            MailSettingCollection settingList = this.GetSettingList();
            base.Serialize(fileName, settingList);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Send Email
        private void btnSend_Click(object sender, EventArgs e)
        {
            if(this._emailList == null)
                return;

            MailSettingCollection settingList = this.GetSettingList();
            if (settingList == null || settingList.Count == 0)
                return;

            this._usedSettingList.Clear();
            this._usedSettingList.AddRange(settingList);

            this._sentEmailList.Clear();
            this._sendErrorIndex.Clear();

            this.tabSendTo.Enabled = false;
            this.tabContent.Enabled = false;
            this.tabAttachment.Enabled = false;
            this.tabConfigure.Enabled = false;

            this.btnSend.Enabled = false;
            this.btnClose.Enabled = false;
            this.btnCancel.Enabled = true;

            this.progressBar1.Enabled = true;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = this._emailList.Count;
            this.progressBar1.Step = 1;
            this.progressBar1.Value = 0;

            MailData data = new MailData();
            data.Subject = this.txtSubject.Text.Trim();
            data.Body = this.txtBody.Text.Trim();
            foreach (object item in this.lstAttachment.Items)
            {
                data.AttachmentList.Add(item.ToString());
            }

            this.WriteLog("********Send email begin...********");
            this.backgroundWorker1.RunWorkerAsync(data);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.Enabled = false;
            this.btnCancel.Text = "Please wait a minute...";
            this.backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                MailData data = e.Argument as MailData;
                if (data == null)
                    return;

                List<List<string>> batchList = new List<List<string>>();
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, string> pair in this._emailList)
                {
                    list.Add(pair.Value);

                    if(list.Count >= 30)
                    {
                        batchList.Add(list);
                        list = new List<string>();
                    }
                }

                if(list.Count > 0)
                {
                    batchList.Add(list);
                }

                foreach (List<string> item in batchList)
                {
                    if(item.Count <= 0)
                        continue;

                    if (this.backgroundWorker1.CancellationPending)
                    {
                        break;
                    }

                    this.SendEmail(item, data);
                    
                    this.backgroundWorker1.ReportProgress(item.Count);
                    Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {
                e.Result = ex;
            }
        }

        private bool SendEmail(List<string> list, MailData data)
        {
            bool success = this.SendBatchEmail(list, data);
            if (success)
            {
                int index = this._sentEmailList.Count;
                foreach (string item in list)
                {
                    index++;
                    this.WriteLog(string.Format("{0:d5}: {1} OK!", index, item));

                    this._sentEmailList.Add(item, item);
                }
            }

            return success;
        }

        private bool SendBatchEmail(List<string> list, MailData data)
        {
            if (list.Count == 0)
                return true;

            MailSetting setting = this.GetValidMailSetting();
            if (setting == null)
                return false;

            this.WriteLog(string.Format("Using {0}:", setting.SmtpServer));
            //for (int i = 0; i < 2; i++)
            {
                string error = HelperEmail.SendEmail(list, data, setting);
                if (error.Length == 0)
                    return true;

                Thread.Sleep(10000);
                this.WriteLog(error);
            }

            string key = string.Format("{0}|{1}", setting.SmtpServer, setting.SmtpUserName);
            if(!this._sendErrorIndex.ContainsKey(key))
            {
                this._sendErrorIndex.Add(key, 1);
            }
            else
            {
                this._sendErrorIndex[key]++;
            }
            
            return false;
        }

        private MailSetting GetValidMailSetting()
        {
            MailSetting result = null;

            int minCount = 10000;
            foreach (MailSetting setting in this._usedSettingList)
            {
                int errorCount;
                string key = string.Format("{0}|{1}", setting.SmtpServer, setting.SmtpUserName);
                if (!this._sendErrorIndex.ContainsKey(key))
                {
                    errorCount = 0;
                }
                else
                {
                    errorCount = this._sendErrorIndex[key];
                }

                if (errorCount < minCount)
                {
                    minCount = errorCount;
                    result = setting;
                }
            }

            return result;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value += e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Enabled = false;

            Exception ex = e.Result as Exception;
            if(ex != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(ex.Message);
                if(ex.InnerException != null)
                {
                    builder.AppendLine(ex.InnerException.Message);
                }
                builder.AppendLine();
                builder.AppendLine(string.Format("You have sent emails({0}) successfully before the failure", this._sentEmailList.Count));
                string error = builder.ToString();
                this.WriteLog(error);
                this.ShowErrorMessage(error);
            }
            else
            {
                 string info = string.Format("Congratulations, you have sent emails({0}) successfully!", this._sentEmailList.Count);
                 this.WriteLog(info);
                 this.ShowMessage(info);
            }

            foreach (KeyValuePair<string, string> pair in this._sentEmailList)
            {
                if(!this._emailList.ContainsKey(pair.Key))
                    continue;

                this._emailList.Remove(pair.Key);
            }

            this.lstSendTo.Items.Clear();
            foreach (KeyValuePair<string, string> pair in this._emailList)
            {
                this.lstSendTo.Items.Add(pair.Value);
            }

            this.tabSendTo.Enabled = true;
            this.tabContent.Enabled = true;
            this.tabAttachment.Enabled = true;
            this.tabConfigure.Enabled = true;

            this.btnSend.Enabled = true;
            this.btnClose.Enabled = true;
            this.btnCancel.Enabled = false;
            this.btnCancel.Text = "Cancel";
            this.WriteLog("********Send email end********");
        }

        private void WriteLog(string log)
        {
            this.Invoke((MethodInvoker)delegate()
                            {
                                this.txtLog.AppendText(string.Format("{0}{1}", log, Environment.NewLine));
                            });
        }
        #endregion

        #region Load Email
        private void btnLoadEmail_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = string.Empty;
            openFileDialog.Filter = "Excel Files (*.xls;*.csv)|*.xls;*.csv";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            this.LoadEmailAddress(openFileDialog.FileName);
        }

        private void btnAddEmail_Click(object sender, EventArgs e)
        {
            FormEmailAddress dlg = new FormEmailAddress();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string email = dlg.EmailAddress;
            if (this._emailList.ContainsKey(email))
                return;

            this._emailList.Add(email, email);
            this.lstSendTo.Items.Add(email);
            this.grpSendTo.Text = string.Format("Total Email Count: {0}", this._emailList.Count);
        }

        private void btnRemoveEmail_Click(object sender, EventArgs e)
        {
            int index = this.lstSendTo.SelectedIndex;
            if (index < 0)
                return;

            string email = this.lstSendTo.Items[index].ToString();
            if (!this._emailList.ContainsKey(email))
                return;

            this._emailList.Remove(email);
            this.lstSendTo.Items.RemoveAt(index);
            this.grpSendTo.Text = string.Format("Total Email Count: {0}", this._emailList.Count);
        }

        private void LoadEmailAddress(string fileName)
        {
            HelperLoader loader = new HelperLoader();
            SortedList<string, string> list = loader.LoadEmailAddress(fileName);
            if (list == null)
                return;

            foreach (KeyValuePair<string, string> pair in list)
            {
                if (this._emailList.ContainsKey(pair.Key))
                    continue;

                this._emailList.Add(pair.Key, pair.Value);
                this.lstSendTo.Items.Add(pair.Value);
            }

            this.grpSendTo.Text = string.Format("Total Email Count: {0}", this._emailList.Count);
        }
        #endregion

        #region Attachment
        private void btnAddAttachment_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = string.Empty;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            string attachment = openFileDialog.FileName;
            this.lstAttachment.Items.Add(attachment);
        }

        private void btnRemoveAttachment_Click(object sender, EventArgs e)
        {
            int index = this.lstAttachment.SelectedIndex;
            if (index < 0)
                return;

            this.lstAttachment.Items.RemoveAt(index);
        }
        #endregion

        #region Setting
        private void btnAddSetting_Click(object sender, EventArgs e)
        {
            FormEmailSetting dlg = new FormEmailSetting();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            this.AddSetting(dlg.Setting);
        }

        private void btnEditEmailSetting_Click(object sender, EventArgs e)
        {
            if (this.lstSetting.SelectedItems.Count == 0)
                return;

            MailSetting entity = this.lstSetting.SelectedItems[0].Tag as MailSetting;
            if (entity == null)
                return;

            FormEmailSetting dlg = new FormEmailSetting(entity);
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            this.DeleteSetting(entity);
            this.AddSetting(dlg.Setting);
        }

        private void btnRemoveEmailSetting_Click(object sender, EventArgs e)
        {
            if (this.lstSetting.SelectedItems.Count == 0)
                return;

            MailSetting entity = this.lstSetting.SelectedItems[0].Tag as MailSetting;
            if (entity == null)
                return;

            this.DeleteSetting(entity);
        }

        private void AddSetting(MailSetting entity)
        {
            ListViewItem item = new ListViewItem(entity.SmtpServer);
            item.Tag = entity;
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, entity.SmtpPort.ToString()));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, entity.SmtpUserName));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, entity.SmtpPassword));
            this.lstSetting.Items.Add(item);
        }

        private void DeleteSetting(MailSetting entity)
        {
            List<ListViewItem> deletedList = new List<ListViewItem>();
            foreach (ListViewItem item in this.lstSetting.Items)
            {
                if (item.Tag == null || item.Tag != entity)
                    continue;

                deletedList.Add(item);
            }

            foreach (ListViewItem item in deletedList)
            {
                this.lstSetting.Items.Remove(item);
            }
        }

        private MailSettingCollection GetSettingList()
        {
            MailSettingCollection retList = new MailSettingCollection();
            foreach (ListViewItem item in this.lstSetting.Items)
            {
                if (item.Tag == null)
                    continue;

                MailSetting entity = item.Tag as MailSetting;
                if(entity == null)
                    continue;

                retList.Add(entity);
            }

            return retList;
        }
        #endregion
    }
}
