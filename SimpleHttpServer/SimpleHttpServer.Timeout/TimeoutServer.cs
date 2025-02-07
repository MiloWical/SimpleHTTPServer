namespace SimpleHttpServer.Timeout
{
    using System;
    using System.Net;
    using System.Threading;
    using Base;

    public class TimeoutServer : HttpServerBase
    {

        /// <summary>
        /// The default time span, set at 6 hours
        /// </summary>
        public static readonly TimeSpan DefaultTimeSpan = new TimeSpan(6, 0, 0);

        private const string ResponseTextFormat = "<html><body>Response received after timeout: {0}</body></html>";
        private readonly string _responseText;

        public TimeSpan Timeout { get; }

        public TimeoutServer() : this(DefaultPort, DefaultTimeSpan)
        { }

        public TimeoutServer(int port) : this(port, DefaultTimeSpan)
        { }

        public TimeoutServer(TimeSpan timeout) : this(DefaultPort, timeout)
        { }

        public TimeoutServer(int port, TimeSpan timeout) : base(port)
        {
            Timeout = timeout;

            _responseText = string.Format(ResponseTextFormat, Timeout.ToString("c"));
        }

        protected override string ProcessRequest(HttpListenerRequest httpListenerRequest)
        {
            Console.WriteLine("\nRequest received: {0:F}\n", DateTime.Now);

            WriteToConsole(httpListenerRequest);

            Thread.Sleep(Timeout);

            Console.WriteLine("\nResponse sent: {0:F}\n", DateTime.Now);

            return _responseText;
        }
    }
}
