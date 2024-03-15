using Microsoft.AspNetCore.Mvc;

using TimesBD.Business;
using TimesBD.Framework;

namespace TimesBD.Controllers
{
    public class TimeDbControllerBase2 : ControllerBase
    {
        protected readonly TimesBackgroundService _backgroundService;

        public TimeDbControllerBase2(TimesBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

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