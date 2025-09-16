namespace SharedDesktop.Api.Services
{
    public interface INetworkService
    {
        IEnumerable<string> GetLocalIPv4();
    }
}
