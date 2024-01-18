using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PeliculasApi.Database.Entities;
using PeliculasApi.Dtos;

namespace PeliculasApi;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    // * Auth 
    CreateMap<IdentityUser, UsuarioDto>();

    // * Salas de Cine
    CreateMap<SalaDeCine,SalaDeCineDto>().ReverseMap();
    CreateMap<SalaCreacionDto, SalaDeCine>();

    // * Generos 

    CreateMap<Genero, GeneroDto>().ReverseMap();
    CreateMap<GeneroCreateDto, Genero>();

    // * Actores 

    CreateMap<Actor, ActorDto>().ReverseMap();
    CreateMap<ActorCreteDto, Actor>()
      .ForMember(x => x.Foto, options => options.Ignore());
    CreateMap<ActorPatchDto, Actor>().ReverseMap();

    // * Peliculas

    CreateMap<Pelicula, PeliculaDto>().ReverseMap();
    CreateMap<PeliculaCreatedto, Pelicula>()
      .ForMember(x => x.Poster, options => options.Ignore())
      .ForMember(x => x.Generos, options => options.MapFrom(MapPeliculaPeliculasGeneros))
      .ForMember(x => x.Actores, options => options.MapFrom(MapPeliculaPeliculasActores));
    CreateMap<Pelicula,PeliculaDetalleDto>()
      .ForMember(x => x.Generos, options => options.MapFrom(MapPeliculasGeneros))
      .ForMember(x => x.Actores, options => options.MapFrom(MapPeliculasActores));
  }

  private List<GeneroDto> MapPeliculasGeneros(Pelicula pelicula, PeliculaDetalleDto detalleDto)
  {
      var resultado = new List<GeneroDto>();

      if(pelicula.Generos == null) { return  resultado; }

      foreach(var genero in pelicula.Generos)
      {
        resultado.Add( new GeneroDto() { Id = genero.GeneroId, Nombre = genero.Genero!.Nombre});
      }

      return resultado;
  }

  private List<PeliculaActorDetalleDto> MapPeliculasActores(Pelicula pelicula, PeliculaDetalleDto detalleDto)
  {
      var resultado = new List<PeliculaActorDetalleDto>();

      if(pelicula.Actores == null) { return  resultado; }

      foreach(var actor in pelicula.Actores)
      {
        resultado.Add( new PeliculaActorDetalleDto() { ActorId = actor.ActorId , Nombre = actor.Actor!.Nombre, Personaje = actor.Personaje });
      }

      return resultado;
  }

    private List<PeliculasActores> MapPeliculaPeliculasActores(PeliculaCreatedto createdto, Pelicula pelicula)
  {
    var resultado = new List<PeliculasActores>();

    if (createdto.Actores == null) { return resultado; }

    foreach (var Actor in createdto.Actores)
    {
      resultado.Add(new PeliculasActores() { ActorId = Actor.ActorId, Orden = Actor.Orden, Personaje = Actor.Personaje });
    }

    return resultado;
  }

  private List<PeliculasGeneros> MapPeliculaPeliculasGeneros(PeliculaCreatedto createdto, Pelicula pelicula)
  {

    var resultado = new List<PeliculasGeneros>();

    if (createdto.GenerosIds == null) { return resultado; }

    foreach (var id in createdto.GenerosIds)
    {
      resultado.Add(new PeliculasGeneros() { GeneroId = id });
    }

    return resultado;
  }
}
