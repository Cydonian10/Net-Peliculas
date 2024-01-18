using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Dtos;

public class ActorDto
{
  public int Id { get; set; }

  [Required]
  [MaxLength(120)]
  public string? Nombre { get; set; }
  public DateTime FechaNacimiento { get; set; }
  public string? Foto { get; set; }
}
