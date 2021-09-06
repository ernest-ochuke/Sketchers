using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository e_basketRepository;
        private readonly IMapper e_mapper;
        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            e_mapper = mapper;
            e_basketRepository = basketRepository;

        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await e_basketRepository.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var customerBasket = e_mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await e_basketRepository.UpdateBasketAsync(customerBasket);

            return Ok(updatedBasket);
        }
        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await e_basketRepository.DeleteBasketAsync(id);
        }
    }
}