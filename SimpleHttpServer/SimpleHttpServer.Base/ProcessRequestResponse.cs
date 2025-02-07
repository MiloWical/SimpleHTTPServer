using System;
using System.Net;
using System.Net.Http;

namespace SimpleHttpServer.Base;

public record ProcessRequestResponse
{
  public string Body { get; init; } = string.Empty;
  public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
} 
