using ChatGPT.Client.Interfaces;
using ChatGPT.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private static IServiceProvider? _serviceProvider;

    static async Task Main(string[] args)
    {
        IConfiguration configuration = BuildConfiguration();

        RegisterServices(configuration);

        IOpenAIService? service = _serviceProvider?.GetService<IOpenAIService>();
        if (service == null)
        {
            Console.WriteLine("Failed to retrieve service.");
            return;
        }

        DisplayGreetingMessage();

        await InteractWithUserAsync(service);

        DisposeServices();
    }

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

    private static void RegisterServices(IConfiguration configuration)
    {
        var collection = new ServiceCollection();
        var configureService = new ConfigureService(configuration);
        _serviceProvider = configureService.ConfigureServices().BuildServiceProvider();
    }

    private static void DisplayGreetingMessage()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("#################### ChatGPT ####################\n");
        Console.ResetColor();
    }

    private static async Task InteractWithUserAsync(IOpenAIService service)
    {
        while (true)
        {
            string? prompt = PromptUserForInput();

            if (prompt?.ToLower() == "exit")
                break;

            await DisplayGptResponseAsync(service, prompt!);
        }
    }

    private static string? PromptUserForInput()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Please enter a question (or type 'exit' to quit):");
        Console.ResetColor();

        return Console.ReadLine();
    }

    private static async Task DisplayGptResponseAsync(IOpenAIService service, string prompt)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n-- ChatGPT response --");
            Console.ResetColor();

            string? response = await service.GetGPTResponseAsync(prompt);

            if (response != null)
            {
                string[] words = response.Split(' ');
                foreach (var word in words)
                {
                    Console.Write(word + " ");
                    await Task.Delay(30); // Adjust the delay to fit your preference
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void DisposeServices()
    {
        if (_serviceProvider == null) return;
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}