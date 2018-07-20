namespace SimpleHttpServer.Static
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using Base;

    public class StaticServer : HttpServerBase
    {
        private readonly OperationProcessor _operationProcessor;
        protected const int InvalidOperationIndex = -1;
        private const string ToggleLoggingOperation = "toggleLogging";

        private bool _logToConsole;
        
        protected const string ResponseString = "<HTML><BODY>Transaction Successful.</BODY></HTML>";

        public StaticServer()
        {
            _operationProcessor = new OperationProcessor();
            _operationProcessor.AddOperation(ToggleLoggingOperation, httpListenerContext =>
                {
                    _logToConsole = !_logToConsole;
                    return _logToConsole.ToString();
                });

            _logToConsole = true;
        }

        protected override string ProcessRequest(HttpListenerRequest httpListenerRequest)
        {
            if (_logToConsole)
                WriteToConsole(httpListenerRequest);

            if (httpListenerRequest.HttpMethod != HttpMethod.Get.Method) return ResponseString;


            var urlSegments = httpListenerRequest.RawUrl.Split('/');
            var processingStartIndex = GetFirstValidSegmentIndexFromUrlSegments(urlSegments);

            if (processingStartIndex == InvalidOperationIndex)
                return ResponseString;

            return urlSegments[processingStartIndex].ToLower() == OperationProcessor.OperationString 
                ? _operationProcessor.ProcessOperation(urlSegments[processingStartIndex + 1], httpListenerRequest, ResponseString) 
                : ResponseString;
        }

        private static void WriteToConsole(HttpListenerRequest httpListenerRequest)
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

        protected virtual int GetFirstValidSegmentIndexFromUrlSegments(string[] segments)
        {
            if (segments.Length < 2)
                return InvalidOperationIndex;

            var firstValidSegmentIndex = 0;

            while (string.IsNullOrWhiteSpace(segments[firstValidSegmentIndex]))
            {
                firstValidSegmentIndex++;

                if (segments.Length == firstValidSegmentIndex)
                    return InvalidOperationIndex;
            }

            if (segments.Length == firstValidSegmentIndex + 1)
                return InvalidOperationIndex;

            return firstValidSegmentIndex;
        }
    }
}
