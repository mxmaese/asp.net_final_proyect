using Entidades.Seguridad;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos.Contextos;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositorios;

public class SecurityRepo : ISecurityRepo
{
    private readonly SecurityContext _ctx;
    private readonly ILogger<SecurityContext> _logger;

    public SecurityRepo(SecurityContext ctx, ILogger<SecurityContext> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public IEnumerable<Perfil> GetPerfiles()
    {
        return _ctx.Perfiles.AsEnumerable();
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

    public bool SetPassword(Guid user, string pass)
    {
        return true;
    }

    public Usuario GetUsuarioFromLogin(string login)
    {
        try
        {
            var user = _ctx.Usuarios
                  .Include(u => u.Perfiles)
                  .SingleOrDefault(u => u.Login == login);
            return user;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Problema desconocido", ex);
        }
    }

    public bool ValidarPassword(string Login, string pass)
    {
        var resultado = _ctx.Database
          .SqlQuery<int>(
            $"select Id_User from users where Login = {Login}")
          .AsEnumerable()
          .SingleOrDefault();

        var data_pass = _ctx.Database
          .SqlQuery<string>(
            $"select Hashed_Password from users where Id_User = {resultado}")
          .AsEnumerable()
          .SingleOrDefault();

        return (data_pass == pass);
    }
}
