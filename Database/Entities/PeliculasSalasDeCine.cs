namespace PeliculasApi.Database.Entities;

public class PeliculasSalasDeCine
{
  public int SalaDeCineId { get; set; }
  public int PeliculaId { get; set; }
  public Pelicula? Pelicula { get; set; }
  public SalaDeCine? SalaDeCine { get; set; }
}
