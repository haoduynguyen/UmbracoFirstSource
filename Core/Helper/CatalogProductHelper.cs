using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

public static class CatalogProductHelper
{
    public static dynamic GetProducts(this IPublishedContent content, string alias)
    {
        return content.HasCommonValue(alias) ? content.GetProperty(alias).Value : null;
    }

    public static dynamic GetProduct(this IPublishedContent content)
    {
        return new Product(content);
    }

    public static dynamic GetProductCart(this IPublishedContent content)
    {
        return new ProductCart(content);
    }
}