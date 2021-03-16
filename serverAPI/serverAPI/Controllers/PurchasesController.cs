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

        [HttpPost]
        public JsonResult MakePurchase(PurchaseRequest request)
        {
            if (!isPurchaseRequestRight(request))
                return new JsonResult(BadRequest());

            if (!_db.isUserAndShopExhist(request))
                return new JsonResult(BadRequest());

            CheckDTO check = _db.MakePurchase(request);
            if (check == null)
                return new JsonResult(StatusCode(409));

            return new JsonResult(Ok(check));
        }


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

        private bool isShopRequestValid(ShopAdminRequest request)
        {
            if (request == null ||
                request.Password == null)
                return false;

            return true;
        }

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
