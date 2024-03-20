using Microsoft.AspNetCore.Mvc;
using Entidades.Seguridad;
using Servicios;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using web.Utils;
using System.Net;
using web.Models;
using Utiles;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Humanizer;

namespace web.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressServices _addressServices;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressServices addressServices, ILogger<AddressController> logger)
        {
            _addressServices = addressServices;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Addresses()
        {
            var user = HttpContext.Session.Get<Usuario>("User");
            if (user != null)
            {
                var addresses = _addressServices.GetAddress(user.Id_user);
                return View(addresses);
            }
            return BadRequest("No hay usuario en sesión.");
        }

        [HttpGet]
        public async Task<IActionResult> EditOrCreate(int? IdAddress)
        {
            try
            {
                var user = HttpContext.Session.Get<Usuario>("User");
                if (user != null)
                {
                    EditorCreate input = new EditorCreate();
                    if (IdAddress.HasValue)
                    {
                        if (user.Id_user == (_addressServices.GetAddressByIdAddress(IdAddress.GetValueOrDefault()) == null ? 0 : _addressServices.GetAddressByIdAddress(IdAddress.GetValueOrDefault()).Id_user))
                        {
                            var address = _addressServices.GetAddressByIdAddress(IdAddress.Value);
                            if (address != null)
                            {
                                var street_string = $"{address.Line_1.Split(";")[0]} {address.Line_1.Split(";")[1]}, {address.City}, {address.State}, {address.Country}";
                                var latlong = await _addressServices.GetCoordinates(street_string);
                                input.address = address;
                                TempData["IdAddress"] = address.Id_address;
                                _logger.LogCritical(latlong.ToString());
                                input.latitude = (latlong.Length != 0 ? latlong[0] : "1");
                                input.longitude = (latlong.Length != 0 ? latlong[1] : "1");

                                input.Street = address.Line_1.Split(";")[0];
                                input.Height = int.Parse(address.Line_1.Split(";")[1]);
                                
                                input.Floor = int.Parse(address.Line_2.Split(";")[0]);
                                input.Departament = address.Line_2.Split(";")[1];
                                return View(input);
                            }
                        }
                        else
                        {
                            FullErrorViewModel err =
                                new()
                                {
                                    Titulo = "Se ha intentado editar una direccion de otro usuario/inexistente",
                                    Mensaje = "Si cree que fue un error contactenos por soporte tecnico",
                                    Comunicacion = "Para comunicarse con soporte tecnico",
                                };

                            return View("Errores/ErrorMejorado", err);
                        }
                    }
                    TempData["IdAddress"] = 0;
                    return View(new EditorCreate());
                }
                else
                {
                    FullErrorViewModel err =
                      new()
                      {
                          Titulo = "Se ha intentado editar una direccion sin estar registrado/logueado",
                      };

                    return View("Errores/ErrorMejorado", err);
                }
            }
            catch (Exception ex)
            {
                FullErrorViewModel err =
                  new()
                  {
                      Titulo = "Se produjo un error inesperado",
                      Mensaje = "Intente recargar la pagina antes de comunicarse con soporte tecnico",
                      Detalle = ex.Message,
                      TraceIdentifier = HttpContext.TraceIdentifier,
                      Comunicacion = "Por favor comunicarse con soporte tecnico",
                      Source = "EditorCreate"
                  };

                return View("Errores/ErrorMejorado", err);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrCreate(EditorCreate editorCreate)
        {
            var validate = editorCreate.address;
            if (!editorCreate.Validate(ModelState))
            {
                return View(editorCreate);
            }

            try
            {
                editorCreate.address.Line_1 = editorCreate.Street + ";" + editorCreate.Height;
                editorCreate.address.Line_2 = editorCreate.Floor + ";" + editorCreate.Departament;
                var id_address = (TempData["IdAddress"] as int?).GetValueOrDefault();
                var db_address = _addressServices.GetAddressByIdAddress(id_address);
                if (id_address > 0)
                {
                    if (editorCreate.type == 0)
                    {
                        editorCreate.address.Id_address = id_address;
                        _addressServices.EditAddress(editorCreate.address);
                    }
                    else if (editorCreate.type == 1)
                    {
                        var address = await _addressServices.GetAddressComponentsFromCoordinates(editorCreate.latitude, editorCreate.longitude);
                        address.Id_address = db_address.Id_address;
                        address.Id_user = db_address.Id_user;

                        address.Line_2 = editorCreate.address.Line_2;
                        address.Message = editorCreate.address.Message;
                        _addressServices.EditAddress(address);
                    }
                }
                else
                {
                    var user = HttpContext.Session.Get<Usuario>("User");
                    if (user != null)
                    {
                        editorCreate.address.Id_user = user.Id_user;
                        _addressServices.CreateAddress(editorCreate.address);
                    }
                }
                return RedirectToAction(nameof(Addresses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la dirección.");
                FullErrorViewModel err =
                  new()
                  {
                      Titulo = "Se produjo un error al guardar la dirección",
                      Detalle = ex.Resumen(),
                      TraceIdentifier = HttpContext.TraceIdentifier,
                      Comunicacion = "Por favor comunicarse con soporte tecnico",
                      Source = "EditorCreateSave"
                  };

                return View("Errores/ErrorMejorado", err);
            }
        }

        [HttpGet]
        public IActionResult AddressDelete(int IdAddress)
        {
            try
            {
                _addressServices.DeleteAddress(IdAddress);
                return RedirectToAction(nameof(Addresses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la dirección.");
                return BadRequest("No se pudo eliminar la dirección.");
            }
        }

        public bool AllIsFillList(string input)
        {
            var list = input.Split(";");
            for (var i = 0; i < list.Length; i++)
            {
                if (list[i] == "")
                {
                    return false;
                }
            }
            return true;
        }
    }
}

public class EditorCreate
{
    public Address address { get; set; }

    public string Street { get; set; }

    public int Height { get; set; }

    public int Floor { get; set; }
    
    public string Departament { get; set; }

    public string latitude { get; set; }

    public string longitude { get; set; }

    public int type { get; set; } // 0 = inputs || 1 = map.

    public bool Validate(ModelStateDictionary ModelState)
    {
      var status = true;
      if (type == 0)
        {
            if (string.IsNullOrWhiteSpace(Street))
            {
                ModelState["Street"].Errors.Clear();
                ModelState.AddModelError("Street", "El campo Calle es obligatorio");
                status = false;
            }
            if(Height == 0)
            {
                ModelState["Height"].Errors.Clear();
                ModelState.AddModelError("Height", "El campo Altura es obligatorio");
                status = false;
            }

            if (string.IsNullOrWhiteSpace(address.Zip_code))
            {
                ModelState["address.Zip_code"].Errors.Clear();
                ModelState.AddModelError("address.Zip_code", "El campo Codigo postal es obligatorio");
                status = false;
            }
            if (string.IsNullOrWhiteSpace(address.City))
            {
                ModelState["address.City"].Errors.Clear();
                ModelState.AddModelError("address.City", "El campo Ciudad es obligatorio");
                status = false;
            }
            if (string.IsNullOrWhiteSpace(address.State))
            {
                ModelState["address.State"].Errors.Clear();
                ModelState.AddModelError("address.State", "El campo Provincia es obligatorio");
                status = false;
            }
            if (string.IsNullOrWhiteSpace(address.Country))
            {
                ModelState["address.Country"].Errors.Clear();
                ModelState.AddModelError("address.Country", "El campo Pais es obligatorio");
                status = false;
            }
        }
        return status;
    }
}