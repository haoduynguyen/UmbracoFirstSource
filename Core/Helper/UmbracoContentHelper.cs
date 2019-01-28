using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Models;

public static class UmbracoContentHelper
{

    private static string languageDeault = "en";
    public static dynamic GetAliasLang(this IPublishedContent content)
    {
        string language = "";
        if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name != languageDeault)
        {
            language = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToUpper();
        }
        return language;
    }
    public static dynamic GetValue<T>(this IPublishedContent content, string alias)
    {
        return (T)content.GetProperty(alias).Value;
    }

    public static dynamic GetCustomValue(this IPublishedContent content, string alias)
    {
        string language = content.GetAliasLang();
        return content.GetProperty(alias + language) != null ? content.GetProperty(alias + language).Value : null;
    }

    public static dynamic GetCommonValue(this IPublishedContent content, string alias)
    {
        return content.HasCommonValue(alias) ? content.GetProperty(alias).Value : null;
    }

    public static int GetCommonIntValue(this IPublishedContent content, string alias)
    {

        return (int)content.GetProperty(alias).Value;
    }

    public static int GetCommonValuePickerId(this IPublishedContent content, string alias)
    {
        dynamic obj = content.GetCommonValue(alias);
        if (obj != null)
        {
            return obj.Id;
        }
        return 0;
    }

    public static bool HasCustomValue(this IPublishedContent content, string alias)
    {
        string language = content.GetAliasLang();
        return content.GetProperty(alias + language) != null 
                && content.GetProperty(alias + language).HasValue
                && content.GetProperty(alias + language).Value.GetType().FullName != typeof(Umbraco.Core.GuidUdi).FullName;
    }

    public static bool HasCommonValue(this IPublishedContent content, string alias)
    {
        return content.GetProperty(alias) != null 
                && content.GetProperty(alias).HasValue
                && content.GetProperty(alias).Value.GetType().FullName != typeof(Umbraco.Core.GuidUdi).FullName;
    }

    public static string GetCustomStringValue(this IPublishedContent content, string alias)
    {
        return content.GetValue<string>(alias);
    }

    public static dynamic GetCustomIntValue(this IPublishedContent content, string alias)
    {
        return content.GetValue<int>(alias);
    }

    public static string GetCustomCreateDate(this IPublishedContent content)
    {
        return content.CreateDate.ToString("dd/MM/yyyy");
    }
    public static string GetCreateDateDisplay(this IPublishedContent content)
    {
        return content.CreateDate.ToString("dddd dd, MMM, yyyy");
    }
    public static string GetCustomUpdateDate(this IPublishedContent content)
    {
        return content.UpdateDate.ToString("dd/MM/yyyy");
    }

    public static int GetCustomFirstMediaValue(this IPublishedContent content, string alias)
    {
        var property = content.GetValue<dynamic>(alias);
        if (property == null)
        {
            return 0;
        }
        var propertyValues = property.Split(',');
        int id = 0;
        Int32.TryParse(propertyValues[0], out id);
        return id;
    }

    public static bool CompareCommonValue(this IPublishedContent content, string alias, int id)
    {
        if (content.HasCommonValue(alias))
        {
            var values = content.GetCommonValue(alias);
            foreach (var value in values)
            {
                if (value.Id == id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static IPublishedContent GetFirstCommonValue(this IPublishedContent content, string alias)
    {
        if (content.HasCommonValue(alias))
        {
            var values = content.GetCommonValue(alias);
            foreach (var value in values)
            {
                return value;
            }
        }
        return null;
    }

    public static string GetSubUrl(this IPublishedContent content)
    {
        return content.Url.Replace("/" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower() + "/", "/");
    }

    public static string GetMedia(this IPublishedContent content, string alias)
    {
        string language = content.GetAliasLang();
        var tempValue = content.HasCustomValue(alias) ? content.GetCustomValue(alias) : "";
        tempValue = tempValue != null ? tempValue : string.Empty;
        return !string.IsNullOrWhiteSpace(tempValue.ToString()) ? string.Concat(System.Configuration.ConfigurationManager.AppSettings["MediaDomain"], tempValue) : "";
    }

    public static string GetCommonMedia(this IPublishedContent content, string alias)
    {
        var tempValue = content.HasCommonValue(alias)  ? content.GetCommonValue(alias) : "";
        tempValue = tempValue != null ? tempValue : string.Empty;
        return !string.IsNullOrWhiteSpace(tempValue.ToString()) ? string.Concat(System.Configuration.ConfigurationManager.AppSettings["MediaDomain"], tempValue) : "";
    
    }

    public static string GetUrl(this IPublishedContent content)
    {
        if (content.HasCommonValue("umbracoUrlAlias") && System.Threading.Thread.CurrentThread.CurrentUICulture.Name=="en")
        {
            return PathHelper.BaseUrl.TrimEnd('/') + content.GetCommonValue("umbracoUrlAlias"); ;
        }
        return content.Url;
    }

    public static string GetCustomMediaUrl(this IPublishedContent content, string alias)
    {
        if (content.HasCustomValue(alias))
        {
            var media = content.GetCustomValue(alias);
            return media != null ? media.Url : "";
        }
        return "";
    }

    public static string GetCommonMediaUrl(this IPublishedContent content, string alias)
    {
        if (content.HasCommonValue(alias))
        {
            var media = content.GetCommonValue(alias);
            return media != null ? media.Url : "";
        }
        return "";
    }

    public static string Description(this IPublishedContent content, string alias)
    {
        return content.HasCustomValue(alias) ? umbraco.library.RemoveFirstParagraphTag(content.GetCustomValue(alias).ToString()) : "";
    }

    public static string GetCustomLink(this IPublishedContent content)
    {
        return content.HasCommonValue("internalLink") ? content.GetCommonValue("internalLink").Url : content.GetCustomValue("externalLink");
    }

    public static string GetAlt(this IPublishedContent content, string alias)
    {
        return alias;
    }
}

