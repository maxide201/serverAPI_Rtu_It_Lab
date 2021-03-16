using Microsoft.AspNetCore.Mvc;
using serverAPI.Models;
using serverAPI.Database;
using System;
using System.Collections.Generic;

namespace serverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRepository _db;

        public UsersController(IUserRepository db)
        {
            _db = db;
        }

        /// <summary>
        /// Return user by id.
        /// </summary>
        /// <response code="200">Returns user</response>
        /// <response code="404">If the user doesn't exist</response>
        [HttpGet]
        public JsonResult GetPurchases(User user)
        {
            if (!isUserValid(user))
                return new JsonResult(BadRequest());

            if (!_db.isUserPasswordRight(user))
                return new JsonResult(StatusCode(403));

            List<PurchaseDTO> purchases = _db.GetPurchases(user.Id);

            return new JsonResult(Ok(purchases));

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
        public JsonResult PostUser(User user)
        {
            if (!isUserValid(user))
                return new JsonResult(BadRequest());

            user = _db.Add(user);

            return new JsonResult(Ok(user));
        }

        /// <summary>
        /// Delete user by id.
        /// </summary>
        /// <response code="200">Returns deleted user</response>
        /// <response code="404">If the user doesn't exist</response>
        [HttpDelete]
        public JsonResult DeleteUser(User user)
        {
            if (!isUserValid(user, idTestFlag:true))
                return new JsonResult(BadRequest());

            if (!_db.isUserPasswordRight(user))
                return new JsonResult(StatusCode(403));

            _db.Delete(user.Id);
            return new JsonResult(Ok(user));
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual bool isUserValid(User user, bool idTestFlag = false)
        {
            if (user == null ||
                user.Name == null||
                user.Password == null)
                return false;

            if (idTestFlag && user.Id == 0)
                return false;

            return true;
        }
    }
}