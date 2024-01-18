using System.ComponentModel.DataAnnotations;

namespace PeliculasApi;

public class ActorPatchDto
{
  [Required]
  [MaxLength(120)]
  public string? Nombre { get; set; }
  public DateTime FechaNacimiento { get; set; }
}
