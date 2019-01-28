using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Core.Models
{
    public class Product
    {
        IPublishedContent content;
        
        public Product(IPublishedContent content)
        {
            this.content = content;
        }

        public int Id
        {
            get
            {
                return this.content.Id;
            }
        }

        public string Name
        {
            get
            {
                return this.content.GetCustomValue("title");
            }
        }

        public string Description
        {
            get
            {
                return this.content.Description("description");
            }
        }

        public string Additional
        {
            get
            {
                return this.content.Description("additional");
            }
        }

        public string FeatureImage
        {
            get
            {
                return this.content.GetMedia("featureImage");
            }
        }

        public string Sku
        {
            get
            {
                return this.content.GetCommonValue("sku");
            }
        }

        public string Price
        {
            get
            {
                try
                {
                    var price = content.HasCommonValue("salePrice") ? content.GetCommonValue("salePrice") : content.GetCommonValue("regularPrice");

                    return StringHelper.FormatPrice(price);
                }
                catch
                {
                    return StringHelper.FormatPrice(0);
                }
            }
        }

        public int BasicPrice
        {
            get
            {
                var price = content.HasCommonValue("salePrice") ? content.GetCommonValue("salePrice").ToString() : content.GetCommonValue("regularPrice").ToString();
                int priceInt = 0;
                if (price != "")
                {
                    Int32.TryParse(price, out priceInt);
                }
                return priceInt;
            }
        }

        public string Url
        {
            get
            {
                return this.content.Url;
            }
        }

        public string Promotion
        {
            get
            {
                return this.content.GetCustomValue("promotion");
            }
        }

        public IPublishedContent Category
        {
            get
            {
                return this.content.HasCommonValue("category") ? this.content.GetFirstCommonValue("category") : null;
            }
        }

        public int CategoryId
        {
            get
            {
                return this.content.HasCommonValue("category") 
                    && this.content.GetFirstCommonValue("category") != null ? this.content.GetFirstCommonValue("category").Id : 0;
            }
        }

        public string CategoryName
        {
            get
            {
                return this.content.HasCommonValue("category") && this.content.GetFirstCommonValue("category") != null  
                    ? this.content.GetFirstCommonValue("category").GetCustomValue("title") : "";
            }
        }

        public string CategoryIcon
        {
            get
            {
                return this.content.HasCommonValue("category")
                        && this.content.GetFirstCommonValue("category") != null
                        && this.content.GetFirstCommonValue("category").HasCommonValue("icon") ?
                    this.content.GetFirstCommonValue("category").GetCommonValue("icon")
                    : "";
            }
        }
    }
}
