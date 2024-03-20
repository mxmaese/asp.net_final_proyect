using Entidades.Seguridad;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Servicios;
using System.IO;
using System.Runtime.Intrinsics.X86;
using Utiles;
using web.Models;

namespace web.Controllers;
public class EmployeeController : Controller
{
    private readonly IRegisterServices _Register;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IRegisterServices Register, ILogger<EmployeeController> logger)
    {
        _Register = Register;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult NewEmployee()
    {
        var vm = new NewEmployeeViewModel
        {
            Usuario = new Usuario(),
            PerfilesDisponibles = _Register.GetPerfiles()
                                  .Where(p => p.Type_User == TipoUsuario.Empleado)
                                  .Select(p => new SelectListItem(p.Name, p.Id_Profiles.ToString())),
            PerfilesAsignados = null
        };
        return View(vm);
    }

    [HttpPost]
    public IActionResult NewEmployee(NewEmployeeViewModel employeeViewModel, int[] perfilesAsignados)
    {
        
        var EMValidateUser = employeeViewModel.ValidateUser(ModelState);
        if (!ModelState.IsValid || !EMValidateUser)
        {
            var errores = ModelState
                            .Where(ms => ms.Value.ValidationState == ModelValidationState.Invalid)
                            .Select(par => par.Value)
                            .ToList();

            
            return ReenviarFormulario("Employee");
        }
        if (!ModelState.IsValid)
            return ReenviarFormulario("Employee");
        try
        {
            _logger.LogCritical("a");
            var id_perfiles = StringArrToIntArr(employeeViewModel.PerfilesAsignados.AsQueryable().Select(a => a.Value).ToList().ToArray());

            var nuevoUsuario = _Register.CrearEmpleado(employeeViewModel.Usuario.Name, employeeViewModel.Usuario.Email, employeeViewModel.Usuario.Login,
            employeeViewModel.Usuario.Pass, employeeViewModel.Usuario.Birthday_Date, perfilesAsignados);

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
        static int[] StringArrToIntArr(String[] s)
        {
            int[] result = new int[s.Count()];
            for (int i = 0; i < s.Count(); i++)
            {
                result[i] = int.Parse(s[i]);
            }
            return result;
        }

        IActionResult ReenviarFormulario(string type)
        {
            var allPerfiles = _Register
              .GetPerfiles()
              .Where(p => p.Type_User == TipoUsuario.Empleado)
              .ToList();
            int[] perfilesAsignados = (employeeViewModel.PerfilesAsignados != null? StringArrToIntArr(employeeViewModel.PerfilesAsignados.Select(a => a.Value).ToArray()):null);

            NewEmployeeViewModel vm;
            if (perfilesAsignados != null)
            {
                vm = new NewEmployeeViewModel
                {
                    Usuario = employeeViewModel.Usuario,
                    PerfilesDisponibles = allPerfiles.ExceptBy(perfilesAsignados, p => p.Id_Profiles)
                        .Select(p => new SelectListItem(p.Name, p.Id_Profiles.ToString())),
                    PerfilesAsignados = allPerfiles.IntersectBy(perfilesAsignados, p => p.Id_Profiles)
                        .Select(p => new SelectListItem(p.Name, p.Id_Profiles.ToString()))
                };
            }
            else
            {
                vm = new NewEmployeeViewModel
                {
                    Usuario = employeeViewModel.Usuario,
                    PerfilesDisponibles = _Register.GetPerfiles()
                                  .Where(p => p.Type_User == TipoUsuario.Empleado)
                                  .Select(p => new SelectListItem(p.Name, p.Id_Profiles.ToString())),
                    PerfilesAsignados = null
                };
            }
            return View(vm);
        }
    }
}
    #endregion

public class NewEmployeeViewModel
{
    public Usuario Usuario { get; set; }

    /// <summary>
    /// Lista de <see cref="SelectListItem"/> que representan perfiles que podemos todavia asignar al usuario
    /// </summary>
    public IEnumerable<SelectListItem> PerfilesDisponibles { get; set; }

    /// <summary>
    /// Lista de <see cref="SelectListItem"/> que representan los perfiles ya asignados al usuario si estoy editando
    /// o una lista vacia si estoy creando uno nuevo
    /// </summary>
    public IEnumerable<SelectListItem> PerfilesAsignados { get; set; }

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
        if (string.IsNullOrWhiteSpace(Usuario.Birthday_Date.Year.ToString()))
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
        if(PerfilesAsignados == null)
        {
            if (ModelState.ContainsKey("PerfilesAsignados"))
            {
                ModelState["PerfilesAsignados"].Errors.Clear();
            }
            ModelState.AddModelError("PerfilesAsignados", "Es obligatorio seleccionar al menos un tipo");
            status = false;
        }

        return status;
    }
}