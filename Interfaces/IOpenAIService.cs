namespace ChatGPT.Client.Interfaces
{
    public interface IOpenAIService
    {
        Task<string?> GetGPTResponseAsync(string prompt);
    }
}
