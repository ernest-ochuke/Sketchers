using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRespository<>)));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderService,OrderService>();
             services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                                                        .SelectMany(x => x.Value.Errors)
                                                        .Select(e => e.ErrorMessage)
                                                        .ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            services.AddScoped<IBasketRepository, BasketRepository>();

            return services;
        }

    }
}