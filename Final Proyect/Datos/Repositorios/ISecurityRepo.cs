using Entidades.Seguridad;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios;

public interface ISecurityRepo
{
  IEnumerable<Perfil> GetPerfiles();

  Usuario CrearUsuario(Usuario nuevo, string pass);

  bool SetPassword(Guid user, string pass);

  Usuario GetUsuarioFromLogin(string login);

  bool ValidarPassword(string Login, string pass);
}
