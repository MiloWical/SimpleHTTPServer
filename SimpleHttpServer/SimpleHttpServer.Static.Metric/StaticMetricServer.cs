namespace SimpleHttpServer.Static.Metric
{
    using System;
    using System.Globalization;
    using System.Net;

    public class StaticMetricServer : StaticServer
    {
        private readonly OperationProcessor _metricProcessor;

        private DateTime _startTime;
        private int _totalRequests;

        public StaticMetricServer()
        {
            _metricProcessor = new OperationProcessor();
            _totalRequests = 0;

            //"rps" = requests per second
            _metricProcessor.AddOperation("rps",
                httpListenerContext =>
                    ((double) _totalRequests / (DateTime.Now - _startTime).Milliseconds)
                    .ToString(CultureInfo.InvariantCulture));
        }

        public new void Serve()
        {
            _startTime = DateTime.Now;

            base.Serve();
        }

        protected override string ProcessRequest(HttpListenerRequest httpListenerRequest)
        {
            _totalRequests++;

            var result = base.ProcessRequest(httpListenerRequest);

            var urlSegments = httpListenerRequest.RawUrl.Split('/');
            var processingStartIndex = GetFirstValidSegmentIndexFromUrlSegments(urlSegments);

            if (processingStartIndex == InvalidOperationIndex)
                return ResponseString;

            if(urlSegments[processingStartIndex].ToLower() == OperationProcessor.MetricString)
                result = _metricProcessor.ProcessOperation(urlSegments[processingStartIndex + 1], httpListenerRequest, ResponseString);

            return result;
        }
    }
}
