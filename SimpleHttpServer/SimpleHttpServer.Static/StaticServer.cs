﻿namespace SimpleHttpServer.Static
{
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

        protected override ProcessRequestResponse ProcessRequest(HttpListenerRequest httpListenerRequest)
        {
            if (_logToConsole)
                WriteToConsole(httpListenerRequest);

            if (httpListenerRequest.HttpMethod != HttpMethod.Get.Method) return new()
            {
                Body = ResponseString
            };

            var urlSegments = httpListenerRequest.RawUrl.Split('/');
            var processingStartIndex = GetFirstValidSegmentIndexFromUrlSegments(urlSegments);

            if (processingStartIndex == InvalidOperationIndex)
                return new()
                {
                    Body = ResponseString
                };

            var responseBody = urlSegments[processingStartIndex].Equals(OperationProcessor.OperationString, System.StringComparison.OrdinalIgnoreCase)
                ? _operationProcessor.ProcessOperation(urlSegments[processingStartIndex + 1], httpListenerRequest, ResponseString)
                : ResponseString;

            return new()
            {
                Body = responseBody
            };

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
