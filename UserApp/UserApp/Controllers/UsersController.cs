using Microsoft.AspNetCore.Mvc;
using ServerModels;
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
                return new JsonResult(BadRequest());

            if (!_db.isUserPasswordRight(user))
                return new JsonResult(StatusCode(403));

            List<PurchaseDTO> purchases = _db.GetPurchases(user.Id);

            return new JsonResult(Ok(purchases));

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
                return new JsonResult(BadRequest());

            user = _db.Add(user);

            return new JsonResult(Ok(user));
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