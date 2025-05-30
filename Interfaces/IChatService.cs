namespace restapi_c.Interfaces;

public interface IChatService
{
    Task<string> GetChatResponseAsync(string userInput);
}