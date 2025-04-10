using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To_do_part_3;

public interface IINetworkHelper
{
    bool HasInternet();
    Task<bool> IsHostReachable(string host);
 }
  
