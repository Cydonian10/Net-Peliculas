namespace PeliculasApi.Dtos;

public class ActorPeliculaCreateDto
{
  public int ActorId { get; set; }
  public string? Personaje { get; set; }
  public int Orden { get; set; }

}
