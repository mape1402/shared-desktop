namespace SharedDesktop.Api.Hubs
{
    public interface ISynchronizationChannel
    {
        Task ReceiveClipboardAsync(string content);
    }
}
