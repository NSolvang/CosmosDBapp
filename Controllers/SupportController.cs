
using Microsoft.AspNetCore.Mvc;
using CosmosDBapp.Model;
using CosmosDBapp.Service;

namespace CosmosDBapp.Controllers;

[ApiController]
[Route("api/support")]
public class SupportController : ControllerBase
{
    private readonly SupportService _supportService;

    public SupportController(SupportService supportService)
    {
        _supportService = supportService;
    }

    [HttpPost]
    public async Task<IActionResult> AddSupportMessage([FromBody] SupportMessage message)
    {
        await _supportService.AddSupportMessageAsync(message);
        return Ok("Support message added!");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupportMessage(string id)
    {
        var message = await _supportService.GetSupportMessageAsync(id);
        return message != null ? Ok(message) : NotFound();
    }
}
