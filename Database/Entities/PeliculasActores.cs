namespace PeliculasApi.Database.Entities;

public class PeliculasActores
{
  public int PeliculaId { get; set; }
  public int ActorId { get; set; }
  public string? Personaje { get; set; }
  public int Orden { get; set; }
  public virtual Pelicula? Pelicula { get; set; }
  public virtual Actor? Actor { get; set; }

}
