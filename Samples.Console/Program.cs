namespace Samples.Console
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using StackExchange.NetGain;
    using StackExchange.NetGain.WebSockets;
    using Console = System.Console;

    internal class Program
    {
        private static void Main(string[] args)
        {
            /* Client
            	using (var client = new TcpClient())
	            {
		            client.ProtocolFactory = WebSocketClientFactory.Default;
		            client.ConnectTimeout = 15000;
		            client.WaitTimeout = 15000;
		            client.Open(new IPEndPoint(IPAddress.Loopback, 6002));
		            var resp = (string)client.ExecuteSync("abcdefg");
		            resp.Dump();	
	            }
             */

            using(var server = new TcpServer())
            {
                server.ProtocolFactory = CustomSelectorProcessor.Instance;
                server.MessageProcessor = CustomMessageProcessor.Instance;
                server.ConnectionTimeoutSeconds = 60;
                server.Received += msg =>
                {
                    var conn = (WebSocketConnection)msg.Connection;
                    var reply = (string)msg.Value + " / " + conn.Host;
                    Console.WriteLine("[server] {0}", msg.Value);
                    msg.Connection.Send(msg.Context, reply);
                };

                server.Start("abc", new IPEndPoint(IPAddress.Loopback, 6002));
                Console.WriteLine("Server running");

                Console.ReadKey();
            }
            Console.WriteLine("Server dead; press any key");
            Console.ReadKey();
        }
    }

    internal sealed class CustomSelectorProcessor: WebSocketsSelectorProcessor
    {
        private static readonly Regex GetRequestRegex = new Regex("GET (.*) HTTP/1.[01]", RegexOptions.Compiled);

        public static CustomSelectorProcessor Instance { get; } = new CustomSelectorProcessor();

        private CustomSelectorProcessor()
        {
        }

        protected override bool TryBasicResponse(NetContext context, StringDictionary requestHeaders, string requestLine, System.Collections.Specialized.StringDictionary responseHeaders, out HttpStatusCode code, out string body)
        {
            var match = GetRequestRegex.Match(requestLine);

            if (match.Success && Uri.TryCreate(match.Groups[1].Value.Trim(), UriKind.RelativeOrAbsolute, out Uri uri))
            {
                switch (uri.OriginalString)
                {
                    case "/ping":
                        code = HttpStatusCode.OK;
                        body = "Ping response from custom factory";
                        return true;
                }
            }
            return base.TryBasicResponse(context, requestHeaders, requestLine, responseHeaders, out code, out body);
        }

        protected override void GracefulShutdown(NetContext context, Connection connection)
        {
            base.GracefulShutdown(context, connection);
        }

        protected override void InitializeClientHandshake(NetContext context, Connection connection)
        {
            base.InitializeClientHandshake(context, connection);
        }

        protected override void InitializeInbound(NetContext context, Connection connection)
        {
            base.InitializeInbound(context, connection);
        }

        protected override void InitializeOutbound(NetContext context, Connection connection)
        {
            base.InitializeOutbound(context, connection);
        }

        protected override int ProcessHeadersAndUpgrade(NetContext context, Connection conn, Stream input, Stream additionalData, int additionalOffset)
        {
            return base.ProcessHeadersAndUpgrade(context, conn, input, additionalData, additionalOffset);
        }

        protected override int ProcessIncoming(NetContext context, Connection connection, Stream incomingBuffer)
        {
            return base.ProcessIncoming(context, connection, incomingBuffer);
        }

        protected override void Send(NetContext context, Connection connection, object message)
        {
            base.Send(context, connection, message);
        }
    }

    internal sealed class CustomMessageProcessor : WebSocketsMessageProcessor
    {
        public static CustomMessageProcessor Instance { get; } = new CustomMessageProcessor();

        private CustomMessageProcessor()
        {
        }
        
        protected override void OnConfigure(TcpService service)
        {
            base.OnConfigure(service);
        }

        protected override void OnStartup(string configuration)
        {
            base.OnStartup(configuration);
        }

        protected override void OnOpened(WebSocketConnection connection)
        {
            base.OnOpened(connection);
        }

        protected override void OnAuthenticate(WebSocketConnection connection, StringDictionary claims)
        {
            base.OnAuthenticate(connection, claims);
        }

        protected override void OnAfterAuthenticate(WebSocketConnection connection)
        {
            base.OnAfterAuthenticate(connection);
        }

        protected override void OnClosed(WebSocketConnection connection)
        {
            base.OnClosed(connection);
        }

        protected override void OnFlushed(WebSocketConnection connection)
        {
            base.OnFlushed(connection);
        }

        protected override void OnHeartbeat()
        {
            base.OnHeartbeat();
        }

        protected override void OnReceive(WebSocketConnection connection, byte[] message)
        {
            base.OnReceive(connection, message);
        }

        protected override void OnReceive(WebSocketConnection connection, string message)
        {
            base.OnReceive(connection, message);
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        protected override void OnShutdown(WebSocketConnection connection)
        {
            base.OnShutdown(connection);
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
        }
    }
}
