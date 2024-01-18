using System.ComponentModel.DataAnnotations;

namespace PeliculasApi;

public class GeneroCreateDto
{
  [Required]
  [MaxLength(120)]
  public string? Nombre { get; set; }
}
