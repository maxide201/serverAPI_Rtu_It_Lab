using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("{id}")]
        public JsonResult GetUser(uint id)
        {
            User user = db.Get(id);

            if (user == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(user));

        }
  
        [HttpPost]
        public JsonResult PostUser(User user)
        {
            if (isUserInvalid(user))
                return new JsonResult(BadRequest());

            user = db.Add(user);

            return new JsonResult(Ok(user));
        }

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
        public bool isUserInvalid(User user, bool idTestFlag = false)
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
