using ChatGPT.Client.Interfaces;
using ChatGPT.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

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
        Console.Clear();
        AnsiConsole.Write(
            new FigletText("ChatGPT")
                .Centered()
                .Color(Color.DarkCyan)
        );
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[bold white]Please ask a question or type 'exit' to quit.[/]");

        AnsiConsole.WriteLine();
    }

    private static async Task InteractWithUserAsync(IOpenAIService service)
    {
        while (true)
        {
            string? prompt = PromptUserForInput();

            if (prompt?.ToLower() == "exit")
                break;

            if (!string.IsNullOrWhiteSpace(prompt))
            {
                await DisplayGptResponseAsync(service, prompt);
            }
        }
    }

    private static string? PromptUserForInput()
    {
        return AnsiConsole.Ask<string?>("[bold magenta]\nAsk:[/]");
    }

    private static async Task DisplayGptResponseAsync(IOpenAIService service, string prompt)
    {
        try
        {
            AnsiConsole.MarkupLine("[bold darkcyan]ChatGPT is thinking...[/]");

            string? response = await service.GetGPTResponseAsync(prompt);

            AnsiConsole.WriteLine();

            if (response != null)
            {
                string[] words = response.Split(' ');
                AnsiConsole.Markup("[bold darkcyan]ChatGPT:[/] ");
                foreach (var word in words)
                {
                    AnsiConsole.Markup(word + " ");
                    await Task.Delay(30); // Adjust the delay to fit your preference
                }
                AnsiConsole.WriteLine();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[bold red]An error occurred:[/] " + ex.Message);
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