using Microsoft.EntityFrameworkCore;

namespace PeliculasApi.Shared.Helpers;

public static class HttpContextExtensions
{
  public static async Task InsertarParametrosPaginar<T>(this HttpContext httpContext, IQueryable<T> queryble, int cantidadRegistroPorPagina)
  {
    double cantidad = await queryble.CountAsync();
    double cantidadPaginas = Math.Ceiling(cantidad / cantidadRegistroPorPagina);

    httpContext.Response.Headers.Append("cantidadPaginas", cantidadPaginas.ToString());
  }
}
