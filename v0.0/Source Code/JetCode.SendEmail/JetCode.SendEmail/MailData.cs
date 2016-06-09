using System;
using System.Collections.Generic;

namespace JetCode.SendEmail
{
    [Serializable]
    public class MailData
    {
        private string _subject = string.Empty;
        private string _body = string.Empty;
        private readonly  List<string> _attachmentList = new List<string>();

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public List<string> AttachmentList
        {
            get { return _attachmentList; }
        }
    }
}
