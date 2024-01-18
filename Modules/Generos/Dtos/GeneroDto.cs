using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Dtos;

public class GeneroDto
{
  public int Id { get; set; }

  [MaxLength(120)]
  public string? Nombre { get; set; }


}
