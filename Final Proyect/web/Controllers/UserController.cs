using Entidades;
using Entidades.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Servicios;
using System.Net;
using System.Runtime.Intrinsics.X86;
using Utiles;
using web.Models;

namespace web.Controllers;
public class UserController : Controller
{
    private readonly IRegisterServices _Register;
    private readonly ILogger<UserController> _logger;

    public UserController(IRegisterServices Register, ILogger<UserController> logger)
    {
        _Register = Register;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult NewUser()
    {
        var vm = new NewUserViewModel
        {
            Usuario = new Usuario(),
            Address = new Address()
        };
        return View(vm);
    }

    [HttpPost]
    public IActionResult NewUser(NewUserViewModel UserViewModel)
    {
        UserViewModel.Address.Line_1 = UserViewModel.Street + ";" + UserViewModel.Height;
        UserViewModel.Address.Line_2 = UserViewModel.Floor + ";" + UserViewModel.Departament;
        var ModelStateIsValid = ModelState.IsValid;
        var UMValidateUser = UserViewModel.ValidateUser(ModelState);
        var UMValidateAddress = UserViewModel.ValidateAddress(ModelState);
        if (!ModelStateIsValid || !UMValidateUser || !UMValidateAddress)
        {
            var errores = ModelState
                            .Where(ms => ms.Value.ValidationState == ModelValidationState.Invalid)
                            .Select(par => par.Value)
                            .ToList();


            return View(UserViewModel);
        }
        try
        {
            var nuevoUsuario = _Register.CrearCliente(UserViewModel.Usuario.Name, UserViewModel.Usuario.Email, UserViewModel.Usuario.Login,
            UserViewModel.Usuario.Pass, UserViewModel.Usuario.Birthday_Date, UserViewModel.Address);

            _logger.LogInformation("Nuevo usuario ingresado con clave = {ID}", nuevoUsuario.Pass);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            FullErrorViewModel err =
              new()
              {
                  Titulo = "Houston...tenemos un problema",
                  Mensaje = "Se produjo un error intentando guardar el nuevo usuario",
                  Detalle = ex.Resumen(),
                  TraceIdentifier = HttpContext.TraceIdentifier,
                  Comunicacion = "Por favor comunicarse con soporte tecnico",
                  Source = "Nuevo Usuario"
              };

            return View("Errores/ErrorMejorado", err);
        }

        #region FUNCIONES LOCALES

        IActionResult ReenviarFormulario()
        {
            var vm = new NewUserViewModel
            {
                Usuario = UserViewModel.Usuario,
                Address = UserViewModel.Address
            };
            return View(vm);
        }
    }
}
#endregion

public class NewUserViewModel
{
    public Usuario Usuario { get; set; }

    /// <summary>
    /// Lista de <see cref="SelectListItem"/> que representan perfiles que podemos todavia asignar al usuario
    /// </summary>
    public Address Address { get; set; }


    public string Street { get; set; }

    public int Height { get; set; }

    public int Floor { get; set; }

    public string Departament { get; set; }

    public bool ValidateUser(ModelStateDictionary ModelState)
    {
        var status = true;

        
        if (string.IsNullOrWhiteSpace(Usuario.Name))
        {
            ModelState["Usuario.Name"].Errors.Clear();
            ModelState.AddModelError("Usuario.Name", "El campo Nombre es obligatorio");
            status = false;
        }
        if (string.IsNullOrWhiteSpace(Usuario.Login))
        {
            ModelState["Usuario.Login"].Errors.Clear();
            ModelState.AddModelError("Usuario.Login", "El campo Login es obligatorio");
            status = false;
        }
        if (Usuario.Pass == null)
        {
            if (!ModelState.ContainsKey("Usuario.Pass"))
            {
                ModelState["Usuario.Pass"].Errors.Clear();
            }
            ModelState.AddModelError("Usuario.Pass", "El campo Contraceña es obligatorio");
            status = false;
        }
        if (string.IsNullOrWhiteSpace(Usuario.Email))
        {
            ModelState["Usuario.Email"].Errors.Clear();
            ModelState.AddModelError("Usuario.Email", "El campo Email es obligatorio");
            status = false;
        }
        var yearis= Usuario.Birthday_Date.Year == 0001;
        if (Usuario.Birthday_Date.Year == 0001)
        {
            ModelState["Usuario.Birthday_Date"].Errors.Clear();
            ModelState.AddModelError("Usuario.Birthday_Date", "El campo Fecha de nacimiento es obligatorio");
            status = false;
        }
        if (Usuario.Birthday_Date.Year < 1900)
        {
            ModelState["Usuario.Birthday_Date"].Errors.Clear();
            ModelState.AddModelError("Usuario.Birthday_Date", "El campo Fecha debe ser porterior a 1900");
            status = false;
        }

        return status;
    }

    public bool ValidateAddress(ModelStateDictionary ModelState)
    {
        var status = true;
        if (string.IsNullOrWhiteSpace(Street))
        {
            ModelState["Street"].Errors.Clear();
            ModelState.AddModelError("Street", "El campo Calle es obligatorio");
            status = false;
        }
        if (Height == 0)
        {
            ModelState["Height"].Errors.Clear();
            ModelState.AddModelError("Height", "El campo Altura es obligatorio");
            status = false;
        }

        if (string.IsNullOrWhiteSpace(Address.Zip_code))
        {
            ModelState["Address.Zip_code"].Errors.Clear();
            ModelState.AddModelError("address.Zip_code", "El campo Codigo postal es obligatorio");
            status = false;
        }
        if (string.IsNullOrWhiteSpace(Address.City))
        {
            ModelState["Address.City"].Errors.Clear();
            ModelState.AddModelError("address.City", "El campo Ciudad es obligatorio");
            status = false;
        }
        if (string.IsNullOrWhiteSpace(Address.State))
        {
            ModelState["Address.State"].Errors.Clear();
            ModelState.AddModelError("address.State", "El campo Provincia es obligatorio");
            status = false;
        }
        if (string.IsNullOrWhiteSpace(Address.Country))
        {
            ModelState["Address.Country"].Errors.Clear();
            ModelState.AddModelError("address.Country", "El campo Pais es obligatorio");
            status = false;
        }
        return status;
    }
}
