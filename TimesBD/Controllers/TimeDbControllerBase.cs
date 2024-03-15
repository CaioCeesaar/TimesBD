

using Microsoft.AspNetCore.Mvc;

using TimesBD.Business;

namespace TimesBD.Controllers
{
    public class TimeDbControllerBase : ControllerBase
    {
        protected BusinessClass _businessClass;

        protected IActionResult ConvertResultToHttpResult(Result result)
        {
            if (result.Sucess)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
