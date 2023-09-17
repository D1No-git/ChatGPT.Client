using ChatGPT.Client.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatGPT.Client.Services
{
    public class ConfigureService : IConfigureService
    {
        private readonly IConfiguration _configuration;

        public ConfigureService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddHttpClient("OpenAiClient", client =>
            {
                client.BaseAddress = new Uri(_configuration["OpenAI:Endpoint"]!);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["OpenAI:MyApiKey"]}");
            });

            services.AddScoped<IOpenAIService, OpenAIService>();

            return services;
        }
    }
}
