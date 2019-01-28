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
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Security;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;


namespace Core.Controllers.SurfaceControllers
{
    public class MemberController : CustomUmbracoSurfaceController
    {


        [HttpPost]
        [ActionName("Register")]
        public ActionResult Register()
        {

            return Ret();  
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login()
        {

            return Ret();  
        }

        [HttpGet]
        [ActionName("Logout")]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [HttpPost]
        [ActionName("SendForgotPassword")]
        public ActionResult SendForgotPassword()
        {

            return Ret();  
        }

        [HttpPost]
        [ActionName("UpdateAccount")]
        public ActionResult UpdateAccount()
        {
            return Ret();  
        }


        

    }
}
