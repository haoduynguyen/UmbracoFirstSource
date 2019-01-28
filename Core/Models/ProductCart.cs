using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Core.Models
{
    public class ProductCart : Product
    {
        IPublishedContent content;

        public ProductCart(IPublishedContent content) : base(content)
        {
            this.content = content;
        }

        public int Quantity
        {
            get;
            set;
        }

        public int ItemPrice
        {
            get
            {
                return this.Quantity * this.BasicPrice;
            }
        }

        public string ItemPriceDisplay
        {
            get
            {
                return StringHelper.FormatPrice(this.ItemPrice);
            }
        }
    }
}
