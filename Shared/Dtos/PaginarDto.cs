namespace PeliculasApi.Dtos;

public class PaginarDto
{
  public int Pagina { get; set; } = 1;
  public int cantidadRegistrosPorPagina = 10;
  public readonly int cantidadMaximaRegistrosPorPagina = 50;

  public int CantidadRegistrosPorPagina
  {
    get => cantidadRegistrosPorPagina;
    set
    {
      cantidadRegistrosPorPagina = (value > 50) ? cantidadMaximaRegistrosPorPagina : value;
    }
  }
}
