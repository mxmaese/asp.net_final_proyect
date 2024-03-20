using Entidades.Seguridad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using Utiles;
using web.Models;
using web.Utils;

namespace web.Controllers;

public class LoginController : Controller
{
  private readonly ISecurityServices _seguridad;
  private readonly ILogger<LoginController> _logger;

  public LoginController(ISecurityServices seguridad, ILogger<LoginController> logger)
  {
    _seguridad = seguridad;
    _logger = logger;
  }

  [HttpGet]
  public IActionResult Inicio()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Inicio(string login, string hashedPass)
  {
    try
    {
      Usuario userConectado = _seguridad.Login(login, hashedPass);
      _logger.LogCritical(userConectado.ToString());
      if (userConectado != null)
        _logger.LogCritical("El usuario {user} se conecto correctamente", userConectado.Name);

      this.SetSession("User", userConectado);

      return RedirectToAction("Index", "Home");
    }
    catch (Exception ex)
    {

      FullErrorViewModel vm = new()
      {
        Titulo = "Parece que la fuerza no te acompaña...",
        Mensaje = "Las credenciales no son validas",
        Detalle = ex.Resumen(),
        Source = "Login Usuario",
        Comunicacion = "Que pasa que no puedo entrar??",
        TraceIdentifier = HttpContext.TraceIdentifier
      };
      return View("Errores/ErrorMejorado", vm);
    }
  }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("User"); 

        return RedirectToAction("Index", "Home"); 
    }
}
