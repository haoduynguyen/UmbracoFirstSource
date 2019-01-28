using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Models;
using System.Globalization;
using Umbraco.Core;
using Core.Models;
using Core.Helper;
using System.Configuration;
using System.Net.Mail;

namespace Core.Controllers.SurfaceControllers
{
    public class ContactController : CustomUmbracoSurfaceController
    {

        [HttpPost]
        [ActionName("SendContact")]
        public ActionResult SubmitContact()
        {
            if (string.IsNullOrEmpty(GetRequest("name"))
                    || string.IsNullOrEmpty(GetRequest("phone"))
                    || string.IsNullOrEmpty(GetRequest("email"))
                    || string.IsNullOrEmpty(GetRequest("message"))
                    || string.IsNullOrEmpty(GetRequest("subject")))
            {
                return Err(this.GetDictionaryValue("MSG_ERROR_SEND_FAILED"));
            }

            if (!StringHelper.IsValidEmail(GetRequest("email")))
            {
                return Err(this.GetDictionaryValue("MSG_ERROR_SEND_FAILED"));
            }
            
            try
            {
                var culture = new CultureInfo("en-GB");
                var DateNow = DateTime.Now.ToString(culture);
                var ItineraryToSubmit = Services.ContentService.CreateContent(DateNow + " - " + GetRequest("subject"), Constant.USER_CONTACT_ID, "userContact");
                ItineraryToSubmit.SetValue("userName", GetRequest("name"));
                ItineraryToSubmit.SetValue("email", GetRequest("email"));
                ItineraryToSubmit.SetValue("phone", GetRequest("phone"));
                ItineraryToSubmit.SetValue("message", GetRequest("message"));
                Services.ContentService.SaveAndPublishWithStatus(ItineraryToSubmit);

                //Sending Email
                IPublishedContent currentNode = Umbraco.TypedContent(this.CurrentPageId);
                string emailTo = currentNode.Parent.GetCommonValue("emailTo");
                if (emailTo != "")
                {
                    this.SendMail(emailTo, this.GetDictionaryValue("EMAIL_TEMPLATE_CONTACT_SUBJECT")
                                    , this.GetDictionaryValue("EMAIL_TEMPLATE_CONTACT_BODY").Replace("{{fullname}}", GetRequest("fullname"))
                                            .Replace("{{email}}", GetRequest("email"))
                                            .Replace("{{phone}}", GetRequest("phone"))
                                            .Replace("{{message}}", GetRequest("message"))
                                            .Replace("{{subject}}", GetRequest("subject")));
                }
                return Ret(this.GetDictionaryValue("MSG_SEND_SUCCESSFULLY"));
            }
            catch(Exception e)
            {
                return Err(this.GetDictionaryValue("MSG_ERROR_SEND_FAILED"));
            }           
        }

    }
}
