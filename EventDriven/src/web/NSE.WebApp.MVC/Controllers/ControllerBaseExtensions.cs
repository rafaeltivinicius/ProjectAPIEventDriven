using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Sockets;

namespace NSE.WebApp.MVC.Controllers
{
    public static class ControllerBaseExtensions
    {
        public static string ObterIPV2(this ControllerBase controller)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string ObterPageUrl(this ControllerBase controller)
        {
            return controller.HttpContext.Request.Host.Value
                 + controller.HttpContext.Request.Path.Value;
        }
    }
}
