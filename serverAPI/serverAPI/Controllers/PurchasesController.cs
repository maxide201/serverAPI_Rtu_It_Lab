using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class PurchasesController : ControllerBase
    {
        IPurchaseRepository _db;

        public PurchasesController(IPurchaseRepository db)
        {
            _db = db;
        }

        /// <summary>
        /// Make a purchase in shop.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Purchases
        ///     {
        ///         "UserId":12,
        ///         "ShopId":2,
        ///         "PaymentMethod":"Money",
        ///         "Products":[
        ///             {
        ///                 "Id":1,
        ///                 "Count":1
        ///             },
        ///             {
        ///                 "Id":2,
        ///                 "Count":1,
        ///                 "Category":"sport"
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return check for user</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="409">If there was a purchase error. Possible reasons: -the user wants to buy more products than they have -there is no such item in the shop</response> 
        [HttpPost]
        public JsonResult MakePurchase(PurchaseRequest request)
        {
            if (!isPurchaseRequestRight(request))
                return new JsonResult(BadRequest());

            if (!_db.isUserAndShopExists(request))
                return new JsonResult(BadRequest());

            CheckDTO check = _db.MakePurchase(request);
            if (check == null)
                return new JsonResult(StatusCode(409));

            return new JsonResult(Ok(check));
        }

        /// <summary>
        /// Get all checks of the shop(for shop's admin).
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Purchases
        ///     {
        ///         "ShopId":1,
        ///         "Password":"1"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return list of checks</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="403">If password incorrect</response>
        [HttpGet]
        public JsonResult GetChecks(ShopAdminRequest request)
        {

            if (!isShopRequestValid(request))
                return new JsonResult(BadRequest());

            if (!_db.isShopPasswordRight(request))
                return new JsonResult(StatusCode(403));

            List<CheckDTO> purchases = _db.GetChecks(request.ShopId);

            return new JsonResult(Ok(purchases));

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private bool isShopRequestValid(ShopAdminRequest request)
        {
            if (request == null ||
                request.Password == null)
                return false;

            return true;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private bool isPurchaseRequestRight(PurchaseRequest request)
        {
            if (request == null ||
                request.UserId == 0 ||
                request.ShopId == 0 ||
                request.PaymentMethod == null || 
                request.Products == null)
                return false;
            return true;
        }
    }
}
