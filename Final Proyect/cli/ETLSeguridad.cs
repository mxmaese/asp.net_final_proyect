using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos.Contextos;
using Entidades;
using Entidades.Seguridad;
using Servicios;

namespace cli;

/// <summary>
/// Permite realizar la inicializacion de valores para la tabla de usuarios y perfiles
/// Perfiles por ahora parece que esta creada asi que no lo hariamos
/// </summary>
public class ETLSeguridad : IHostedService
{
    private readonly ILogger<ETLSeguridad> _logger;

    private readonly IAddressServices _Address;

    //  private readonly ISecurityServices _security;
    private readonly IHostApplicationLifetime _host;

    public ETLSeguridad(ILogger<ETLSeguridad> logger, IAddressServices security,
      IHostApplicationLifetime host)
    {
        _logger = logger;
        _Address = security;
        //  _security = security;
        _host = host;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public bool AllIsFillList(string input)
    {
        var list = input.Split(";");
        for(var i = 0; i < list.Length; i++)
        {
            if (list[i] == "")
            {
                return false;
            }
        }
        return true;
    }
    #region WORKING METHODS

    /*
  private async Task GenerarDatosUsuarios()
  {
    try
    {
      //  bool conecta = await _ctx.Database.CanConnectAsync();

      //  Console.WriteLine(conecta ? "CONEXION OK!!" : "ERROR!! FALLA CONEXION");

      var misPerfiles = _security.GetPerfiles().ToList();
      //  var misPerfiles = _security.GetPerfiles().ToList();

      //  intentando leer los perfiles
      foreach (var perfil in misPerfiles)
        Console.WriteLine($"{perfil.ID} {perfil.Nombre} {perfil.TipoUsuario} {perfil.Descripcion}");

      var ethedy = _security.CrearEmpleado("Enrique Thedy", "ethedy@gmail.com", "ethedy", "1234", new DateTime(1967, 4, 10),
        new[] { 1 });

      var groot = _security.CrearEmpleado("Groot", "groot@gmail.com", "imgroot", "5678", new DateTime(2598, 1, 31),
        new[] { 2, 5 });

      var bill = _security.CrearCliente("Bill Gates", "bgates@gmail.com", "bgates", "9865", new DateTime(1964, 10, 7));

      Console.WriteLine($"ID ethedy: {ethedy.Clave} {ethedy.TipoUsuario}");
      Console.WriteLine($"ID groot: {groot.Clave} {groot.TipoUsuario}");
      Console.WriteLine($"ID bill: {bill.Clave} {bill.TipoUsuario}");
    }
    catch (Exception ex)
    {
      _logger.LogCritical(ex, "Fallo la verificacion de conectividad contra la DB");
      Console.WriteLine("La base de datos no esta conectada!!");
      _host.StopApplication();
    }
  }

  */
    #endregion
}
