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

    protected override ProcessRequestResponse ProcessRequest(HttpListenerRequest httpListenerRequest)
  {
    var file = _baseDir + httpListenerRequest.RawUrl;

    Console.WriteLine("Returning file: " + file);

    if(!System.IO.File.Exists(file))
    {
      return new()
      {
        Body = "Not Found",
        StatusCode = HttpStatusCode.NotFound
      };
    }

    // For now, we'll ignore the method and just return the file contents.

    return new()
    {
      Body = System.IO.File.ReadAllText(file)
    };
  }
}
