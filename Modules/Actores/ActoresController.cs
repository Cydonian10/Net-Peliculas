using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Database;
using PeliculasApi.Dtos;
using PeliculasApi.Images;
using PeliculasApi.Shared.Helpers;

namespace PeliculasApi.Controllers;

[ApiController]
[Route("api/actores")]
public class ActoresController : CustomBaseController
{
  private readonly DataContext context;
  private readonly IMapper mapper;
  private readonly IAlmacenadorArchivos almacenadorArchivos;

  public ActoresController(DataContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos) : base(context, mapper)
  {
    this.context = context;
    this.mapper = mapper;
    this.almacenadorArchivos = almacenadorArchivos;
  }

  [HttpGet]
  public async Task<ActionResult<List<ActorDto>>> List([FromQuery] PaginarDto paginarDto)
  {
    return await ListPaginacion<Actor,ActorDto>(paginarDto);
  }

  [HttpGet("{id:int}", Name = "ObtnerActor")]
  public async Task<ActionResult<ActorDto>> GetOne([FromRoute] int id)
  {
    return await GetOne<Actor,ActorDto>(id);
  }

  [HttpPost]
  public async Task<ActionResult> Create([FromForm] ActorCreteDto actorCreteDto)
  {
    var actor = mapper.Map<Actor>(actorCreteDto);

    if (actorCreteDto.Foto != null)
    {
      using (var ms = new MemoryStream())
      {
        await actorCreteDto.Foto.CopyToAsync(ms);
        var contendio = ms.ToArray();
        var extension = Path.GetExtension(actorCreteDto.Foto.FileName);
        var contentType = actorCreteDto.Foto.ContentType;
        actor.Foto = await almacenadorArchivos.SaveImage(contendio, extension, "actores", contentType);
      }
    }

    await context.Actores.AddAsync(actor);
    await context.SaveChangesAsync();

    var actorDto = mapper.Map<ActorDto>(actor);

    return new CreatedAtRouteResult("ObtnerActor", new { id = actor.Id }, actorDto);
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult> Update([FromRoute] int id, [FromForm] ActorCreteDto actorCreteDto)
  {
    var actorDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

    if (actorDB == null) { return NotFound(); }

    mapper.Map(actorCreteDto, actorDB);

    if (actorCreteDto.Foto != null)
    {
      using (var ms = new MemoryStream())
      {
        await actorCreteDto.Foto.CopyToAsync(ms);
        var contendio = ms.ToArray();
        var extension = Path.GetExtension(actorCreteDto.Foto.FileName);
        var contentType = actorCreteDto.Foto.ContentType;
        actorDB.Foto = await almacenadorArchivos.UpdateImage(contendio, extension, "actores", contentType, actorDB.Foto!);
      }
    }

    await context.SaveChangesAsync();

    return NoContent();
  }

  [HttpPatch("{id:int}")]
  public async Task<ActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<ActorPatchDto> patchDocument)
  {
    return await Patch<Actor,ActorPatchDto>(id,patchDocument);
  }

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> Delete([FromRoute] int id)
  {
    return await Delete<Actor>(id);
  }

}
