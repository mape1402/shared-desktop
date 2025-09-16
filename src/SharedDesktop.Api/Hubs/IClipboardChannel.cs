namespace SharedDesktop.Api.Hubs
{
    public interface IClipboardChannel
    {
        Task ReceiveClipboardAsync(string content);
    }
}
