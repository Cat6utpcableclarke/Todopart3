using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
namespace To_do_part_3;

class NetworkHelper : IINetworkHelper
{
    public static NetworkHelper network;
    public static NetworkHelper GetInstance
    {
        get
        {
            if (network == null)
            {
                network = new NetworkHelper();
            }
            return network;
        }
    }

    public bool HasInternet()
    {
        if (!CrossConnectivity.IsSupported)
        {
            return true;
        }
        return CrossConnectivity.Current.IsConnected;
    }

    public bool HasInternetConnection()
    {
        return HasInternet();
    }

    public async Task<bool> IsHostReachable(string host)
    {
        // Implement logic to check if a host is reachable
        return await Task.FromResult(true); // Placeholder
    }
}