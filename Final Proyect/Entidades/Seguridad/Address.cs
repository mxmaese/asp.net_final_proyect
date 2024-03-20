using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Seguridad;

public class Address
{
    public int Id_address { get; set; }

    public int Id_user { get; set; }

    public string Line_1 { get; set; }

    public string? Line_2 { get; set; }

    public string Zip_code { get; set; }

    public string City { get; set; }

    public string? State { get; set; }
    
    public string? Country { get; set; }

    public string? Message {  get; set; }

    [Required]
    public bool Verificated { get; set; }

    public Usuario User { get; set; }

    
}
