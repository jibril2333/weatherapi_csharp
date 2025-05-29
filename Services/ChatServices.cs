using System.Net.Http;
using System.Text;
using System.Text.Json;
using restapi_c.Interfaces;
using restapi_c.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace restapi_c.Services;

public class ChatService : IChatService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger<ChatService> _logger;

    public ChatService(
        IHttpClientFactory httpClientFactory,
        IOptions<ApiSettings> apiSettings,
        ILogger<ChatService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _apiSettings = apiSettings.Value;
        _logger = logger;
        
        // 记录配置信息
        _logger.LogInformation("API Key 长度: {Length}", _apiSettings.ApiKey?.Length ?? 0);
        _logger.LogInformation("Model: {Model}", _apiSettings.Model);
    }

    public async Task<string> GetChatResponseAsync(string userInput)
    {
        try
        {
            _logger.LogInformation("开始处理用户输入: {UserInput}", userInput);
            
            using var client = _httpClientFactory.CreateClient();
            var authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiSettings.ApiKey);
            client.DefaultRequestHeaders.Authorization = authHeader;
            
            _logger.LogDebug("Authorization Header: {Header}", authHeader.ToString());

            var requestBody = new
            {
                model = _apiSettings.Model,
                messages = new[]
                {
                    new { role = "user", content = userInput }
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            _logger.LogDebug("请求体: {RequestBody}", jsonContent);

            var content = new StringContent(
                jsonContent,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API 调用失败: {StatusCode}, {ErrorContent}", 
                    response.StatusCode, errorContent);
            }

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<JsonElement>(responseBody);

            return responseObject
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "无法获取响应";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "调用 OpenAI API 时发生错误: {Message}", ex.Message);
            throw;
        }
    }
}
