using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SharedDesktop.Api.Services
{
    public class NetworkService : INetworkService
    {
        public IEnumerable<string> GetLocalIPv4()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up) 
                    continue;

                if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) 
                    continue;

                var ipProps = ni.GetIPProperties();

                foreach (var addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(addr.Address))
                        yield return addr.Address.ToString();
                }
            }
        }
    }
}
