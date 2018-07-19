using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleHttpServer.CLI
{
    public class CLI
    {
        public static void Main(string[] args)
        {
            var server = new StaticServer();

            server.Serve();
        }
    }
}
