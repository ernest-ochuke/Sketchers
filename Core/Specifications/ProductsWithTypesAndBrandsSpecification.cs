using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productsParams)
            : base(x =>
            (string.IsNullOrEmpty(productsParams.Search) || x.Name.ToLower().Contains(productsParams.Search))&&
            (!productsParams.BrandId.HasValue || x.ProductBrandId == productsParams.BrandId)
                && (!productsParams.TypeId.HasValue || x.ProductTypeId == productsParams.TypeId))
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);

            AddOrderBy(x => x.Name);
            ApplyPaging(productsParams.PageSize * (productsParams.PageIndex - 1),
            productsParams.PageSize);

            if (!string.IsNullOrEmpty(productsParams.Sort))
            {
                switch (productsParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDsc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}