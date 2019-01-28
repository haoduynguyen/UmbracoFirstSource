using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace Customs.Helper
{
    public class CustomUmbracoSurfaceController : SurfaceController
    {
        protected UmbracoCustomHelper Helper { get; set; }
        public string Language { get { return GetRequest("globalLanguage"); } }
        public int CurrentPageId { get { return GetIntRequest("globalCurrentPageId"); } }
        public int CultureLCID { get { return GetIntRequest("globalCultureLCID"); } }


        private IPublishedContent customCurrentPage;
        protected IPublishedContent CustomCurrentPage
        {
            get
            {
                if (this.CurrentPageId != 0)
                {
                    customCurrentPage = Umbraco.TypedContent(CurrentPageId);
                } 
                return customCurrentPage;
            }
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (CultureLCID != 0)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(CultureLCID);
                Helper = new UmbracoCustomHelper();
            }
            this.InitContent();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                System.IO.File
                        .AppendAllText(Server.MapPath("~/App_Data/logs/") + "Application_Error_" + DateTime.Now.ToString("ddMMyyyy") + ".log"
                                        , DateTime.Now.ToString()
                                        + " - Url: "
                                        + Request.Url
                                        + Environment.NewLine
                                        + filterContext.Exception.ToString()
                                        + Environment.NewLine
                                        + Environment.NewLine);
                filterContext.ExceptionHandled = true;
            }
        }

        protected string GetRequest(string name, string escapeValue = "")
        {
            return Request[name] != null ? Request[name].Trim() : escapeValue;
        }

        protected int GetIntRequest(string name, int escapeValue = 0)
        {
            int output;
            if (Request[name] != null && int.TryParse(Request[name], out output))
            {
                return output;
            }
            else
            {
                return escapeValue;
            }
        }
        protected double GetDoubleRequest(string name, int escapeValue = 0)
        {
            double output;
            if (Request[name] != null && double.TryParse(Request[name], out output))
            {
                return output;
            }
            else
            {
                return escapeValue;
            }
        }
        protected int GetId(string name)
        {
            return this.GetIntRequest(name);
        }

        protected bool GetBoolRequest(string name, bool escapeValue = false)
        {
            bool output;
            if (Request[name] != null && bool.TryParse(Request[name], out output))
            {
                return output;
            }
            else
            {
                return escapeValue;
            }
        }

        protected virtual void InitContent() { }

        protected string GetDictionaryValue(String key)
        {
            int CultureId = Session["CultureId"] != null ? (int)Session["CultureId"] : System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(CultureId);
            Umbraco.Web.UmbracoHelper helper = new Umbraco.Web.UmbracoHelper(UmbracoContext);
            return helper.GetDictionaryValue(key);  
        }

        protected JsonResult Ret(object message = null, int status = 1, object data = null)
        {
            return Json(new
            {
                status = status,
                message = message,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Err(object message = null, int status = 0, object data = null)
        {
            return Json(new
            {
                status = status,
                message = message,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        protected bool SendMail(string emailTo, string subject, string body)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTP_USER"], ConfigurationManager.AppSettings["SMTP_PASSWORD"]);
                smtpClient.Host = ConfigurationManager.AppSettings["SMTP_SERVER"];
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_SSL"]);

                MailMessage mailToClient = new MailMessage();
                mailToClient.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"], ConfigurationManager.AppSettings["SENDER_NAME"]);
                mailToClient.To.Add(new MailAddress(emailTo));
                if (ConfigurationManager.AppSettings["CC_EMAIL"] != "")
                {
                    string[] emailCC = ConfigurationManager.AppSettings["CC_EMAIL"].Split(';');
                    for (int i = 0; i < emailCC.Length; i++)
                    {
                        mailToClient.CC.Add(new MailAddress(emailCC[i]));
                    }
                }
                mailToClient.Subject = subject;
                mailToClient.Body = body;
                mailToClient.IsBodyHtml = true;
                mailToClient.BodyEncoding = System.Text.UnicodeEncoding.UTF8;
                smtpClient.Send(mailToClient);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
            
        }
    }

}
