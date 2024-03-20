using Entidades.Seguridad;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios;

public interface IAddressServices
{
    public IEnumerable<Address> GetAddress(int id_user);

    public Address GetAddressByIdAddress(int id_address);

    public void CreateAddress(Address address);

    public void DeleteAddress(int Id_address);

    public void EditAddress(Address NewAddress);

    Task<string> VerificateAddressAsync(string address);

    public Task<Address> GetAddressComponentsFromCoordinates(string latitude, string longitude);

    public Task<String[]> GetCoordinates(string address);
}
