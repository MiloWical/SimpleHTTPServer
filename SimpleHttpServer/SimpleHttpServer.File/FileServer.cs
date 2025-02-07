using System.Net;
using SimpleHttpServer.Base;

namespace SimpleHttpServer.File;

public class FileServer : HttpServerBase
{
  private readonly string _baseDir;

    public FileServer(int port, string baseDir)
      : base(port)
    {
        if(baseDir is not null)
        {
          _baseDir = baseDir.TrimEnd('/');
        }
        else
        {
          _baseDir = ".";
        }

        Console.WriteLine("Base Directory: " + _baseDir);
    }

    protected override string ProcessRequest(HttpListenerRequest httpListenerRequest)
  {
    var file = _baseDir + httpListenerRequest.RawUrl;

    Console.WriteLine("Returning file: " + file);

    if(!System.IO.File.Exists(file))
    {
      return "Not Found";
    }

    // For now, we'll ignore the method and just return the file contents.

    return System.IO.File.ReadAllText(file);
  }
}
