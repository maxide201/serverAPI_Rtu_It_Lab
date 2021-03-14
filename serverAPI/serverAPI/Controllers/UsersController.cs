using Microsoft.AspNetCore.Mvc;
using serverAPI.Models;
using serverAPI.Database;

namespace serverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRepository db;

        public UsersController(IUserRepository db)
        {
            this.db = db;
        }

        /// <summary>
        /// Return user by id.
        /// </summary>
        /// <response code="200">Returns user</response>
        /// <response code="404">If the user doesn't exist</response>
        [HttpGet("{id}")]
        public JsonResult GetUser(uint id)
        {
            User user = db.Get(id);

            if (user == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(user));

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
            if (isUserInvalid(user))
                return new JsonResult(BadRequest());

            user = db.Add(user);

            return new JsonResult(Ok(user));
        }

        /// <summary>
        /// Update purchases list of existing user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Users
        ///     {
        ///        "id": 1
        ///        "purchases": [
        ///             {
        ///                 "id": 1,
        ///                 "userId": 1,
        ///                 "name": "apple",
        ///                 "purchaseDate": "2000-01-01T01:01:01",
        ///                 "cost: 1000
        ///             },
        ///             {
        ///                 "name": "banana",
        ///                 "purchaseDate": "2000-01-01T01:01:02",
        ///                 "cost: 200
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Updated user</returns>
        /// <response code="200">Returns updated user</response>
        /// <response code="400">If the user is null or invalid</response>
        /// <response code="404">If the user doesn't exist</response>
        [HttpPut]
        public JsonResult PutUser(User user)
        {
            if (isUserInvalid(user, idTestFlag: true))
                return new JsonResult(BadRequest());
            if (db.Get(user.Id) == null)
                return new JsonResult(NotFound());

            user = db.Update(user);
            return new JsonResult(Ok(user));
        }

        /// <summary>
        /// Delete user by id.
        /// </summary>
        /// <response code="200">Returns deleted user</response>
        /// <response code="404">If the user doesn't exist</response>
        [HttpDelete("{id}")]
        public JsonResult DeleteUser(uint id)
        {
            User user = db.Get(id);

            if (user == null)
                return new JsonResult(NotFound());

            db.Delete(id);
            return new JsonResult(Ok(user));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual bool isUserInvalid(User user, bool idTestFlag = false)
        {
            if (user == null || user.Purchases == null)
                return true;
            if (idTestFlag && user.Id == 0)
                return true;
            foreach (var purchase in user.Purchases)
            {
                if (purchase == null)
                    return true;
                if (purchase.Name == null ||
                    purchase.PurchaseDate == System.DateTime.MinValue)
                    return true;
            }
            return false;
        }
    }
}