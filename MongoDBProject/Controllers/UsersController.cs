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
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;
        private readonly OrderService orderService;

        public UsersController(UserService uService, OrderService oService)
        {
            userService = uService;
            orderService = oService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUser()
        {
            var vendors = await userService.GetAllUsersAsync();
            return Ok(vendors);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUserLogin(string login, string password)
        {
            var vendor = await userService.GetUserLoginAsync(login, password);
            return Ok(vendor);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{login}")]
        public async Task<ActionResult<User>> GetUserByLogin(string login)
        {
            var user = await userService.GetUserByLoginAsync(login);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            await userService.CreateUserAsync(user);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(string id, User updatedUser)
        {
            var queriedUser = await userService.GetUserByIdAsync(id);
            if (queriedUser == null)
            {
                return NotFound();
            }
            await userService.UpdateUserAsync(id, updatedUser);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(string id)
        {
            var orders = await orderService.GetUserOrdersAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            await orderService.CreateOrderAsync(order);
            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserShoppingCartById(string id)
        {
            var shoppingCart = await userService.GetUserShoppingCartByIdAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return Ok(shoppingCart);
        }
    }
}
