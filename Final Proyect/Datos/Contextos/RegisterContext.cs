using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace Datos.Contextos;

public class RegisterContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Perfil> Perfiles { get; set; }

    public DbSet<Address> Address { get; set; }

    public RegisterContext(DbContextOptions<RegisterContext> options)
      : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Perfil>(builder =>
        {
            builder.ToTable("profiles");
            builder.HasKey(per => per.Id_Profiles);
            builder.Property(per => per.Id_Profiles).ValueGeneratedOnAdd();

            builder.Property(per => per.Id_Profiles).HasColumnName("Id_Profiles");
            builder.Property(per => per.Name).HasColumnName("Name");
            builder.Property(per => per.Type_User).HasColumnName("Type_User");
            builder.Property(per => per.Description).HasColumnName("Description");
        });

        modelBuilder.Entity<Usuario>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(usr => usr.Id_user);
            builder.Property(usr => usr.Id_user).ValueGeneratedOnAdd();

            builder.Property(usr => usr.Id_user).HasColumnName("Id_User");
            builder.Property(usr => usr.Pass).HasColumnName("Hashed_Password");
            builder.Property(usr => usr.Login).HasColumnName("Login");
            builder.Property(usr => usr.Name).HasColumnName("Name");
            builder.Property(usr => usr.User_Types).HasColumnName("User_Types");
            builder.Property(usr => usr.Email).HasColumnName("Email");
            builder.Property(usr => usr.Active).HasColumnName("Active");
            builder.Property(usr => usr.Register_Date).HasColumnName("Register_Date");
            builder.Property(usr => usr.Birthday_Date).HasColumnName("Birthday_Date");
            builder.Property(usr => usr.Last_Login).HasColumnName("Last_Login");

            //
            //  configuramos la relacion Perfil *----* Usuario
            builder
            .HasMany(usr => usr.Perfiles) // <-- parado en Perfil
            .WithMany()
            .UsingEntity<Dictionary<string, object>>("Relacion_Perfiles_Usuarios",
              right => right.HasOne<Perfil>().WithMany().HasForeignKey("Profiles_Id"),
              left => left.HasOne<Usuario>().WithMany().HasForeignKey("User_Id"))
            .ToTable("users_profiles");

            //  configuramos la relacion usuario ----* Address
            builder.HasMany(usr => usr.Address) // Aquí asumimos que la propiedad en Usuario se llama Addresses
                   .WithOne(addr => addr.User) // Asumiendo que hay una propiedad Usuario en la entidad Address
                   .HasForeignKey(addr => addr.Id_user); // Asumiendo que la propiedad de clave foránea en Address es UsuarioId
        });

        modelBuilder.Entity<Address>(bulder => {
            bulder.ToTable("address_users");
            bulder.HasKey(adr => adr.Id_address);
            bulder.Property(adr => adr.Id_address).ValueGeneratedOnAdd();

            bulder.Property(adr => adr.Id_address).HasColumnName("Id_Address");
            bulder.Property(adr => adr.Id_user).HasColumnName("Id_User");
            bulder.Property(adr => adr.Line_1).HasColumnName("Line_1");
            bulder.Property(adr => adr.Line_2).HasColumnName("Line_2");
            bulder.Property(adr => adr.Zip_code).HasColumnName("Zip_Code");
            bulder.Property(adr => adr.City).HasColumnName("City");
            bulder.Property(adr => adr.State).HasColumnName("State");
            bulder.Property(adr => adr.Country).HasColumnName("Country");
            bulder.Property(adr => adr.Message).HasColumnName("Message");
            bulder.Property(adr => adr.Verificated).HasColumnName("Verificated");
        });
    }
}