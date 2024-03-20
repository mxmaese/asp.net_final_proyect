using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;
using Servicios.Modelos;

namespace cli.Testing;

public class MockImportServices : IImportServices
{
  public Task<IEnumerable<LibroDTO>> ObtenerLibros()
  {
    return Task.FromResult(Enumerable.Repeat(
      new LibroDTO() {ISBN = "1234", Titulo = "Titulo", Paginas = 123}, 1));
  }
}
