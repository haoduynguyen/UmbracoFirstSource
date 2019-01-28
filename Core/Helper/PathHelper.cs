using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Customs.Helper
{
    public class PathHelper
    {

        public static string SiteUrl
        {
            get
            {
                HttpContext context = HttpContext.Current;
                string port = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["WEBSITE_PORT"]) ? ":" + ConfigurationManager.AppSettings["WEBSITE_PORT"] : "";
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SITE_URL"]))
                    return ConfigurationManager.AppSettings["SITE_URL"].TrimEnd('/');
                return (context.Request.Url.Scheme + "://" + context.Request.Url.Authority + "" + port + context.Request.ApplicationPath.TrimEnd('/'));
            }
        }
        public static string Webroot
        {
            get
            {
                HttpContext context = HttpContext.Current;
                return context.Request.ApplicationPath.TrimEnd('/') + '/';
            }
        }
        public static string Domain
        {
            get
            {
                HttpContext context = HttpContext.Current;
                string port = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["WEBSITE_PORT"]) ? ":" + ConfigurationManager.AppSettings["WEBSITE_PORT"] : "";
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SITE_URL"]))
                    return ConfigurationManager.AppSettings["SITE_URL"].ToLower().Replace(Webroot, "").TrimEnd('/');
                return context.Request.Url.Scheme + "://" + context.Request.Url.Authority + "" + port;
            }
        }
        public static string BaseUrl
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context.Request.Browser.Type.ToUpper().Contains("IE") && Convert.ToDouble(context.Request.Browser.Version) < 10)
                    return context.Request.Url.GetLeftPart(UriPartial.Authority) + PathHelper.Webroot;
                return PathHelper.Webroot;

            }
        }        
        public static string GetIP()
        {
            HttpContext context = HttpContext.Current;
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public static string ToAbsoluteUrl( string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
        public static string WebrootContent
        {
            get
            {
                HttpContext context = HttpContext.Current;
                return context.Request.ApplicationPath.TrimEnd('/');
            }
        }

    }
}