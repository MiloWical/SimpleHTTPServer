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
        public void Serve()
        {
            var server = new HttpListener();
            server.Prefixes.Add("http://" + Environment.MachineName + "/");
            server.Prefixes.Add("http://localhost/");

            AddLocalIpAddresses(server.Prefixes);

            while (true)
            {
                Console.WriteLine("Server Listening...\n");

                server.Start();

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
        }

        private static void AddLocalIpAddresses(HttpListenerPrefixCollection prefixes)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
                prefixes.Add("http://" + ip + "/");
        }

        protected abstract string ProcessRequest(HttpListenerRequest httpListenerRequest);
    }
}