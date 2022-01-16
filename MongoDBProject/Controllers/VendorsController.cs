using MongoDBProject.Models;
using MongoDBProject.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly VendorService vendorService;
        private readonly OrderService orderService;
        private readonly ProductService productService;

        public VendorsController(VendorService vService, OrderService oService, ProductService pService)
        {
            vendorService = vService;
            orderService = oService;
            productService = pService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetAllVendors()
        {
            var vendors = await vendorService.GetAllVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendorLogin(string login, string password)
        {
            var vendor = await vendorService.GetVendorLoginAsync(login, password);
            return Ok(vendor);
        }

        // GET ../api/vendors/GetVendorById/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendorById(string id)
        {
            var vendor = await vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            return Ok(vendor);
        }

        // POST ../api/vendors/createvendor
        [HttpPost]
        public async Task<IActionResult> CreateVendor(Vendor vendor)
        {
            await vendorService.CreateVendorAsync(vendor);
            return Ok(vendor);
        }

        [HttpPut( "{id}")]

        public async Task<IActionResult> UpdateVendor(string id, Vendor updatedVendor)
        {
            var queriedVendor = await vendorService.GetVendorByIdAsync(id);
            if (queriedVendor == null)
            {
                return NotFound();
            }
            await vendorService.UpdateVendorAsync(id, updatedVendor);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var vendor = await vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            await vendorService.DeleteVendorAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetVendorOrders(string id)
        {
            var orders = await orderService.GetVendorOrdersAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(string id, Order updatedOrder)
        {
            var queriedOrder = await orderService.GetOrderByIdAsync(id);
            if (queriedOrder == null)
            {
                return NotFound();
            }
            await orderService.UpdateOrderAsync(id, updatedOrder);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await productService.CreateProductAsync(product);
            return Ok(product);
        }

        // GET ../api/vendors/GetProductsByVendorId/61e3057e1c1a6d1fd4165edc
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByVendorId(string id)
        {
            var products = await productService.GetProductsByVendorIdAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await productService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ProductOrder(string id, Product updatedProduct)
        {
            var queriedProduct = await productService.GetProductByIdAsync(id);
            if (queriedProduct == null)
            {
                return NotFound();
            }
            await productService.UpdateProductAsync(id, updatedProduct);
            return NoContent();
        }
    }
}
