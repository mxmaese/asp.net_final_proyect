
using cli;
using cli.Testing;
using Datos.Contextos;
using Datos.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servicios;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((ctx, serv) =>
{
    // serv.AddScoped<IImportServices, ImportServices>();
    //    serv.AddScoped<HelloService>();
    serv.AddSingleton<IAddressRepo, AddressRepo>();
    serv.AddSingleton<IAddressServices, AddressServices>();
    //  serv.AddScoped<ISecurityRepo, SecurityRepo>();

    //  serv.AddHostedService<AppImprimirLineas>();
    //  serv.AddHostedService<AppImportarLibros>();
    //  serv.AddHostedService<ETLSeguridad>();
    serv.AddHostedService<ETLSeguridad>();

  serv.AddDbContext<AddressContext>(opciones =>
  {
    opciones.UseSqlServer(ctx.Configuration.GetConnectionString("seguridad"));
    opciones.EnableDetailedErrors();
    opciones.EnableSensitiveDataLogging();
  });

  //  serv.AddSingleton<Aplicacion>(container => new Aplicacion(container.GetRequiredService<HelloService>(), "Hola, Mundo!!"));
  //  serv.AddSingleton<Aplicacion>();
});

var host = builder.Build();

host.Run();

//  == new Aplicacion(...)
//  var app = host.Services.GetRequiredService<Aplicacion>();  

//  await app.Ejecutar();


Console.ReadLine();

public class Aplicacion 
{
  private readonly HelloService _serv;
  private readonly IImportServices _import;

  public Aplicacion(HelloService serv, IImportServices import)
  {
    _serv = serv;
    _import = import;
  }

  public async Task Ejecutar()
  {
    //var hello = new HelloService();
    Console.WriteLine(_serv.GetMensaje());
    var listaImportacion = _import.ObtenerLibros();

    foreach (var libroDTO in await listaImportacion)
    {
      Console.WriteLine($"{libroDTO.ISBN} {libroDTO.Titulo} {libroDTO.Paginas}");
    }
  }
}


/// <summary>
/// Uno de los hosted-service que vamos a tener en esta aplicacion de linea de comandos
/// Obtiene datos de libros desde una API externa
/// </summary>
public class AppImportarLibros : IHostedService
{
  private readonly HelloService _serv;
  private readonly IImportServices _import;

  public AppImportarLibros(HelloService serv, IImportServices import)
  {
    _serv = serv;
    _import = import;
  }

  public async Task Ejecutar()
  {
    Console.WriteLine(_serv.GetMensaje());
    var listaImportacion = _import.ObtenerLibros();

    foreach (var libroDTO in await listaImportacion)
    {
      Console.WriteLine($"{libroDTO.ISBN} {libroDTO.Titulo} {libroDTO.Paginas}");
    }
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Run(() => Ejecutar());

    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}

/// <summary>
/// El otro hosted-service de esta aplicacion sera una tarea que imprime una linea cada
/// determinado tiempo. Simplemente la usamos para ver como ambos hosted-services coexisten
/// y se procesan simultaneamente
/// </summary>
public class AppImprimirLineas : IHostedService
{
  private readonly HelloService _serv;

  public AppImprimirLineas(HelloService serv)
  {
    _serv = serv;
  }

  public async Task Ejecutar()
  {
    for (int j = 1; j < 50; j++)
    {
      Console.WriteLine("Imprimir Linea");
      await Task.Delay(100);
    }
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Run(Ejecutar);

    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
