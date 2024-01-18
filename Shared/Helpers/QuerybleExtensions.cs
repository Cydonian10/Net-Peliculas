using PeliculasApi.Dtos;

namespace PeliculasApi.Shared.Helpers;

public static class QuerybleExtensions
{
  public static IQueryable<T> Paginar<T>(this IQueryable<T> queryble, PaginarDto paginarDto)
  {
    return queryble
      .Skip((paginarDto.Pagina - 1) * paginarDto.cantidadRegistrosPorPagina)
      .Take(paginarDto.cantidadRegistrosPorPagina);
  }
}
