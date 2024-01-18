using System.ComponentModel.DataAnnotations;
using PeliculasApi.Validaciones;

namespace PeliculasApi.Dtos;

public class ActorCreteDto
{

  [Required]
  [MaxLength(120)]
  public string? Nombre { get; set; }
  public DateTime FechaNacimiento { get; set; }

  [PesoArchivoValidation(PesoMaximoenMegaBytes: 4)]
  [TipoArchivoValidaton(tipoArchivo: ETipoArchivo.Imagen)]
  public IFormFile? Foto { get; set; }
}
