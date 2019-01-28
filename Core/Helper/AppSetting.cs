using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;

namespace Customs.Helper
{
    public class AppSetting
    {
        private AppSetting() { }
        private static readonly XmlSerializer serial = new XmlSerializer(typeof(AppSettingsSerializable));
        private static AppSetting instance;
        public static AppSetting Instance
        {
            get
            {
               if(instance == null){
                   instance = new AppSetting();
               }
               Cache cache = HttpContext.Current.Cache;
               string fileDir = HttpContext.Current.Server.MapPath("~/Config/appSettings.config");
               if (cache[fileDir] != null)
               {
                   return (AppSetting)cache[fileDir];
               }
               CacheDependency dep = new CacheDependency(fileDir);
               using (StreamReader sr = new StreamReader(fileDir))
               {
                    AppSettingsSerializable settingSerilaizable = (AppSettingsSerializable)serial.Deserialize(sr);
                    instance.Settings = settingSerilaizable.Apps.ToDictionary(i => i.Lang
                                                                , i => i.SettingItems.ToDictionary(j=>j.Key,j=>j.Value) );
                    cache.Insert(fileDir, instance, dep, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
                    sr.Close();
                    return instance;
               }

            }
        }

        public string GetCurrentValue(string name)
        {
            return Settings[CultureInfo.CurrentCulture.Name][name];
        }
        
        public Dictionary<string,Dictionary<string,string>> Settings { get; set; }
    }

    [XmlRoot("appSettings"), XmlType("appSettings")]
    [System.Serializable]
    public class AppSettingsSerializable
    {
        [XmlElement("apps")]
        public AppsSerializable[] Apps { get; set; }

    }


    [System.Serializable]
    public class AppsSerializable
    {
        [XmlAttribute("lang")]
        public string Lang { get; set; }

        [XmlElement("add")]
        public AppSettingItemSerializable[] SettingItems { get; set; }
    }

    [System.Serializable]
    public class AppSettingItemSerializable
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

    }
}