using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Database;
using PeliculasApi.Database.Entities;
using PeliculasApi.Dtos;
using PeliculasApi.Images;
using PeliculasApi.Shared.Helpers;

namespace PeliculasApi.Controllers;

[ApiController]
[Route("api/peliculas")]
public class PeliculasController : CustomBaseController
{
  private readonly DataContext context;
  private readonly IMapper mapper;
  private readonly IAlmacenadorArchivos almacenadorArchivos;

  public PeliculasController(DataContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos) : base(context, mapper)
  {
    this.context = context;
    this.mapper = mapper;
    this.almacenadorArchivos = almacenadorArchivos;
  }

  [HttpGet]
  public async Task<ActionResult<List<PeliculaDto>>> List()
  {
    var peliculasDB = await context.Peliculas.ToListAsync();
    return mapper.Map<List<PeliculaDto>>(peliculasDB);
  }

  [HttpGet("top-estrenos")]
  public async Task<ActionResult<PeliculaTopEstrenoDto>> ListTop()
  {
    var top = 5;
    var hoy = DateTime.Today;

    var proximosEstrenos = await context.Peliculas
            .Where(x => x.FechaEstreno > hoy)
            .OrderBy(x => x.FechaEstreno)
            .Take(top)
            .ToListAsync();

    var enCines = await context.Peliculas
            .Where(x => x.EnCines)
            .OrderBy(x => x.FechaEstreno)
            .ToListAsync();

    var resultado = new PeliculaTopEstrenoDto()
    {
      EnCines = mapper.Map<List<PeliculaDto>>(enCines),
      FuturosEstrenos = mapper.Map<List<PeliculaDto>>(proximosEstrenos),
    };

    return resultado;
  }

  [HttpGet("filtro")]
  public async Task<ActionResult<List<PeliculaDto>>> ListFiltro([FromQuery] PeliculaFiltroDto filtroDto)
  {
    var peliculaQueryble = context.Peliculas.AsQueryable();

    if (!string.IsNullOrEmpty(filtroDto.Titulo))
    {
      peliculaQueryble = peliculaQueryble.Where(x => x.Titulo!.Contains(filtroDto.Titulo!));
    }
    if (filtroDto.ProximosEstrenos)
    {
      var hoy = DateTime.Today;
      peliculaQueryble = peliculaQueryble.Where(x => x.FechaEstreno > hoy);
    }
    if (filtroDto.EnCines)
    {
      peliculaQueryble = peliculaQueryble.Where(x => x.EnCines);
    }
    if (filtroDto.GeneroId != 0)
    {
      peliculaQueryble = peliculaQueryble.Where(x => x.Generos!.Select(y => y.GeneroId).Contains(filtroDto.GeneroId));
    }
    // if (!string.IsNullOrEmpty(filtroDto.CampoOrdenar))
    // {
    //   var tipoOrder = filtroDto.OrdenAscendente ? "ascending":"descending";
    //   peliculaQueryble = peliculaQueryble.OrderBy($"{filtroPeliculasDto.CampoOrdenar} {tipoOrden}");
    // }

    await HttpContext.InsertarParametrosPaginar(peliculaQueryble, filtroDto.CantidadRegistrosPorPagina);

    var peliculas = await peliculaQueryble.Paginar(filtroDto.Paginacion).ToListAsync();

    return mapper.Map<List<PeliculaDto>>(peliculas);
  }

  [HttpGet("{id:int}", Name = "ObtnerPelicula")]
  public async Task<ActionResult<PeliculaDetalleDto>> GetOne([FromRoute] int id)
  {
    var peliculaDB = await context.Peliculas.
                                Include(x =>x.Actores!).
                                ThenInclude(x => x.Actor).
                                Include(x =>x.Generos!).
                                ThenInclude(x => x.Genero).
                                FirstOrDefaultAsync(x => x.Id == id);

    if (peliculaDB == null) { return NotFound(); }

    return mapper.Map<PeliculaDetalleDto>(peliculaDB);
  }


  [HttpPost]
  public async Task<ActionResult> Create([FromForm] PeliculaCreatedto peliculaCreatedto)
  {
    var pelicula = mapper.Map<Pelicula>(peliculaCreatedto);


    if (peliculaCreatedto.Poster != null)
    {
      using (var ms = new MemoryStream())
      {
        await peliculaCreatedto.Poster.CopyToAsync(ms);
        var contendio = ms.ToArray();
        var extension = Path.GetExtension(peliculaCreatedto.Poster.FileName);
        var contentType = peliculaCreatedto.Poster.ContentType;
        pelicula.Poster = await almacenadorArchivos.SaveImage(contendio, extension, "peliculas", contentType);
      }
    }

    await context.Peliculas.AddAsync(pelicula);
    await context.SaveChangesAsync();

    var peliculaDto = mapper.Map<PeliculaDto>(pelicula);

    return new CreatedAtRouteResult("ObtnerPelicula", new { id = pelicula.Id }, peliculaDto);
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult> Update([FromRoute] int id, [FromForm] PeliculaCreatedto peliculaCreatedto)
  {
    var peliculaDB = await context.Peliculas
      .Include(x => x.Generos)
      .Include(x => x.Actores)
      .FirstOrDefaultAsync(x => x.Id == id);

    if (peliculaDB == null) { return NotFound(); }

    mapper.Map(peliculaCreatedto, peliculaDB);

    if (peliculaCreatedto.Poster != null)
    {
      using (var ms = new MemoryStream())
      {
        await peliculaCreatedto.Poster.CopyToAsync(ms);
        var contendio = ms.ToArray();
        var extension = Path.GetExtension(peliculaCreatedto.Poster.FileName);
        var contentType = peliculaCreatedto.Poster.ContentType;
        peliculaDB.Poster = await almacenadorArchivos.UpdateImage(contendio, extension, "peliculas", contentType, peliculaDB.Poster!);
      }
    }

    await context.SaveChangesAsync();

    return NoContent();
  }

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> Delete([FromRoute] int id)
  {
    return await Delete<Pelicula>(id);
  }
}
