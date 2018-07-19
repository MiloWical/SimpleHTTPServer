namespace SimpleHttpServer.Static
{
    using System;
    using System.IO;
    using System.Net;
    using Base;

    public class StaticServer : HttpServerBase
    {
        private const string ResponseString = "<HTML><BODY>Transaction Successful.</BODY></HTML>";

        protected override string ProcessRequest(HttpListenerRequest httpListenerRequest)
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

            return ResponseString;
        }
    }
}
