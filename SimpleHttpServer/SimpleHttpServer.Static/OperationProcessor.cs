namespace SimpleHttpServer.Static
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class OperationProcessor
    {
        public const string OperationString = "operation";
        public const string MetricString = "metric";
        private readonly IDictionary<string, Func<HttpListenerRequest, string>> _operations;

        public OperationProcessor()
        {
            _operations = new Dictionary<string, Func<HttpListenerRequest, string>>();
        }

        public void AddOperation(string operationName, Func<HttpListenerRequest, string> operation)
        {
            if (string.IsNullOrEmpty(operationName)) throw new ArgumentException("Cannot add an empty or null operation name", nameof(operationName));
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            _operations.Add(operationName, operation);
        }

        public string ProcessOperation(string operationName, HttpListenerRequest request)
        {
            return ProcessOperation(operationName, request, string.Empty);
        }

        public string ProcessOperation(string operationName, HttpListenerRequest request, string defaultResponse)
        {
            return !_operations.TryGetValue(operationName, out var operation) ? defaultResponse : operation.Invoke(request);
        }
    }
}
