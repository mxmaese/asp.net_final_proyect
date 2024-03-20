using Entidades;
using Entidades.Seguridad;
using Microsoft.AspNetCore.Mvc;
using web.Utils;

namespace web.Componentes;

public class UserInfoComponent :ViewComponent
{
    public IViewComponentResult Invoke()
    {
        if (HttpContext.Session.IsAvailable)
        {
            var user = HttpContext.Session.Get<Usuario>("User");

            if (user != null)
            {
                if (user.User_Types == TipoUsuario.Empleado)
                {
                    return View("Empleado", user);
                }
                else
                {
                    return View("Cliente", user);
                }
            }
            return View("UserGenerico");
        }
        return View("UserGenerico");
    }
}
