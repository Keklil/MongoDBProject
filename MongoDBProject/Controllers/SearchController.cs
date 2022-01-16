using System;
using MongoDBProject.Models;
using MongoDBProject.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDBProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ProductService productService;
        private readonly OrderService orderService;

        public SearchController(ProductService pService)
        {
            productService = pService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // /api/search/FilterByPrice/minPrice=10.0&maxPrice=1000.0
        [HttpGet("minPrice={minPrice}&maxPrice={maxPrice}")]
        public async Task<ActionResult<Product>> FilterByPrice(float minPrice, float maxPrice)
        {
            var result = await productService.FilterByPriceAsync(minPrice, maxPrice);
            return Ok(result);
        }
    }
}