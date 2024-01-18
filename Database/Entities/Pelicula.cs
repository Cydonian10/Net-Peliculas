namespace PeliculasApi.Database.Entities;

public class Pelicula:IId
{
  public int Id { get; set; }
  public string? Titulo { get; set; }
  public bool EnCines { get; set; }
  public DateTime FechaEstreno { get; set; }
  public string? Poster { get; set; }

  public virtual List<PeliculasActores>? Actores { get; set; }
  public virtual List<PeliculasGeneros>? Generos { get; set; }
  public virtual List<PeliculasSalasDeCine>? PeliculasSalasDeCines { get; set; }

}
