using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PeliculasApi.Shared.Helpers;

namespace PeliculasApi.Dtos;

public class PeliculaCreatedto
{
  [Required]
  public string? Titulo { get; set; }

  [Required]
  public bool? EnCines { get; set; }
  public DateTime FechaEstreno { get; set; }
  public IFormFile? Poster { get; set; }


  [Required]
  [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
  public List<int>? GenerosIds { get; set; }

  [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaCreateDto>>))]
  public List<ActorPeliculaCreateDto>? Actores { get; set; }
}
