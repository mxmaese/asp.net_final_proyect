using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utiles;

public static class Util
{
  public static bool NoThrow(Action operacion) => NoThrow(operacion, null);

  /// <summary>
  /// Permite llamar a un metodo que posiblemente cause una excepcion pero sin que la misma se propague
  /// al nivel superior
  /// Usar con sabiduria!
  /// Solo si sabemos que ignorar el error no producira mayores problemas
  /// </summary>
  /// <param name="operacion"></param>
  /// <param name="logger"></param>
  /// <returns></returns>
  public static bool NoThrow(Action operacion, ILogger logger)
  {
    if (operacion == null)
      return false;

    try
    {
      operacion.Invoke();
    }
    catch (Exception ex)
    {
      if (logger != null)
        logger.LogCritical(ex, "Se ignoro la excepcion");

      return false;
    }
    return true;
  }
}
