using Microsoft.AspNetCore.Mvc;

namespace UserApp
{
    public static class _Response
    {
        public static JsonResult Ok(object o)
        {
            return new JsonResult(o) { StatusCode = 200 };
        }
        public static JsonResult BadRequest()
        {
            return new JsonResult(null) { StatusCode = 400 };
        }
        public static JsonResult Forbid()
        {
            return new JsonResult(null) { StatusCode = 403 };
        }
    }
}
