using Entidades.Seguridad;
using Entidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos.Repositorios;
using Microsoft.Extensions.Logging;
using Utiles;

namespace Servicios;
public class RegisterServices : IRegisterServices
{
    private readonly IRegisterRepo _repo;
    private readonly ILogger<RegisterServices> _logger;

    public RegisterServices(IRegisterRepo repo, ILogger<RegisterServices> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public IEnumerable<Perfil> GetPerfiles()
    {
        return _repo.GetPerfiles(null);
    }

    public Usuario CrearEmpleado(string nombre, string mail, string login, string pwd, DateTime nacimiento,
      int[] perfiles)
    {
        Usuario nuevo = new Usuario
        {
            Name = nombre,
            Login = login,
            Email = mail,
            Birthday_Date = nacimiento,
            User_Types = TipoUsuario.Empleado,
            Register_Date = DateTime.Now,
            Pass = pwd
        };

        foreach (var perfil in perfiles)
            nuevo.Perfiles.Add(GetPerfiles().Single(p => p.Id_Profiles == perfil));

        return _repo.CrearUsuario(nuevo, pwd);
    }

    public Usuario CrearCliente(string nombre, string mail, string login, string pwd, DateTime nacimiento, Address address)
    {
        Usuario nuevo = new Usuario
        {
            Name = nombre,
            Login = login,
            Email = mail,
            Birthday_Date = nacimiento,
            User_Types = TipoUsuario.Cliente,
            Register_Date = DateTime.Now,
            Pass = pwd
        };

        nuevo.Perfiles.Add(GetPerfiles().Single(p => p.Name == "Visitante"));
        var output = _repo.CrearUsuario(nuevo, pwd);
        
        address.Id_user = output.Id_user;

        _repo.AddAddress(address);

        return output;
    }
}
