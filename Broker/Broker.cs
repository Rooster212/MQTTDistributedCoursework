using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Http;
using System.Web.Http.Routing;
using BrokerApp.MQTTWcfService;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using MQTTProtocol;
using Timer = System.Timers.Timer;

namespace BrokerApp
{
    public class Broker : MQTTBase, IBroker
    {
        private const string User = "201102762";
        private int SignalRHostPort = 5001;
        private static bool _registered = false;
        private static DirectoryServiceClient _directoryServiceClientRef;
        private static string _token = string.Empty;
        private static bool _runBroker = false;
        private static Timer _pollTimer;
        private static BrokerConnections _connectionObj;

        private TcpListener listener;
        private Thread listenThread;

        public static void Main(string[] args)
        {
            _directoryServiceClientRef = new DirectoryServiceClient();
            //if (_directoryServiceClientRef.ClientCredentials != null)
            //{
            //    _directoryServiceClientRef.ClientCredentials.Windows.ClientCredential.UserName = "423048";
            //    _directoryServiceClientRef.ClientCredentials.Windows.ClientCredential.Password = "<redacted>";
            //    _directoryServiceClientRef.ClientCredentials.Windows.ClientCredential.Domain = "ADIR";
            //}

            // setup timer to poll server
            _pollTimer = new Timer();
            _pollTimer.Elapsed += PollServer;
            _pollTimer.Interval = PING_INTERVAL_MS;
            _pollTimer.Enabled = true;
            _runBroker = true;

            _connectionObj = new BrokerConnections();
            _connectionObj.StartServer();

            // setup web server
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string localIp = string.Empty;
            foreach (IPAddress ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ipAddress.ToString();
                    break;
                }
            }


            var signalrHostPort = int.Parse(ConfigurationManager.AppSettings["signalrHostPort"]);

            Console.WriteLine("This Broker IP: {0}", localIp);

            GlobalHost.Configuration.DisconnectTimeout = new TimeSpan(0, 0, 60);
            GlobalHost.Configuration.ConnectionTimeout = new TimeSpan(0, 0, 20);
            GlobalHost.Configuration.KeepAlive = new TimeSpan(0, 0, 5);

            bool webInterfaceRunning = false;
            IDisposable webAppHost;
            try
            {
                string urlAdminOnly = string.Format("http://+:{0}", signalrHostPort);
                webAppHost = WebApp.Start<Startup>(urlAdminOnly);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Broker SignalR running on {0}", urlAdminOnly);
                webInterfaceRunning = true;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Error occurred running web interface for all IP addresses (You probably need admin rights). Reverting to localhost only");
                Console.ResetColor();
            }

            if (!webInterfaceRunning)
            {
                string url = string.Format("http://localhost:{0}", signalrHostPort);
                webAppHost = WebApp.Start<Startup>(url);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Broker SignalR running on {0}", url);
            }
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            string navigateUrl = "http://" +(webInterfaceRunning ? localIp : "localhost") +":"+ signalrHostPort;

            Console.WriteLine("You can navigate to the broker manager by going to {0}", navigateUrl);
            Console.ResetColor();
            // perform initial register
            Process.Start(navigateUrl);

            RegisterOrPollServer();
            Console.WriteLine("--------------------------------------------------------------");
        }

        private static void PollServer(object source, ElapsedEventArgs e)
        {
            RegisterOrPollServer();
        }

        private static void RegisterOrPollServer()
        {
            if (_registered == false)
            {
                _token = _directoryServiceClientRef.RegisterServer(User);
                Console.WriteLine("Server connected " + DateTime.Now.ToLongTimeString());
                if (!string.IsNullOrEmpty(_token))
                {
                    _registered = true;
                }
            }
            else
            {
                _directoryServiceClientRef.RegisterServer(_token);
                Console.WriteLine("Poll performed " + DateTime.Now.ToLongTimeString());
            }
        }

        private static void Unregister()
        {
            _directoryServiceClientRef.UnregisterServer(_token);
            _registered = false;
        }

        private static string[] GetServers()
        {
            return _registered ? _directoryServiceClientRef.GetServers() : null; // returns the list if we are registered, otherwise null
        }

        public static void ShutDownBroker()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Shutdown has been initiated");
            Console.ResetColor();

            // notify broker manager(s)
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BrokerHub>();
            hubContext.Clients.All.brokerDisconnecting();

            (new Thread(_connectionObj.Shutdown)).Start();
            _pollTimer.Enabled = false;
            Unregister();
            _runBroker = false;
        }

        public List<MQTTClient> GetClients()
        {
            throw new NotImplementedException();
        }

        public bool TestCommunication()
        {
            return true;
        }

        string[] IBroker.GetServers()
        {
            return GetServers();
        }
    }
}
