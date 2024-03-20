using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Seguridad;

public class Perfil
{
  public int Id_Profiles { get; set; }

  public string Name { get; set; }

  /// <summary>
  /// El tipo de usuario al cual se aplicara este perfil
  /// </summary>
  public TipoUsuario Type_User { get; set; }

  public string Description { get; set; }
}
