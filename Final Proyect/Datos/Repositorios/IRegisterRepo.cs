using Datos.Contextos;
using Entidades;
using Entidades.Seguridad;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios;

public interface IRegisterRepo
{
    public IEnumerable<Perfil> GetPerfiles(TipoUsuario? type);

    public Usuario CrearUsuario(Usuario nuevo, string pass);

    public Address AddAddress(Address address);
}
