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
    public class ProductsController : ControllerBase
    {
        IProductRepository _db;

        public ProductsController(IProductRepository db)
        {
            _db = db;
        }

        [HttpGet("{shopId}")]
        public JsonResult GetProducts(uint shopId)
        {
            List<ProductDTO> products = _db.GetProducts(shopId);

            return new JsonResult(Ok(products));
        }

        [HttpPost]
        public JsonResult PostProduct(ShopAdminProductRequest request)
        {
            if (!isRequestValid(request))
                return new JsonResult(BadRequest());

            if (!_db.isPasswordRight(request))
                return new JsonResult(StatusCode(403));

            Product product = request.Product;
            product = _db.AddProduct(product);

            return new JsonResult(Ok(product));
        }

        [HttpPut]
        public JsonResult UpdateProduct(ShopAdminProductRequest request)
        {
            if (!isRequestValid(request))
                return new JsonResult(BadRequest());

            if (!_db.isPasswordRight(request))
                return new JsonResult(StatusCode(403));

            Product product = _db.Get(request.Product.Id);
            if (product == null)
                return new JsonResult(NotFound());

            product = _db.UpdateProduct(request.Product);

            return new JsonResult(Ok(product));
        }

        [HttpDelete]
        public JsonResult DeleteProduct(ShopAdminProductRequest request)
        {
            if (!isRequestValid(request, DeleteFlag:true))
                return new JsonResult(BadRequest());

            if (!_db.isPasswordRight(request))
                return new JsonResult(StatusCode(403));

            Product product = _db.Get(request.Product.Id);
            if (product == null)
                return new JsonResult(NotFound());

            _db.DeleteProduct(product.Id);

            return new JsonResult(Ok(product));
        }

        private bool isRequestValid(ShopAdminProductRequest request, bool DeleteFlag = false)
        {
            if (request == null ||
                request.Password == null ||
                request.Product == null ||
                request.Product.ShopId == 0)
                return false;
            if(!DeleteFlag && (
                request.Product.Name == null ||
                request.Product.Category == null))
                return false;
                return true;
        }
    }
}
