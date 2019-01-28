using Core.Models;
using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Models.Membership;


namespace Core.Controllers.SurfaceControllers
{
    public class SubscriberController : CustomUmbracoSurfaceController
    {
        [HttpPost]
        [ActionName("SendEmail")]
        public ActionResult SubmitEmail()
        {
            if (string.IsNullOrEmpty(GetRequest("name"))
                    || string.IsNullOrEmpty(GetRequest("phone"))
                    || string.IsNullOrEmpty(GetRequest("email"))
                    || string.IsNullOrEmpty(GetRequest("message")))
            {
                return Err();
            }

            if (!StringHelper.IsValidEmail(GetRequest("email")))
            {
                return Err();
            }

            try
            {
                SubscriberEmail subscriberEmail = new SubscriberEmail();
                int id = subscriberEmail.AddEmail(GetRequest("email"));
                return id != 0 ? Ret() : Err();
            }
            catch
            {
                return Err();
            }
        }


        [HttpGet]
        [ActionName("ExportEmail")]      
        public ActionResult Export()
        {
            SubscriberEmail subscriberEmail = new SubscriberEmail();
            DataTable result = subscriberEmail.GetAllEmail();
            if (result != null && result.Rows.Count > 0)
            {
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = result;
                grid.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=SubscriberEmail.xls");
                Response.ContentType = "application/excel";
                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

                grid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            return Content("Don't result!");
        }  
    }    


}
