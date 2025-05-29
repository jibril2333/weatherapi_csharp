using Microsoft.AspNetCore.Mvc;
using restapi_c.Interfaces;
using restapi_c.Models;

namespace restapi_c.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        var resultJson = await _chatService.GetChatResponseAsync(request.Message);
        return Content(resultJson, "application/json");
    }
}