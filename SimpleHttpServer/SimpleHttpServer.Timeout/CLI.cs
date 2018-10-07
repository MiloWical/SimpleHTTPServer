namespace SimpleHttpServer.Timeout
{
    using System;

    public class CLI
    {

        private static int _hours;
        private static int _minutes;
        private static int _seconds;

        public static void Main(string[] args)
        {
            var server = args.Length > 0
                ? new TimeoutServer(CalculateTimeSpanFromCommandLineArgs(args))
                : new TimeoutServer();

            server.Serve();
        }

        private static TimeSpan CalculateTimeSpanFromCommandLineArgs(string[] args)
        {
            switch (args.Length)
            {
                case 3:
                    _hours = int.Parse(args[0]);
                    _minutes = int.Parse(args[1]);
                    _seconds = int.Parse(args[2]);
                    break;
                case 2:
                    _hours = 0;
                    _minutes = int.Parse(args[0]);
                    _seconds = int.Parse(args[1]);
                    break;
                case 1:
                    _hours = 0;
                    _minutes = 0;
                    _seconds = int.Parse(args[0]);
                    break;
                default:
                    return TimeoutServer.DefaultTimeSpan;
            }

            return new TimeSpan(_hours, _minutes, _seconds);
        }
    }
}
