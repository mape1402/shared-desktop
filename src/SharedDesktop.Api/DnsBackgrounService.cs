using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using SharedDesktop.Api.Services;
using System.Net.Sockets;
using System.Text;

namespace SharedDesktop.Api
{
    public class DnsBackgrounService : BackgroundService
    {
        private readonly IServer _server;
        private readonly INetworkService _networkService;

        public DnsBackgrounService(IServer server, INetworkService networkService)
        {
            _server = server ?? throw new ArgumentNullException(nameof(server));
            _networkService = networkService ?? throw new ArgumentNullException(nameof(networkService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var udp = new UdpClient(8888);

            while (true)
            {
                var result = await udp.ReceiveAsync();
                var request = Encoding.UTF8.GetString(result.Buffer);

                if (request == "DISCOVER_SHARED_DESKTOP")
                {
                    var port = GetPort();
                    var addresses = _networkService.GetLocalIPv4();
                    var discoveredAddress = string.Join(",", addresses.Select(ip => $"http://{ip}:{port}"));

                    var response = Encoding.UTF8.GetBytes(discoveredAddress);
                    await udp.SendAsync(response, response.Length, result.RemoteEndPoint);
                }
            }
        }

        private string GetPort()
        {
            var addresses = _server.Features.Get<IServerAddressesFeature>()?.Addresses;
            var address = addresses?.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(address))
                return "0";

            return address.Split(":").LastOrDefault() ?? "0";
        }
    }
}
