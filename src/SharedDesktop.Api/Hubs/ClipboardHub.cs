using Microsoft.AspNetCore.SignalR;

namespace SharedDesktop.Api.Hubs
{
    public class ClipboardHub : Hub<IClipboardChannel>
    {
        public Task SendAsync(string content)
        {
            return Clients.Others.ReceiveClipboardAsync(content);
        }
    }
}
