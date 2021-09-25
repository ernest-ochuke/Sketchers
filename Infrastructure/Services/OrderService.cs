using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;

namespace Infrastructure.Services
{

    public class OrderService : IOrderService
    {
        private readonly IPaymentService e_paymentService;

        private readonly IBasketRepository e_basketRepo;
        private readonly IUnitOfWork e_unitOfWork;
        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            e_paymentService = paymentService;
            e_unitOfWork = unitOfWork;
            e_basketRepo = basketRepo;
            
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketid, Address shippingAddress)
        {
            var basket = await e_basketRepo.GetBasketAsync(basketid);

            var items = new List<OrderItem>();
            if(!basket.Items.Any())
            {
                return null;
            }
            foreach (var item in basket.Items)
            {
                var productItem = await e_unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            var delivryMethod = await e_unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var spec = new OrderByPaymentIntentIdWithItemsSpecification(basket.PaymentIntentId);
            var existingOrder = await e_unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (existingOrder != null)
            {
                e_unitOfWork.Repository<Order>().Delete(existingOrder);
                await e_paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
            }
            //Create Order
            var order = new Order(items, buyerEmail, shippingAddress, delivryMethod, subtotal, basket.PaymentIntentId);

            e_unitOfWork.Repository<Order>().Add(order);
            //Save to DB
            var result = await e_unitOfWork.Complete();

            if (result <= 0) return null;

            // Delete Basket
            await e_basketRepo.DeleteBasketAsync(basketid);
            //Return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await e_unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await e_unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await e_unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}