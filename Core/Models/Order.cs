using Core.Helper;
using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Order
    {

        public static bool AddOrder(dynamic order, Cart cart) 
        {
            DataAccess model = new DataAccess();
            Dictionary<string, object> dataInsert = new Dictionary<string, object>();
            dataInsert.Add("title",order.name + " - " + order.phone + " - " + DateTime.Now.ToShortDateString());
            if (order.email != "")
            {
                dataInsert.Add("customer_email", order.email);
            }
            dataInsert.Add("customer_name", order.name);
            dataInsert.Add("customer_phone", order.phone);
            dataInsert.Add("customer_address", order.address);
            dataInsert.Add("customer_city", order.city);
            dataInsert.Add("customer_district", order.district);
            dataInsert.Add("customer_ward", order.ward);
            if (order.invoice != "")
            {
                dataInsert.Add("customer_invoice", order.invoice);
            }
            dataInsert.Add("status", Constant.ORDER_NOT_APPROVED);
            dataInsert.Add("total", order.total + (order.total*10/100));
            int orderId = model.InsertTable("catalogOrder", dataInsert);
            if (orderId != 0)
            {
                List<ProductCart> products = cart.GetProductItems();
                foreach (var item in products)
                {
                    Order.AddOrderItem(orderId, item);
                }
                return true;
            }
            return false;
        }

        public static int AddOrderItem(int orderId, ProductCart cartItem)
        {
            DataAccess model = new DataAccess();
            Dictionary<string, object> dataInsert = new Dictionary<string, object>();
            dataInsert.Add("order_id", orderId);
            dataInsert.Add("product_id", cartItem.Id);
            dataInsert.Add("product_sku", cartItem.Sku);
            dataInsert.Add("product_title", cartItem.Name);
            dataInsert.Add("qty", cartItem.Quantity);
            dataInsert.Add("price", cartItem.BasicPrice);
            int orderItemId = model.InsertTable("catalogOrderItem", dataInsert);
            return orderItemId;
        }
    }
}
