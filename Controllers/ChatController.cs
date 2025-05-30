using Microsoft.AspNetCore.Mvc;
using restapi_c.Interfaces;
using restapi_c.Models;

namespace restapi_c.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(ChatRequest request)
    {
        var response = await chatService.GetChatResponseAsync(request.Message);
        return Ok(new { response });
    }
}