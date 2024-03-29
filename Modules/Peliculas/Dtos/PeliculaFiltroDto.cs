﻿namespace PeliculasApi.Dtos;

public class PeliculaFiltroDto
{
  public int Pagina { get; set; } = 1;
  public int CantidadRegistrosPorPagina { get; set; } = 10;
  public PaginarDto Paginacion
  {
    get { return new PaginarDto() { Pagina = Pagina, CantidadRegistrosPorPagina = CantidadRegistrosPorPagina }; }
  }

  public string? Titulo { get; set; }
  public int GeneroId { get; set; }
  public bool EnCines { get; set; }
  public bool ProximosEstrenos { get; set; }
  public string? CampoOrdenar { get; set; }
  public bool OrdenAscendente { get; set; } = true;
}
