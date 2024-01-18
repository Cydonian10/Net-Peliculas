using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Database.Entities;

public class Genero:IId
{

  public int Id { get; set; }

  [MaxLength(120)]
  public string? Nombre { get; set; }

  public virtual List<PeliculasGeneros>? Peliculas { get; set; }

}
