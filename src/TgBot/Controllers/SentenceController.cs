using DropWord.TgBot.Model;
using Microsoft.AspNetCore.Mvc;

namespace DropWord.TgBot.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SentenceController : ControllerBase
{
    
    [HttpPost("[action]")]
    public async Task RepeatForDay([FromBody] RepeatForDayApiModel model)
    {
        var testPoint = model;
        
        await Task.CompletedTask;
        NoContent();
    }
}
