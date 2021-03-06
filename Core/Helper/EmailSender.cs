using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Customs.Helper
{
    /// <summary>
    /// Summary description for manager
    /// </summary>
    public class EmailSender
    {
        public string SenderEmail = "";
        public string SenderName = "";
        public string ToEmail = "";
        public string Subject = "";
        public string CCEmail = "";
        public string Error = "";
        public EmailSender()
        {
            SenderEmail = ConfigurationManager.AppSettings["SENDER_EMAIL"];
            SenderName = ConfigurationManager.AppSettings["SENDER_NAME"];
        }
        public string Send(Dictionary<string, object> Data, string Template)
        {        
            string result = string.Empty;
            Template = GetMailBody(Data, Template);
            try
            {               
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IS_SMTP"]))
                {
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new MailAddress(SenderEmail, SenderName);
                    mailMessage.To.Add(new MailAddress(ToEmail));
                    mailMessage.Subject = Subject;
                    mailMessage.Body = Template;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = System.Text.UnicodeEncoding.UTF8;
                    if (CCEmail != null && CCEmail != "")
                        mailMessage.CC.Add(new MailAddress(CCEmail));

                    System.Net.Mail.SmtpClient client = new SmtpClient();
                    client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTP_USER"], ConfigurationManager.AppSettings["SMTP_PASSWORD"]);                   
                    client.Host = ConfigurationManager.AppSettings["SMTP_SERVER"];//"smtp.gmail.com";
                    client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
                    client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_SSL"]);                
                    client.Send(mailMessage);
                    result = "1";
                }
                else
                {                
                    System.Web.Mail.MailMessage mailMessage = new System.Web.Mail.MailMessage();
                    mailMessage.To = ToEmail;
                    mailMessage.From = SenderEmail;
                    if (CCEmail != null && CCEmail != "")
                        mailMessage.Cc = CCEmail;
                    mailMessage.Subject = Subject;
                    mailMessage.Body = Template;
                    mailMessage.Priority = System.Web.Mail.MailPriority.High;
                    mailMessage.BodyFormat = System.Web.Mail.MailFormat.Html;
                    mailMessage.BodyEncoding = System.Text.UnicodeEncoding.UTF8;
                    System.Web.Mail.SmtpMail.Send(mailMessage);
                    result = "okie";
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                result = Error;
            }
            return result;    
        }
        public string GetMailBody(Dictionary<string, object> data, string Template)
        {
            TextReader tr = new StreamReader(Template);
            String strMailBody = tr.ReadToEnd();
            tr.Close();
            foreach (KeyValuePair<string, object> kVal in data)
            {
                strMailBody = strMailBody.Replace("{" + kVal.Key + "}", ParseString(kVal.Value));
            }
            return strMailBody;
        }
        private string ParseString(object obj){
            try{
                return obj.ToString();
            }
            catch(Exception Exception){
            
            }
            return "";
        }   
    }
}
