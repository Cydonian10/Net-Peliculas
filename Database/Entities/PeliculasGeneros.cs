namespace PeliculasApi.Database.Entities;

public class PeliculasGeneros
{
  public int PeliculaId { get; set; }
  public int GeneroId { get; set; }
  public virtual Pelicula? Pelicula { get; set; }
  public virtual Genero? Genero { get; set; }
}
