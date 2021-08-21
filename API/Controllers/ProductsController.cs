using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> e_productsRepo;
        private readonly IGenericRepository<ProductBrand> e_productBrandRepo;
        private readonly IGenericRepository<ProductType> e_productTypeRepo;
        private readonly IMapper e_mapper;

        public ProductsController(IGenericRepository<Product> ProductsRepo,
                                  IGenericRepository<ProductBrand> productBrandRepo,
                                  IGenericRepository<ProductType> ProductTypeRepo, IMapper mapper)
        {
            e_mapper = mapper;
            e_productsRepo = ProductsRepo;
            e_productBrandRepo = productBrandRepo;
            e_productTypeRepo = ProductTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await e_productsRepo.ListAsync(spec);
            var prodToReturns = e_mapper.Map<IReadOnlyList<Product>,
                                         IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(prodToReturns);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await e_productsRepo.GetEntityWithSpec(spec);

            return e_mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await e_productBrandRepo.ListAllAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await e_productTypeRepo.ListAllAsync();
            return Ok(types);
        }
    }
}