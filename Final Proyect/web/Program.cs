using Datos.Contextos;
using Datos.Repositorios;
using Microsoft.EntityFrameworkCore;
using Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<HelloService>();
builder.Services.AddScoped<IImportServices, ImportServices>();
builder.Services.AddScoped<ISecurityServices, SecurityServices>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<IAddressServices, AddressServices>();
builder.Services.AddScoped<ISecurityRepo, SecurityRepo>();
builder.Services.AddScoped<ISecurityServices, SecurityServices>();
builder.Services.AddScoped<IRegisterRepo, RegisterRepo>();
builder.Services.AddScoped<IRegisterServices, RegisterServices>();

builder.Services.AddSession();

builder.Services.AddDbContext<SecurityContext>(opciones =>
{
    opciones.UseMySql(builder.Configuration.GetConnectionString("seguridad"), new MySqlServerVersion("8.0.0"));
    opciones.EnableDetailedErrors();
    opciones.EnableSensitiveDataLogging();
});

builder.Services.AddDbContext<AddressContext>(opciones =>
{
    opciones.UseMySql(builder.Configuration.GetConnectionString("seguridad"), new MySqlServerVersion("8.0.0"));
    opciones.EnableDetailedErrors();
    opciones.EnableSensitiveDataLogging();
});

builder.Services.AddDbContext<RegisterContext>(opciones =>
{
    opciones.UseMySql(builder.Configuration.GetConnectionString("seguridad"), new MySqlServerVersion("8.0.0"));
    opciones.EnableDetailedErrors();
    opciones.EnableSensitiveDataLogging();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();
