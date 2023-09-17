using ChatGPT.Client.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ChatGPT.Client.Services
{
    public class OpenAIService : IOpenAIService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(IHttpClientFactory httpClientFactory, ILogger<OpenAIService> loger)
        {
            _httpClient = httpClientFactory.CreateClient("OpenAiClient");
            _logger = loger;
        }

        public async Task<string?> GetGPTResponseAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                throw new ArgumentNullException(nameof(prompt), "Prompt cannot be null or whitespace.");

            var endpoint = "/v1/chat/completions";

            JObject requestBody = new JObject
            {
                ["model"] = "gpt-3.5-turbo", // Specifying the model here
                ["messages"] = new JArray
            {
                new JObject
                {
                    ["role"] = "system",
                    ["content"] = "You are a helpful assistant."
                },
                new JObject
                {
                    ["role"] = "user",
                    ["content"] = prompt
                }
            }
            };

            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError(errorContent);

                throw new Exception($"Failed to retrieve GPT-3 response. Status code: {response.StatusCode}. Response content: {errorContent}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            var jsonResponse = JObject.Parse(responseContent);

            return jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim();
        }

        public void Dispose() => _httpClient.Dispose();
    }
}
