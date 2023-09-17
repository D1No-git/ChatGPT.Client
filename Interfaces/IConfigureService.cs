using Microsoft.Extensions.DependencyInjection;

namespace ChatGPT.Client.Interfaces
{
    public interface IConfigureService
    {
        IServiceCollection ConfigureServices();
    }
}
