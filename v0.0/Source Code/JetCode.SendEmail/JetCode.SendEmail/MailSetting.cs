using System;

namespace JetCode.SendEmail
{
    [Serializable]
    public class MailSetting
    {
        private string _smtpServer = string.Empty;
        private int _smtpPort = 25;
        private string _smtpUserName = string.Empty;
        private string _smtpPassword = string.Empty;

        private string _fromAddress = string.Empty;
        private string _fromDisplayName = string.Empty;
        private string _replyToAddress = string.Empty;
        private string _replyToDisplayName= string.Empty;
      
        public string SmtpServer
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        public int SmtpPort
        {
            get { return _smtpPort; }
            set { _smtpPort = value; }
        }

        public string SmtpUserName
        {
            get { return _smtpUserName; }
            set { _smtpUserName = value; }
        }

        public string SmtpPassword
        {
            get { return _smtpPassword; }
            set { _smtpPassword = value; }
        }

        public string FromDisplayName
        {
            get { return _fromDisplayName; }
            set { _fromDisplayName = value; }
        }

        public string FromAddress
        {
            get { return _fromAddress; }
            set { _fromAddress = value; }
        }

        public bool EnableSsl
        {
            get { return this.SmtpPort != 25; }
        }

        public string ReplyToAddress
        {
            get { return _replyToAddress; }
            set { _replyToAddress = value; }
        }

        public string ReplyToDisplayName
        {
            get { return _replyToDisplayName; }
            set { _replyToDisplayName = value; }
        }

        public override bool Equals(object obj)
        {
            return ((obj.GetType() == this.GetType()) && this.InternalEquals(obj as MailSetting));
        }

        private bool InternalEquals(MailSetting obj)
        {
            if (this.SmtpServer.CompareTo(obj.SmtpServer) != 0)
            {
                return false;
            }

            if (this.SmtpUserName.CompareTo(obj.SmtpUserName) != 0)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}",this.SmtpServer, this.SmtpUserName);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
