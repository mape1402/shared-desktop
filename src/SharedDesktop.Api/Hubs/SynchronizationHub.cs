using Microsoft.AspNetCore.SignalR;

namespace SharedDesktop.Api.Hubs
{
    public class SynchronizationHub : Hub<ISynchronizationChannel>
    {
        public Task SendClipboardAsync(string content)
        {
            return Clients.Others.ReceiveClipboardAsync(content);
        }
    }
}
