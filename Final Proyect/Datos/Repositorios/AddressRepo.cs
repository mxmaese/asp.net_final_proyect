using Entidades.Seguridad;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos.Contextos;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Datos.Repositorios;

public class AddressRepo : IAddressRepo
{
    private readonly AddressContext _ctx;
    private readonly ILogger<AddressRepo> _logger;

    public AddressRepo(AddressContext ctx, ILogger<AddressRepo> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public IEnumerable<Address> GetAddress(int Id, int type_of_identity = 0)
    {
        if (type_of_identity == 0)
        {
            return _ctx.Address
                    .Where(add => add.Id_user == Id)
                    .AsEnumerable();
        }
        else if (type_of_identity == 1)
        {
            return _ctx.Address
                    .Where(add => add.Id_address == Id)
                    .AsEnumerable();
        }
        else
        {
            return null;
        }
    }

    public void AddAddress(Address address)
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
    }

    public void RemoveAddress(int id_address)
    {
        var transaction = _ctx.Database.BeginTransaction();

        try
        {
            _ctx.Database.ExecuteSqlRaw("DELETE FROM `address_users` WHERE `Id_Address` = {0}", id_address);
            _ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogCritical("*** Rollback ***");
            throw new ApplicationException("Error: ", ex);
        }

        transaction.Commit();
    }

    public void UpdateAddress(Address Address)
    {
        var transaction = _ctx.Database.BeginTransaction();

        try
        {
            var sql = $"UPDATE `address_users` SET `Line_1`='{Address.Line_1}',`Line_2`='{Address.Line_2}',`Zip_Code`='{Address.Zip_code}',`City`='{Address.City}',`State`='{Address.State}',`Country`='{Address.Country}',`Message`='{Address.Message}',`Verificated`='{Address.Verificated}' WHERE `Id_Address` = {Address.Id_address}";
            try
            {
                _logger.LogCritical(sql);
                _ctx.Database.ExecuteSql(FormattableStringFactory.Create(sql));
            }
            catch (DbUpdateException ex)
            {
                transaction.Rollback();
                _logger.LogCritical("Hubo un error al editar el usuario: " + Address.Id_address);
            }
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogCritical("*** Rollback ***");
            throw new ApplicationException("Error: ", ex);
        }
        _ctx.SaveChanges();
        transaction.Commit();
    }
}
