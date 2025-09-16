
using Microsoft.AspNetCore.SignalR.Client;

namespace SharedDesktop.Client.Services.RealTime
{
    public class SignalRService : ISignalRService
    {
        private readonly HubConnection _hubConnection;
        private Dictionary<string, Func<string, Task>> _clipboardReceivedActions = new();

        public SignalRService(HubConnection hubConnection)
        {
            _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));
        }

        public async Task StartAsync()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
                return;

            _hubConnection.On<string>("ReceiveClipboardAsync", async (content) =>
            {
                foreach (var action in _clipboardReceivedActions.Values)
                {
                    await action(content);
                }
            });

            await _hubConnection.StartAsync();
        }

        public async Task SendClipboardAsync(string content)
        {
            if (_hubConnection.State != HubConnectionState.Connected)
                return;

            await _hubConnection.InvokeAsync("SendClipboardAsync", content);
        }

        public void SubscribeOnClipboardReceived(string key, Func<string, Task> action)
            => _clipboardReceivedActions.TryAdd(key, action);

        public void UnsubscribeOnClipboardReceived(string key)
            => _clipboardReceivedActions.Remove(key);
    }
}
