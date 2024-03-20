using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Servicios;

public class HelloService
{
  private readonly ILogger<HelloService> _logger;

  public HelloService(ILogger<HelloService> logger)
  {
    _logger = logger;
  }

  public string GetMensaje()
  {
    DateTime ahora = DateTime.Now;

    _logger.LogTrace($"{ahora:F}");

    return ahora switch
    {
      { Hour: >= 6 and <=13} => "Buenos Dias, Mundo!!!",
      { Hour: > 13 and <= 19 } => "Buenos Tardes, Mundo!!!",
      _ => "Hola, Mundo!!"
    };
  }
}
