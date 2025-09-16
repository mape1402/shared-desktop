
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SharedDesktop.Client.Services.Discovery
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        public async Task<string> GetRemoteServiceUrlAsync()
        {
            using var udp = new UdpClient();
            var data = Encoding.UTF8.GetBytes("DISCOVER_SHARED_DESKTOP");
            await udp.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            var result = await udp.ReceiveAsync();
            var addresses = Encoding.UTF8.GetString(result.Buffer).Split(",");

            foreach (var address in addresses)
            {
                if(await PingAsync(address))
                    return address;
            }

            return null;
        }

        private async Task<bool> PingAsync(string address)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                var response = await httpClient.GetAsync($"{address}/ping");
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
