using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace JetCode.SendEmail.Helper
{
    public class HelperEmail
    {
        public static string SendEmail(List<string> sendToList, MailData data, MailSetting setting)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.Priority = MailPriority.High;
                mail.From = new MailAddress(setting.FromAddress, setting.FromDisplayName, Encoding.GetEncoding(936));
                mail.ReplyToList.Add(new MailAddress(setting.ReplyToAddress, setting.ReplyToDisplayName, Encoding.GetEncoding(936)));

                for (int i = 0; i < sendToList.Count; i++)
                {
                    string item = sendToList[i];
                    if(i == 0)
                    {
                        mail.To.Add(item);   
                    }
                    else
                    {
                        mail.Bcc.Add(item);
                    }
                }

                mail.Subject = data.Subject;
                mail.SubjectEncoding = Encoding.GetEncoding(936);
                mail.Body = data.Body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.GetEncoding(936);
                foreach (string item in data.AttachmentList)
                {
                    Attachment entity = new Attachment(item);
                    entity.NameEncoding = Encoding.GetEncoding(936);
                    mail.Attachments.Add(entity);
                }

                SmtpClient client = new SmtpClient(setting.SmtpServer, setting.SmtpPort);
                client.Credentials = new NetworkCredential(setting.SmtpUserName, setting.SmtpPassword);
                client.EnableSsl = setting.EnableSsl;
                client.Send(mail);
                return string.Empty;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool IsValidEmail(string value)
        {
            if (String.IsNullOrEmpty(value))
                return false;

            return Regex.IsMatch(value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
