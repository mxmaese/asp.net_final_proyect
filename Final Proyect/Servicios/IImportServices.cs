using Servicios.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios;

public interface IImportServices
{
  Task<IEnumerable<LibroDTO>> ObtenerLibros();
}
