using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Servicios.Modelos;

namespace Servicios;

public class ImportServices : IImportServices
{
  private readonly ILogger<ImportServices> _logger;
  private readonly IConfiguration _config;

  public ImportServices(ILogger<ImportServices> logger, IConfiguration config)
  {
    _logger = logger;
    _config = config;
  }

  public async Task<IEnumerable<LibroDTO>> ObtenerLibros()
  {
    HttpClient cliente = new HttpClient();

    string url = _config["urlLibros"] 
                 ?? throw new ApplicationException("Se necesita configurar el valor de urlLibros");

    _logger.LogInformation("Importando libros desde: {apiUrl}", url);

    var response = await cliente.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
      if (response.StatusCode == HttpStatusCode.NotFound)
      {
        var cadenaError = await response.Content.ReadAsStringAsync();

        throw new ApplicationException($"Se recibio el error: {cadenaError}");
      }

      throw new ApplicationException($"Error sin datos");
    }

    var cadenaJson = await response.Content.ReadAsStringAsync();

    var opciones = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    var resultado = JsonSerializer.Deserialize<IEnumerable<LibroDTO>>(cadenaJson, opciones);

    return resultado;
  }
}
