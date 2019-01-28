using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Models
{
    public class ShippingAddress
    {
        public ShippingAddress()
        {
            HttpContext context = HttpContext.Current;

            if (HttpContext.Current.Session[this.sessionName] != null)
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>((string)HttpContext.Current.Session[this.sessionName]);
                this.Name = data.name;
                this.Email = data.email;
                this.Phone = data.phone;
                this.City = data.city;
                this.Address = data.address;
                this.District = data.district;
                this.Ward = data.ward;
                this.Invoice = data.invoice;
            }
        }

        public string sessionName = "shipping-address";
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Invoice { get; set; }
        public void Save(dynamic data)
        {
            HttpContext.Current.Session[this.sessionName] = JsonConvert.SerializeObject(data);
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Name)
                && !string.IsNullOrEmpty(this.Phone)
                && !string.IsNullOrEmpty(this.City)
                && !string.IsNullOrEmpty(this.Address)
                && !string.IsNullOrEmpty(this.District)
                && !string.IsNullOrEmpty(this.Ward);
        }
    }
}
