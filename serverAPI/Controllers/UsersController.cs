using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using serverAPI.Models;

namespace serverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public JsonResult GetUsers()
        {
            return new JsonResult(new User() {Id = 1, Purchases = new List<Purchase>() { new Purchase() { Id = 1, UserId = 1, Cost = 100, Name = "aaa", PurchaseDate = DateTime.Now} } });
        }
    }
}
