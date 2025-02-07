using SimpleHttpServer.File;

int port = 80;

if(args.Length >= 1 && int.TryParse(args[0], out int parsedPort))
{
  port = parsedPort;
}

var processPath = Path.GetDirectoryName(Environment.ProcessPath);

var server = new FileServer(port, processPath!);

server.Serve();