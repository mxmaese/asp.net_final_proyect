using Entidades.Seguridad;
using Datos.Repositorios;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Servicios;
public class AddressServices : IAddressServices
{
    private readonly IAddressRepo _repo;
    private readonly ILogger<AddressServices> _logger;

    public AddressServices(IAddressRepo repo, ILogger<AddressServices> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public IEnumerable<Address> GetAddress(int id_user)
    {
        return _repo.GetAddress(id_user);
    }

    public Address GetAddressByIdAddress(int id_address)
    {
        return _repo.GetAddress(id_address, 1).FirstOrDefault();
    }

    public void CreateAddress(Address address)
    {
        Address newAddress = new Address
        {
            Id_user = address.Id_user,
            Line_1 = address.Line_1,
            Line_2 = address.Line_2,
            Zip_code = address.Zip_code,
            City = address.City,
            State = address.State,
            Country = address.Country,
            Verificated = false
        };
        var address_string = $"{address.Line_1.Split(";")[0]} {address.Line_1.Split(";")[0]}, {address.City} {address.State}, {address.Country}";
        var address_validation = VerificateAddressAsync(address_string);
        _logger.LogCritical(address_string+ " - " + address_validation);
        if (address_validation != null)
        {
            _repo.AddAddress(newAddress);
        }
        else
        {
            throw new ApplicationException("No se ha encontrado la ubicacion");
        }
    }

    public void DeleteAddress(int Id_address)
    {
        _repo.RemoveAddress(Id_address);
    }

    public void EditAddress(Address NewAddress)
    {
        _repo.UpdateAddress(NewAddress);
    }

    private const string ApiKey = "AIzaSyCm5g5Bh2pVxBTUMAN1dCokpz6UuG5iiKo";
    public async Task<string> VerificateAddressAsync(string address)
    {
        try
        {
            HttpClient client = new HttpClient();
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={ApiKey}";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseBody);
            if (json["status"].ToString() == "OK")
            {
                string formattedAddress = json["results"][0]["formatted_address"].ToString();
                _logger.LogCritical("Se encontro la direccion: " + formattedAddress);
                return formattedAddress;
            }
            else
            {
                _logger.LogCritical("No se pudo encontrar la direccion");
                return null;
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return null;
        }
    }
    public async Task<Address> GetAddressComponentsFromCoordinates(string latitude, string longitude)
    {
        string street = "";
        int height = 0;
        var httpClient = new HttpClient();
        var requestUri = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={ApiKey}";

        var output = new Address();
        var response = await httpClient.GetAsync(requestUri);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var geocodingResponse = JsonConvert.DeserializeObject<GeocodingResponse>(json);

            if (geocodingResponse.status == "OK" && geocodingResponse.results.Length > 0)
            {
                foreach (var component in geocodingResponse.results[0].address_components)
                {
                    if (component.types.Contains("route"))
                    {
                        street = component.long_name;
                        Console.WriteLine($"Calle: {component.long_name}");
                    }
                    else if (component.types.Contains("street_number"))
                    {
                        height = int.Parse(component.long_name);
                        Console.WriteLine($"Número: {component.long_name}");
                    }
                    else if (component.types.Contains("locality"))
                    {
                        output.City = component.long_name;
                        Console.WriteLine($"Ciudad: {component.long_name}");
                    }
                    else if (component.types.Contains("administrative_area_level_1"))
                    {
                        output.State = component.long_name;
                        Console.WriteLine($"Estado/Provincia: {component.long_name}");
                    }
                    else if (component.types.Contains("country"))
                    {
                        output.Country = component.long_name;
                        Console.WriteLine($"País: {component.long_name}");
                    }
                    else if (component.types.Contains("postal_code"))
                    {
                        output.Zip_code = component.long_name;
                        Console.WriteLine($"Código Postal: {component.long_name}");
                    }
                }
            }
            else
            {
                _logger.LogCritical("No se pudo encontrar la dirección.");
            }
        }
        output.Line_1 = $"{street};{height}";
        return output;
    }
    public async Task<String[]> GetCoordinates(string address)
    {
        var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={ApiKey}";

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                var json = JObject.Parse(content);

                if (json["status"].ToString() == "OK")
                {
                    var location = json["results"][0]["geometry"]["location"];
                    var lat = location["lat"].Value<string>();
                    var lng = location["lng"].Value<string>();

                    return new string[] { lat, lng };
                }
            }
        }

        return new string[] { };
    }
}
public class GeocodingResponse
{
    public string status { get; set; }
    public Result[] results { get; set; }

    public class Result
    {
        public AddressComponent[] address_components { get; set; }
    }

    public class AddressComponent
    {
        public string long_name { get; set; }
        public string[] types { get; set; }
    }
}