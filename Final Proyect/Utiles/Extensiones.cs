using System.Text;
using Microsoft.Extensions.Logging;

namespace Utiles;

public static class Extensiones
{
  /// <summary>
  /// Retorna un mensaje unico a partir de una excepcion, agregando los mensajes de las inner exception
  /// que pueda llegar a tener
  /// </summary>
  public static string Resumen(this Exception src)
  {
    StringBuilder sb = new();
    Exception next = src;

    while (next != null)
    {
      sb.Append(next.Message).Append("<br />");
      next = next.InnerException;
    }

    return sb.ToString();
  }
}
