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
        private const int Port = 80;

        public void Serve()
        {
            var server = new HttpListener();
            server.Prefixes.Add($"http://*:{Port}/");
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

                    var responseString = ProcessRequest(context.Request);

                    Console.WriteLine("\n\nSending Response:\n-----------------\n" + responseString + "\n");

                    var response = context.Response;

                    var buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
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

        private static void AddLocalIpAddresses(HttpListenerPrefixCollection prefixes)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
                prefixes.Add($"http://{ip}:{Port}/");
        }

        protected abstract string ProcessRequest(HttpListenerRequest httpListenerRequest);
    }
}