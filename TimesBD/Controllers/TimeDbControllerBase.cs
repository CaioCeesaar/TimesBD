using Microsoft.AspNetCore.Mvc;

using TimesBD.Business;

namespace TimesBD.Controllers
{
    public class TimeDbControllerBase : ControllerBase
    {
        public IActionResult ConvertResultToHttpResult(Result result)
        {
            if (result.Sucess)
            {
                return Ok(result.Message);
            }
            
            return BadRequest(result.Message);
        }
    }
}
