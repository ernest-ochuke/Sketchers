using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    context.Database.OpenConnection();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrands] ON");
                    var bradsData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                    var brands = JsonConvert.DeserializeObject<List<ProductBrand>>(bradsData);

                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }
                    await context.SaveChangesAsync();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrands] OFF");
                    context.Database.CloseConnection();
                }

                if (!context.ProductTypes.Any())
                {
                    context.Database.OpenConnection();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductTypes] ON");
                    var typesData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                    var types = JsonConvert.DeserializeObject<List<ProductType>>(typesData);

                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductTypes] OFF");
                    context.Database.CloseConnection();
                }
                if (!context.Products.Any())
                {
                   
                    var productsData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var logger = loggerFactory.CreateLogger<StoreContextSeed>();

                    //  var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    var products = JsonConvert.DeserializeObject<List<Product>>(productsData);
                    logger.LogInformation("productCount " + products.Count());
                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();
                   
                }
                if (!context.DeliveryMethods.Any())
                {


                    context.Database.OpenConnection();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[DeliveryMethods] ON");
                    var dmData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");

                    var methods = JsonConvert.DeserializeObject<List<DeliveryMethod>>(dmData);

                     var logger = loggerFactory.CreateLogger<StoreContextSeed>();

                   
                     logger.LogInformation("Delivery Method " + methods.Count());
                    foreach (var method in methods)
                    {
                        context.DeliveryMethods.Add(method);
                    }
                    await context.SaveChangesAsync();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[DeliveryMethods] OFF");
                    context.Database.CloseConnection();
                }
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}