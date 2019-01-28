using Core.Helper;
using Customs.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Core.Models
{
    public class Cart
    {
        private static Cart instance;
        private int mode;
        private string cartId;

        private Dictionary<int, dynamic> items;

        public static Cart GetInstance()
        {
            if (Cart.instance == null)
            {
                Cart.instance = new Cart();
            }
            Cart.instance.init();
            Cart.instance.Read();
            return Cart.instance;
        }

        private Cart()
        {
            this.mode = Constant.CART_MODE;
            this.cartId = "MyCart";
            this.init();
            this.Read();
        }

        public void init()
        {
            this.items = new Dictionary<int, dynamic>();
        }

        public void Add(int productId, int qty)
        {
            if(!this.IsExistedItem(productId))
            {
                this.Update(productId, qty);
            }
        }

        public Dictionary<int, dynamic> GetItems()
        {
            return this.items;
        }

        public void RemoveItem(int productId)
        {
            if(this.IsExistedItem(productId))
            {
                this.items.Remove(productId);
                this.Write();
            }
        }

        public bool IsEmptyCart()
        {
            return this.GetCartLength() == 0;
        }

        public int GetCartLength()
        {
            return this.items.Count();
        }

        public bool IsExistedItem(int productId) 
        {
            return this.items.ContainsKey(productId);
        }

        public void RemoveCart()
        {
            this.init();
            this.Write();
        }

        public void Update(int productId, int qty)
        {
            if (!this.IsExistedItem(productId))
            {
                this.items.Add(productId, new
                {
                    qty = qty
                });
            }
            else
            {
                this.items[productId] = new { qty = qty };
            }

            if (this.items[productId].qty < 0)
            {
                this.items[productId] = new { qty = 0 };
            }
            this.Write();
        }

        public dynamic GetItem(int productId)
        {
            if (this.IsExistedItem(productId))
            {
                return this.items[productId];
            }
            return null;
        }

        public void Write()
        {
            switch(this.mode)
            {
                case 1:
                    HttpContext context = HttpContext.Current;
                    context.Session[this.cartId] = JsonConvert.SerializeObject(this.items);
                break;
            }
        }

        public void Read()
        {
            switch (this.mode)
            {
                case 1:
                    HttpContext context = HttpContext.Current;
                    if(context.Session[this.cartId] != null)
                    {
                        this.items = JsonConvert.DeserializeObject<Dictionary<int, dynamic>>((string)context.Session[this.cartId]);
                    }
                break;
            }
		}

        public List<ProductCart> GetProductItems()
        {
            List<ProductCart> products = new List<ProductCart>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            foreach(KeyValuePair<int, dynamic> item in this.GetItems())
            {
                int productId = item.Key;
                IPublishedContent content = umbracoHelper.TypedContent(productId);
                ProductCart cartItem = content.GetProductCart();
                cartItem.Quantity = Convert.ToInt32(item.Value.qty);
                products.Add(cartItem);
            }
            return products;
        }

        public int GetTotalQuantity()
        {
            int qty = 0;
            foreach (KeyValuePair<int, dynamic> item in this.GetItems())
            {
                qty += Convert.ToInt32(item.Value.qty);
            }
            return qty;
        }

        public string GetTotalPrice()
        {
            return StringHelper.FormatPrice(this.GetBaseTotalPrice());
        }

        public string GetTotalPriceVAT()
        {
            return StringHelper.FormatPrice(this.GetBaseTotalPrice() + (this.GetBaseTotalPrice() * 10 /100) );
        }

        public int GetBaseTotalPrice()
        {
            int totalPrice = 0;
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            foreach (KeyValuePair<int, dynamic> item in this.GetItems())
            {
                int productId = item.Key;
                IPublishedContent content = umbracoHelper.TypedContent(productId);
                ProductCart cartItem = content.GetProductCart();
                cartItem.Quantity = Convert.ToInt32(item.Value.qty);
                totalPrice += cartItem.ItemPrice;
            }
            return totalPrice;
        }

        public ProductCart GetProductItem(int productId)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            IPublishedContent content = umbracoHelper.TypedContent(productId);
            ProductCart product = content.GetProductCart();
            product.Quantity = this.GetItem(productId).qty;
            return product;
        }
    }
}
