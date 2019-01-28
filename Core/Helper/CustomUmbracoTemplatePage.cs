using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace Customs.Helper
{
    public abstract class UmbracoCustomTemplatePage : Umbraco.Web.Mvc.UmbracoTemplatePage
    {
        public UmbracoCustomHelper UmbracoCHelper
        {
            get
            {
                if (!ViewData.ContainsKey("UmbracoCustomHelper"))
                {   
                    ViewData["UmbracoCustomHelper"] = new UmbracoCustomHelper();
                }
                return (UmbracoCustomHelper)ViewData["UmbracoCustomHelper"];
            }
        }

        public IPublishedContent Content
        {
            get
            {
                if (ViewData.ContainsKey("UmbracoContentHelper"))
                {
                    return (IPublishedContent)ViewData["UmbracoContentHelper"];
                }
                else
                {
                    return CurrentPage;
                }
            }
            set
            {
                ViewData["UmbracoContentHelper"] = value;
            }
        }

        public string CurrentLanguage
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            }
        }
    }
}