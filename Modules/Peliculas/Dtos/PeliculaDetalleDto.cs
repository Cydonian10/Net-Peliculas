namespace PeliculasApi.Dtos;

public class PeliculaDetalleDto:PeliculaDto
{
  public List<GeneroDto>? Generos { get; set; }
  public List<PeliculaActorDetalleDto>? Actores { get; set; }
}
