using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Database.Entities;

namespace PeliculasApi.Database;

public class DataContext : IdentityDbContext
{
  public DataContext(DbContextOptions options) : base(options)
  {
  }

  public DbSet<Genero> Generos { get; set; }
  public DbSet<Actor> Actores { get; set; }
  public DbSet<Pelicula> Peliculas { get; set; }
  public DbSet<SalaDeCine> SalaDeCines { get; set; }
  public DbSet<PeliculasActores> PeliculasActores { get; set; }
  public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
  public DbSet<PeliculasSalasDeCine> PeliculasSalasDeCine { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Genero>(e =>
    {
      e.ToTable("generos");
      e.HasKey(e => e.Id);
    });

    modelBuilder.Entity<Actor>(e =>
    {
      e.ToTable("actores");
      e.HasKey(e => e.Id);
      e.Property(e => e.FechaNacimiento).HasColumnType("date");
    });

    modelBuilder.Entity<Pelicula>(e =>
    {
      e.ToTable("peliculas");
      e.HasKey(e => e.Id);
      e.Property(e => e.FechaEstreno).HasColumnType("date");
    });

    modelBuilder.Entity<PeliculasActores>(e =>
    {
      e.ToTable("peliculas_actores");
      e.HasKey(e => new { e.ActorId, e.PeliculaId });

      e.HasOne(e => e.Actor).WithMany(e => e.Peliculas).HasForeignKey(e => e.ActorId).OnDelete(DeleteBehavior.Cascade);
      e.HasOne(e => e.Pelicula).WithMany(e => e.Actores).HasForeignKey(e => e.PeliculaId).OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<PeliculasGeneros>(e =>
     {
       e.ToTable("peliculas_generos");
       e.HasKey(e => new { e.GeneroId, e.PeliculaId });
       e.HasOne(e => e.Genero).WithMany(e => e.Peliculas).HasForeignKey(e => e.GeneroId).OnDelete(DeleteBehavior.Cascade);
       e.HasOne(e => e.Pelicula).WithMany(e => e.Generos).HasForeignKey(e => e.PeliculaId).OnDelete(DeleteBehavior.Cascade);
     });

     modelBuilder.Entity<PeliculasSalasDeCine>( e => 
     {  
       e.ToTable("peliculas_salasdecine");
       e.HasKey(e => new { e.PeliculaId, e.SalaDeCineId });
       e.HasOne(e => e.Pelicula).WithMany(e => e.PeliculasSalasDeCines).HasForeignKey(e => e.PeliculaId).OnDelete(DeleteBehavior.Cascade);
       e.HasOne(e => e.SalaDeCine).WithMany(e => e.PeliculasSalasDeCines).HasForeignKey(e => e.SalaDeCineId).OnDelete(DeleteBehavior.Cascade);

     });

    base.OnModelCreating(modelBuilder);
  }
}
