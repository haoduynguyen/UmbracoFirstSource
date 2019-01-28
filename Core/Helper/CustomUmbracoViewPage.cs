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
    public abstract class UmbracoCustomViewPage<T> : Umbraco.Web.Mvc.UmbracoViewPage<T> 
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
    }
}