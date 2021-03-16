using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using serverAPI.Database;
using serverAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        IShopRepository _db;
        IConfiguration _configuration;

        public ShopsController(IShopRepository db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// Return user by id.
        /// </summary>
        /// <response code="200">Returns user</response>
        /// <response code="404">If the user doesn't exist</response>
        [HttpGet]
        public JsonResult GetShops()
        {
            List<ShopDTO> shops = _db.Get();

            return new JsonResult(Ok(shops));
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Users
        ///     {
        ///        "purchases": [
        ///             {
        ///                 "name": "apple",
        ///                 "purchaseDate": "2000-01-01T01:01:01",
        ///                 "cost: 1000
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created user</returns>
        /// <response code="200">Returns the newly created user</response>
        /// <response code="400">If the user is null or invalid</response>      
        [HttpPost]
        public JsonResult PostShop(SuperAdminRequest request)
        {
            if (!isSuperAdmin(request.RootPassword))
                return new JsonResult(StatusCode(403));

            Shop shop = request.Shop;
            if (!isShopValid(shop))
                return new JsonResult(BadRequest());

            shop = _db.Add(shop);

            return new JsonResult(Ok(shop));
        }

        /// <summary>
        /// Delete user by id.
        /// </summary>
        /// <response code="200">Returns deleted user</response>
        /// <response code="404">If the user doesn't exist</response>
        [HttpDelete]
        public JsonResult DeleteShop(SuperAdminRequest request)
        {
            if (!isSuperAdmin(request.RootPassword))
                return new JsonResult(Forbid());

            uint id = request.Shop.Id;
            Shop shop = _db.Get(id);

            if (shop == null)
                return new JsonResult(NotFound());

            _db.Delete(id);
            return new JsonResult(Ok(shop));
        }

        public virtual bool isShopValid(Shop shop)
        {
            if (shop == null ||
                shop.Name == null ||
                shop.PhoneNumber == null ||
                shop.Address == null ||
                shop.Password == null)
                return false;

            return true;
        }

        private bool isSuperAdmin(string password)
        {
            return password == _configuration.GetValue<string>("RootPassword");
        }

    }
}
