using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using Servicios;
using web.Models;

namespace web.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly IImportServices _import;

  public HomeController(ILogger<HomeController> logger, IImportServices import)
  {
    _logger = logger;
    _import = import;
  }

  public async Task<IActionResult> Index()
  {
    ViewData.Model = null;
    return View("Inicio");
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
