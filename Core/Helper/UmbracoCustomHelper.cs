using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Customs.Helper
{
    public class UmbracoCustomHelper
    {
        private IPublishedContent contentRoot;

        public UmbracoCustomHelper()
        {
            this.Value = new Umbraco.Web.UmbracoHelper(UmbracoContext.Current);
        }

        public UmbracoHelper Value
        {
            get;
            set;
        }
        
        public IPublishedContent GetContentRoot(){
            if (contentRoot == null)
            {
                contentRoot = Value.TypedContent(UmbracoContext.Current.PageId.Value)
                        .AncestorOrSelf(1); 
            }
            return contentRoot;
        }

        public dynamic MediaFile(object v)
        {

            if (v == null || string.IsNullOrWhiteSpace(v.ToString())) return null; 
            var media = this.Value.Media(v);
            return media != null ? string.Concat(System.Configuration.ConfigurationManager.AppSettings["MediaDomain"], media.umbracoFile.src) : null;
        }

        public Dictionary<string, List<IPublishedContent>> GroupContent(string alias,IEnumerable<IPublishedContent> enums)
        {
            Dictionary<string, List<IPublishedContent>> dictionaries = new Dictionary<string, List<IPublishedContent>>();
            foreach (var item in enums)
            {
                if (item.HasValue(alias))
                {
                    var section = item.GetCustomStringValue(alias);
                    if (!dictionaries.ContainsKey(section))
                    {
                        dictionaries.Add(section, new List<IPublishedContent>());
                    }
                    dictionaries[section].Add(item);
                }
            }
            return dictionaries;
        }

        public int GetFirstMediaValue(IPublishedContent content,string alias)
        {
            var property = content.GetCustomStringValue(alias);
            if (property == null)
            {
                return 0;
            }
            var propertyValues = property.Split(',');
            int id = 0;
            Int32.TryParse(propertyValues[0], out id);
            return id;
        }

        public string Date(DateTime date)
        {
            return date.ToString("dd-MM-yyyy");
        }

        public string NiceUrl(int id)
        {
            return umbraco.library.NiceUrl(id);
        }

        public bool HasNodePickerRelation(IPublishedContent content, string alias, int id)
        {
            return  content.GetCommonValue(alias) != null ?  Array.IndexOf(content.GetCommonValue(alias).Split(','), id.ToString()) > -1 : false;
        }

        public string GetDictionaryValue(String key)
        {
            return Value.GetDictionaryValue(key);
        }

        public IPublishedContent GetContent(String alias)
        {
            return Value.TypedContentSingleAtXPath("//"+alias);
        }

        public string GetMedia(int id)
        {
            var image = Value.Media(id);
            return image != null ? image.url : "";
        }
    }
}
