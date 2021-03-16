using Microsoft.AspNetCore.Mvc;
using ShopApp.Database;
using ShopApp.Models;
using System.Collections.Generic;

namespace ShopApp.Controllers
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

        /// <summary>
        /// Return products of the shop.
        /// </summary>
        /// <response code="200">Return list of products</response>
        [HttpGet("{shopId}")]
        public JsonResult GetProducts(uint shopId)
        {
            List<ProductDTO> products = _db.GetProducts(shopId);

            return new JsonResult(Ok(products));
        }

        /// <summary>
        /// Add new product in shop(for shop's admin).
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Products
        ///     {
        ///         "password":"1",
        ///         "product": {
        ///             "shopId":1,
        ///             "Name":"water", 
        ///             "Category":"food", 
        ///             "Cost":10,
        ///             "Count":10
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return new product</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="403">If password incorrect</response>
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

        /// <summary>
        /// Update information about product in shop(for shop's admin).
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Products
        ///     {
        ///         "password":"1",
        ///         "product": {
        ///             "id":1,
        ///             "shopId":1,
        ///             "Name":"water", 
        ///             "Category":"food", 
        ///             "Cost":10,
        ///             "Count":10
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return updated product</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="403">If password incorrect</response>
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

        /// <summary>
        /// Delete product in shop(for shop's admin).
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/Products
        ///     {
        ///         "password":"1",
        ///         "product": {
        ///             "id":1,
        ///             "shopId":1
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return deleted product</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="403">If password incorrect</response>
        /// <response code="404">If product doesen't exist</response>
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

            if (product.ShopId != request.Product.ShopId)
                return new JsonResult(StatusCode(403));

            _db.DeleteProduct(product.Id);

            return new JsonResult(Ok(product));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
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
