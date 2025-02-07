namespace SimpleHttpServer.Static
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
