using Entidades.Seguridad;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos.Contextos;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Datos.Repositorios;

public class RegisterRepo : IRegisterRepo
{
    private readonly RegisterContext _ctx;
    private readonly ILogger<RegisterRepo> _logger;

    public RegisterRepo(RegisterContext ctx, ILogger<RegisterRepo> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public IEnumerable<Perfil> GetPerfiles(TipoUsuario? type)
    {
        if (type == null)
        {
            return _ctx.Perfiles.AsEnumerable();
        }
        else
        {
            return _ctx.Perfiles.Where(per => per.Type_User == type);
        }
    }

    public Usuario CrearUsuario(Usuario nuevo, string pass)
    {
        using var transaccion = _ctx.Database.BeginTransaction();

        try
        {
            _ctx.Usuarios.Add(nuevo);
            _ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            transaccion.Rollback();

            _logger.LogCritical("*** ROLLBACK!!! ***");
            throw new ApplicationException("No se pudo ingresar el nuevo usuario", ex);
        }

        transaccion.Commit();
        return nuevo;
    }
    public Address AddAddress(Address address)
    {
        using var transaccion = _ctx.Database.BeginTransaction();

        try
        {
            _ctx.Address.Add(address);
            _ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            transaccion.Rollback();

            _logger.LogCritical("*** ROLLBACK!!! ***");
            throw new ApplicationException("No se pudo añadir la nueva direccion", ex);
        }

        transaccion.Commit();
        return address;
    }
}
