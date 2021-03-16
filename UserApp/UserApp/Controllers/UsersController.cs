using Microsoft.AspNetCore.Mvc;
using UserApp.Models;
using UserApp.Database;
using System.Collections.Generic;

namespace UserApp.Controllers
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
        /// Return user's purchases.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Users
        ///     {
        ///        "id": 1,
        ///        "name: "Maxim",
        ///        "password": "secret"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns purchases</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="403">If password incorrect or user doesen't exist</response>
        [HttpGet]
        public JsonResult GetPurchases(User user)
        {
            if (!isUserValid(user))
                return _Response.BadRequest();

            if (!_db.isUserPasswordRight(user))
                return _Response.Forbid();

            List<PurchaseDTO> purchases = _db.GetPurchases(user.Id);

            return _Response.Ok(purchases);

        }
        /// <summary>
        /// Add new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Users
        ///     {
        ///        "name: "Maxim",
        ///        "password": "secret"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return user</response>
        /// <response code="400">If request is invalid</response>
        [HttpPost]
        public JsonResult PostUser(User user)
        {
            if (!isUserValid(user))
                return _Response.BadRequest();

            user = _db.Add(user);

            return _Response.Ok(user);
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/Users
        ///     {
        ///        "id": 1,
        ///        "name: "Maxim",
        ///        "password": "secret"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return deleted user</response>
        /// <response code="400">If request is invalid</response>
        /// <response code="403">If password incorrect or user doesen't exist</response>
        [HttpDelete]
        public JsonResult DeleteUser(User user)
        {
            if (!isUserValid(user, idTestFlag:true))
                return _Response.BadRequest();

            if (!_db.isUserPasswordRight(user))
                return _Response.Forbid();

            _db.Delete(user.Id);
            return _Response.Ok(user);
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