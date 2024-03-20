using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Seguridad;

public class Usuario
{
  public Usuario()
  {
    Perfiles = new HashSet<Perfil>();
  }

  public int Id_user { get; set; }

  public string  Pass { get; set; }

  public string Login { get; set; }

  public string Name { get; set; }

  public TipoUsuario User_Types { get; set; }

  public bool Active { get; set; }

  public string Email { get; set; }

  public DateTime? Last_Login { get; set; }

  public DateTime Register_Date { get; set; }

  public DateTime Birthday_Date { get; set; }

  public ISet<Perfil> Perfiles { get; set; }
  
  public ISet<Address> Address { get; set; }
}
