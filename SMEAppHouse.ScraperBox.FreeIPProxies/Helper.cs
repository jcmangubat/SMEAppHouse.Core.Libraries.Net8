using SMEAppHouse.ScraperBox.Common.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using SMEAppHouse.ScraperBox.Common;

namespace SMEAppHouse.ScraperBox.FreeIPProxies
{
    public static class Helper
    {
        public static bool ValidateProxy(string host, int port)
        {
            try
            {
                var wc = new WebClient { Proxy = new WebProxy(host, port) };
                wc.DownloadString("http://google.com/ncr");
                return true;
            }
            catch (Exception ex)
            {
                // ignored
            }
            return false;
        }

        public static bool InternetIsGood()
        {
            try
            {
                var wc = new WebClient();
                wc.DownloadString("http://google.com/ncr");
                return true;
            }
            catch
            {
                // ignored
            }
            return false;
        }

        /// <summary>
        /// Test if proxy is working or not by pinging directly on the host
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool ValidateProxyByPing(string host, int port)
        {
            var address = $"{host}:{port}";
            return ValidateProxyByPing(address);

        }

        /// <summary>
        /// Test if proxy is working or not by pinging directly in the host
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool ValidateProxyByPing(string address)
        {
            var ping = new Ping();
            try
            {
                var reply = ping.Send(address, 2000);
                if (reply == null) return false;
                return (reply.Status == IPStatus.Success);
            }
            catch (PingException e)
            {
                return false;
            }
        }

        public static bool CanPing(string address)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send(address, 2000);
                if (reply == null) return false;

                return (reply.Status == IPStatus.Success);
            }
            catch (PingException e)
            {
                return false;
            }
        }


        /// <summary>
        /// Test if socket proxy is working or not
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool ValidateSocket(string host, int port)
        {
            var isSuccess = false;
            try
            {
                var connsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                connsock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 200);
                Thread.Sleep(500);
                var hip = IPAddress.Parse(host);
                var ipep = new IPEndPoint(hip, port);
                connsock.Connect(ipep);
                isSuccess = connsock.Connected;
                connsock.Close();
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public static bool TestIPProxy2(IPProxy proxy)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                proxy.CheckStatus = Rules.ProxyCheckerStatusEnum.Checking;
                var content = Common.Helper.GetPage("http://example.com", proxy);
                sw.Stop();
                proxy.SpeedRate = (int)sw.ElapsedMilliseconds;
                proxy.CheckStatus = Rules.ProxyCheckerStatusEnum.Checked;
            }
            catch (Exception e)
            {
                proxy.CheckStatus = Rules.ProxyCheckerStatusEnum.CheckedInvalid;
            }
            return proxy.CheckStatus == Rules.ProxyCheckerStatusEnum.Checked;
        }

        public static bool TestIPProxy(IPProxy proxy)
        {
            var protHt = proxy.Protocol == Rules.ProxyWebProtocolsEnum.HTTPS ? "https://" : proxy.Protocol == Rules.ProxyWebProtocolsEnum.HTTP ? "http://" : "";
            var prxy = new WebProxy()
            {
                Address = new Uri($"{protHt}{proxy.IPAddress}:{proxy.PortNo}"),
                Credentials = CredentialCache.DefaultCredentials,

                //still use the proxy for local addresses
                BypassProxyOnLocal = false,

            };

            if (proxy.Credential != null)
            {
                prxy.UseDefaultCredentials = false;
                // *** These credentials are given to the proxy server, not the web server ***
                prxy.Credentials = proxy.GetNetworkCredential();
            }

            var sw = Stopwatch.StartNew();

            // Finally, create the HTTP client object to test the proxy
            var request = (HttpWebRequest)WebRequest.Create("http://example.com/");
            request.Proxy = prxy;

            try
            {
                proxy.CheckStatus = Rules.ProxyCheckerStatusEnum.Checking;
                using (var req = request.GetResponse())
                {
                    var webResponse = req;
                    using (var reader = new StreamReader(webResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        var response = reader.ReadToEnd();
                        proxy.CheckStatus = Rules.ProxyCheckerStatusEnum.Checked;
                        proxy.SpeedRate = (int)sw.ElapsedMilliseconds;
                    }
                }
            }
            catch (Exception e)
            {
                proxy.CheckStatus = Rules.ProxyCheckerStatusEnum.CheckedInvalid;
                proxy.SpeedRate = 0;
            }
            finally
            {
                sw.Stop();
                proxy.LastChecked = DateTime.Now;
            }

            return proxy.CheckStatus == Rules.ProxyCheckerStatusEnum.Checked;
        }

    }
}
