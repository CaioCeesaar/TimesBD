using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Framework;

namespace TimesBD.Controllers;

public class LogController : TimeDbControllerBase2
{
    public LogController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection");
    }
    
    [HttpPost]
    public async Task<Result> PostLog([FromBody] LogEntry logEntry)
    {
        await _backgroundService.InserirLogAsync(logEntry.Action, logEntry.Message, logEntry.Details);
        return new Result(true, "Log inserido com sucesso!");
    }
}