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
using Core.Helper;
using System.Configuration;
using System.Net.Mail;

namespace Core.Controllers.SurfaceControllers
{
    public class ProductController : CustomUmbracoSurfaceController
    {

        [HttpGet]
        [ActionName("GetDetail")]
        public ActionResult GetDetail()
        {
            var helper = new UmbracoCustomHelper();
            IPublishedContent currentNode = Umbraco.TypedContent(Convert.ToInt32(GetRequest("product_id")));
            Product product = new Product(currentNode);
            List<string> gallery = new List<string>();
            var caseImagesList = currentNode.GetCustomValue("gallery");
            foreach (var item in caseImagesList)
            {
                var media = helper.GetMedia(item.Id);
                gallery.Add(media);
            }          
            return Ret("", 1, new { name = product.Name, price = product.Price, additional = product.Additional, description = product.Description, gallery = gallery });
            
        }

    }
}
