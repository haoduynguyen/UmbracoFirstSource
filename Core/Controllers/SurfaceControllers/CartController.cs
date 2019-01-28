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
    public class CartController : CustomUmbracoSurfaceController
    {


        [HttpPost]
        [ActionName("Add")]
        public ActionResult Add()
        {
            try
            {
                int productId = Convert.ToInt32(GetRequest("productId"));
                int qty = Convert.ToInt32(GetRequest("qty"));
                Cart cart = Cart.GetInstance();
                if (!cart.IsExistedItem(productId))
                {
                    cart.Add(productId, qty);
                }
                return Ret(this.GetDictionaryValue("MSG_CART_ADDED_SUCCESSFULLY"), 1, new {  total_quantity = cart.GetTotalQuantity() });
            }
            catch(Exception ex)
            {
                return Err(this.GetDictionaryValue("MSG_CART_ADDED_FAILED"));
            }
        }

        [HttpPost]
        [ActionName("Update")]
        public ActionResult Update()
        {
            try
            {
                int productId = Convert.ToInt32(GetRequest("productId"));
                int qty = Convert.ToInt32(GetRequest("qty"));
                Cart cart = Cart.GetInstance();
                cart.Update(productId, qty);
                ProductCart product = cart.GetProductItem(productId);
                return Ret(this.GetDictionaryValue("MSG_CART_ADDED_SUCCESSFULLY"), 1, new { quantity = qty, item_price = (product != null ? product.ItemPriceDisplay : ""), total_price = cart.GetTotalPrice(), total_quantity = cart.GetTotalQuantity(), total_price_vat = cart.GetTotalPriceVAT() });
            }
            catch(Exception ex)
            {
                return Err(this.GetDictionaryValue("MSG_CART_ADDED_FAILED"));
            }
        }

        [HttpPost]
        [ActionName("Remove")]
        public ActionResult Remove()
        {
            try
            {
                int productId = Convert.ToInt32(GetRequest("productId"));
                Cart cart = Cart.GetInstance();
                cart.RemoveItem(productId);
                return Ret(this.GetDictionaryValue("MSG_SEND_SUCCESSFULLY"), 1, new { total_price = cart.GetTotalPrice(), total_quantity = cart.GetTotalQuantity(), total_price_vat = cart.GetTotalPriceVAT() });
            }
            catch
            {
                return Err(this.GetDictionaryValue("MSG_ERROR_SEND_FAILED"));
            }
        }

        [HttpPost]
        [ActionName("SendShipping")]
        public ActionResult SendShipping()
        {
            try
            {
                ShippingAddress address = new ShippingAddress();
                address.Save(new
                {
                    name = GetRequest("name"),
                    email = GetRequest("email"),
                    phone = GetRequest("phone"),
                    address = GetRequest("address"),
                    city = GetRequest("city"),
                    district = GetRequest("district"),
                    ward = GetRequest("ward"),
                    invoice = GetRequest("invoice")
                });
                return Ret(this.GetDictionaryValue("MSG_SEND_SUCCESSFULLY"));
            }
            catch
            {
                return Err(this.GetDictionaryValue("MSG_ERROR_SEND_FAILED"));
            }
        }

        [HttpPost]
        [ActionName("Payment")]
        public ActionResult Payment()
        {
            try
            {
                ShippingAddress address = new ShippingAddress();
                Cart cart = Cart.GetInstance();
                if(cart.IsEmptyCart()){
                    return Err(this.GetDictionaryValue("CART_EMPTY_CART"), 0, new { data_miss_type = 1  });
                }
                else if (!address.IsValid())
                {
                    return Err(this.GetDictionaryValue("CART_EMPTY_SHIPPING_ADDRESS"), 0, new { data_miss_type = 2 });
                }
                if (Order.AddOrder(new {
                    name = address.Name,
                    email = address.Email,
                    phone = address.Phone,
                    address = address.Address,
                    city = address.City,
                    district = address.District,
                    ward = address.Ward,
                    invoice = address.Invoice,
                    total = cart.GetBaseTotalPrice()
                }, cart))
                {

                    //Sending Email
                    var emailTo = address.Email;
                    if (emailTo != "")
                    {
                        string productItemsPattern = this.GetDictionaryValue("EMAIL_TEMPLATE_PRODUCT_ITEM_BODY");
                        string productItemsTemplate = "";
                        List<ProductCart> products = cart.GetProductItems();
                        foreach (var item in products)
                        {
                            productItemsTemplate += productItemsPattern.Replace("{{product_title}}", item.Name)
                                                                       .Replace("{{sku}}", item.Sku)
                                                                       .Replace("{{price}}", item.Price)
                                                                       .Replace("{{quantity}}", item.Quantity.ToString())
                                                                       .Replace("{{total_price}}", item.ItemPriceDisplay);
                        }
                        
                        this.SendMail(emailTo, this.GetDictionaryValue("EMAIL_TEMPLATE_ORDER_SUBJECT")
                                        , this.GetDictionaryValue("EMAIL_TEMPLATE_ORDER_BODY")
                                                .Replace("{{fullname}}", address.Name)
                                                .Replace("{{email}}", address.Email)
                                                .Replace("{{phone}}", address.Phone)
                                                .Replace("{{address}}", address.Address)
                                                .Replace("{{city}}", address.City)
                                                .Replace("{{district}}", address.District)
                                                .Replace("{{ward}}", address.Ward)
                                                .Replace("{{total}}", cart.GetTotalPrice())
                                                .Replace("{{total_vat}}", cart.GetTotalPriceVAT())
                                                .Replace("{{product_items}}", productItemsTemplate)
                                                .Replace("{{invoice}}", address.Invoice));
                    }
                    cart.RemoveCart();

                    return Ret(this.GetDictionaryValue("MSG_SEND_SUCCESSFULLY"), 1, new { url_redirect = this.GetDictionaryValue("URL_ORDER_SUCCESS") });
                }
                else
                {
                    return Err(this.GetDictionaryValue("MSG_ERROR_SEND_FAILED"));
                }
            }
            catch
            {
                return Err(this.GetDictionaryValue("MSG_ERROR_SEND_FAILED"));
            }
        }
    }
}
