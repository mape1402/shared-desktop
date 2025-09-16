
using Microsoft.AspNetCore.SignalR.Client;
using SharedDesktop.Client.Services.Discovery;

namespace SharedDesktop.Client.Services.RealTime
{
    public class SignalRService : ISignalRService
    {
        private HubConnection _hubConnection;
        private readonly IServiceDiscovery _serviceDiscovery;
        private Dictionary<string, Func<string, Task>> _clipboardReceivedActions = new();

        public SignalRService(IServiceDiscovery serviceDiscovery)
        {
            _serviceDiscovery = serviceDiscovery ?? throw new ArgumentNullException(nameof(serviceDiscovery));
        }

        public async Task StartAsync()
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
                return;

            var remoteUrl = await _serviceDiscovery.GetRemoteServiceUrlAsync();

            if (string.IsNullOrWhiteSpace(remoteUrl))
                return;

            _hubConnection = new HubConnectionBuilder()
                                    .WithUrl($"{remoteUrl}/realtime/synchronization")
                                    .WithAutomaticReconnect()
                                    .Build();

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
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                return;

            await _hubConnection.InvokeAsync("SendClipboardAsync", content);
        }

        public void SubscribeOnClipboardReceived(string key, Func<string, Task> action)
            => _clipboardReceivedActions.TryAdd(key, action);

        public void UnsubscribeOnClipboardReceived(string key)
            => _clipboardReceivedActions.Remove(key);
    }
}
