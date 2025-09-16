namespace SharedDesktop.Client.Services.RealTime
{
    public interface ISignalRService
    {
        public Task StartAsync();

        public Task SendClipboardAsync(string content);

        public void SubscribeOnClipboardReceived(string key, Func<string, Task> action);

        public void UnsubscribeOnClipboardReceived(string key);
    }
}
