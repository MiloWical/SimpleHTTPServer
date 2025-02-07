namespace SimpleHttpServer.Base
{
    #region Usings

    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    #endregion

    public abstract class HttpServerBase
    {
        public const int DefaultPort = 80;

        private int _port;

        protected HttpServerBase() : this(DefaultPort)
        { }

        protected HttpServerBase(int port)
        {
            if (port < 0)
                throw new ArgumentException("Port cannot be less than 1.", nameof(port));

            _port = port;
        }

        public void Serve()
        {
            var server = new HttpListener();
            server.Prefixes.Add($"http://*:{_port}/");
            //server.Prefixes.Add($"http://{Environment.MachineName}:{Port}/");
            //server.Prefixes.Add($"http://localhost:{Port}/");
            //AddLocalIpAddresses(server.Prefixes);

            Console.WriteLine("Prefixes:");

            foreach (var prefix in server.Prefixes)
            {
                Console.WriteLine(prefix);
            }

            while (true)
            {
                Console.WriteLine("\nServer Listening...\n");

                server.Start();
                try
                {
                    var context = server.GetContext();

                    var processRequestResponse = ProcessRequest(context.Request);

                    Console.WriteLine("\n\nSending Response:\n-----------------\n" + processRequestResponse.Body + "\n");

                    var response = context.Response;

                    var buffer = Encoding.UTF8.GetBytes(processRequestResponse.Body);
                    response.ContentLength64 = buffer.Length;
                    response.StatusCode = (int)processRequestResponse.StatusCode;
                    var output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                
            }
        }

        private void AddLocalIpAddresses(HttpListenerPrefixCollection prefixes)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
                prefixes.Add($"http://{ip}:{_port}/");
        }

        protected static void WriteToConsole(HttpListenerRequest httpListenerRequest)
        {
            Console.WriteLine("Request:\n--------");

            using (var inStreamReader = new StreamReader(httpListenerRequest.InputStream))
            {
                Console.WriteLine(httpListenerRequest.HttpMethod + " " + httpListenerRequest.RawUrl);

                foreach (var header in httpListenerRequest.Headers.AllKeys)
                {
                    var values = httpListenerRequest.Headers.GetValues(header);

                    if (values == null || values.Length <= 0) continue;

                    Console.Write(header + ": ");

                    for (var i = 0; i < values.Length - 1; i++) Console.Write(values[i] + ";");

                    Console.Write(values[values.Length - 1]);

                    Console.WriteLine();
                }

                Console.WriteLine("\n");
                Console.WriteLine(inStreamReader.ReadToEnd());
            }
        }

        protected abstract ProcessRequestResponse ProcessRequest(HttpListenerRequest httpListenerRequest);
    }
}