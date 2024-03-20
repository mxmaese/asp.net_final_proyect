using Entidades.Seguridad;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios;

public interface IAddressRepo
{
    public IEnumerable<Address> GetAddress(int Id, int type_of_identity = 0);

    public void AddAddress(Address address);

    public void RemoveAddress(int id_address);

    public void UpdateAddress(Address Address);
}
