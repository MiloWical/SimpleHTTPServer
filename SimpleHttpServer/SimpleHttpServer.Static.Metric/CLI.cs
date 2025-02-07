namespace SimpleHttpServer.Static.Metric
{
    public class CLI
    {
        
        public static void Main(string[] args)
        {
            var server = new StaticMetricServer();

            server.Serve();
        }
    }
}
