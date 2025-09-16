namespace SharedDesktop.Client.Services.Discovery
{
    public interface IServiceDiscovery
    {
        Task<string> GetRemoteServiceUrlAsync();
    }
}
