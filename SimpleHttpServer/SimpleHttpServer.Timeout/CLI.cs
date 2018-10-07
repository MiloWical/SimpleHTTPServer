namespace SimpleHttpServer.Timeout
{
    using System;
    using Base;

    public class CLI
    {
        private static int _port;
        private static int _hours;
        private static int _minutes;
        private static int _seconds;

        public static void Main(string[] args)
        {
            var timeout = ProcessCommandLineArgs(args);

            var server = new TimeoutServer(_port, timeout);

            server.Serve();
        }

        private static TimeSpan ProcessCommandLineArgs(string[] args)
        {
            switch (args.Length)
            {
                case 4:
                    _port = int.Parse(args[0]);
                    _hours = int.Parse(args[1]);
                    _minutes = int.Parse(args[2]);
                    _seconds = int.Parse(args[3]);
                    break;
                case 3:
                    _port = HttpServerBase.DefaultPort;
                    _hours = int.Parse(args[0]);
                    _minutes = int.Parse(args[1]);
                    _seconds = int.Parse(args[2]);
                    break;
                case 2:
                    _port = HttpServerBase.DefaultPort;
                    _hours = 0;
                    _minutes = int.Parse(args[0]);
                    _seconds = int.Parse(args[1]);
                    break;
                case 1:
                    _port = HttpServerBase.DefaultPort;
                    _hours = 0;
                    _minutes = 0;
                    _seconds = int.Parse(args[0]);
                    break;
                default:
                    _port = HttpServerBase.DefaultPort;
                    return TimeoutServer.DefaultTimeSpan;
            }

            return new TimeSpan(_hours, _minutes, _seconds);
        }
    }
}
