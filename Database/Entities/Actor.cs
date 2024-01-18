using System.ComponentModel.DataAnnotations;
using PeliculasApi.Database.Entities;

namespace PeliculasApi;

public class Actor:IId
{
  public int Id { get; set; }

  [Required]
  [MaxLength(120)]
  public string? Nombre { get; set; }
  public DateTime FechaNacimiento { get; set; }
  public string? Foto { get; set; }
  public virtual List<PeliculasActores>? Peliculas { get; set; }
}
