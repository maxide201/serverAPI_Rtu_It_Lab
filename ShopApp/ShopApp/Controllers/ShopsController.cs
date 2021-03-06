using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopApp.Database;
using ShopApp.Models;
using System.Collections.Generic;

namespace ShopApp.Controllers
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
        /// Return all shops.
        /// </summary>
        /// <response code="200">Return list of shops</response>
        [HttpGet]
        public JsonResult GetShops()
        {
            List<ShopDTO> shops = _db.Get();

            return _Response.Ok(shops);
        }

        /// <summary>
        /// Post new shop (for SuperAdmin).
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /api/Shops
        ///     {
        ///         "rootPassword": "bigsecret",
        ///         "shop": {
        ///             "Name": "shop1",
        ///             "phoneNumber": "88005553535",
        ///             "Address": "Moscow",
        ///             "Password": "1"
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return shop</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="403">If root password incorrect</response>  
        [HttpPost]
        public JsonResult PostShop(SuperAdminRequest request)
        {
            if (!isSuperAdmin(request.RootPassword))
                return _Response.Forbid();

            Shop shop = request.Shop;
            if (!isShopValid(shop))
                return _Response.BadRequest();

            shop = _db.Add(shop);

            return _Response.Ok(shop);
        }

        /// <summary>
        /// Delete shop (for SuperAdmin).
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /api/Shops
        ///     {
        ///         "rootPassword": "bigsecret",
        ///         "shop": {
        ///             "id":1
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return deleted shop</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="403">If root password incorrect</response>  
        [HttpDelete]
        public JsonResult DeleteShop(SuperAdminRequest request)
        {
            if (!isSuperAdmin(request.RootPassword))
                return _Response.Forbid();

            uint id = request.Shop.Id;
            Shop shop = _db.Get(id);

            if (shop == null)
                return _Response.NotFound();

            _db.Delete(id);
            return _Response.Ok(shop);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual bool isSuperAdmin(string password)
        {
            return password == _configuration.GetValue<string>("RootPassword");
        }

    }
}
