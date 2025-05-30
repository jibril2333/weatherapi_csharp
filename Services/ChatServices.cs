using System.Net.Http;
using System.Text;
using System.Text.Json;
using restapi_c.Interfaces;
using restapi_c.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace restapi_c.Services;

public class ChatService(
    IHttpClientFactory httpClientFactory,
    IOptions<ApiSettings> apiSettings,
    ILogger<ChatService> logger) : IChatService
{
    private readonly ApiSettings _apiSettings = apiSettings.Value;

    public async Task<string> GetChatResponseAsync(string userInput)
    {
        try
        {
            using var client = httpClientFactory.CreateClient();
            
            if (string.IsNullOrEmpty(_apiSettings.ApiKey))
            {
                throw new InvalidOperationException("API Key 未设置");
            }

            logger.LogInformation("使用 API Key 长度: {Length}", _apiSettings.ApiKey.Length);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiSettings.ApiKey);

            var requestBody = new
            {
                model = _apiSettings.Model,
                messages = new[]
                {
                    new { role = "user", content = userInput }
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            logger.LogDebug("请求体: {RequestBody}", jsonContent);

            var response = await client.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(jsonContent, Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                logger.LogError("API 调用失败: {StatusCode}, {ErrorContent}", response.StatusCode, errorContent);
                throw new HttpRequestException($"API 调用失败: {response.StatusCode}, {errorContent}");
            }

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
            logger.LogError(ex, "调用 OpenAI API 时发生错误: {Message}", ex.Message);
            throw;
        }
    }
}
