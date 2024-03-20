using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace web.Utils;

public static class ControllerExtencion
{
    /// <summary>
    /// evitamos serirializar tipos valor con este metodo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scr"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void Set<T>(this ISession scr, string key, T value)
    {
        scr.SetString(key, JsonConvert.SerializeObject(value));
    }

    /// <summary>
    /// Obtiene una instancia del tipo T a partir de una clave en la sesion
    /// Si el valor no existe en la sesion, retorna el defaul(T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scr"></param>
    /// <param name="key"></param>
    public static T Get<T>(this ISession scr, string key)
    {
        string valor = scr?.GetString(key);

        return valor is null ? default : JsonConvert.DeserializeObject<T>(valor);
    }

    public static void clear<T>(this ISession scr, string key)
    {
        scr.Remove(key);
    }

    public static T GetSession<T>(this ControllerBase scr, string key)
    {
        return scr.HttpContext.Session.Get<T>(key);
    }

    public static void SetSession<T>(this ControllerBase scr, string key, T value)
    {
        scr.HttpContext.Session.Set<T>(key, value);
    }
}
