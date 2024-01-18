using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Database;
using PeliculasApi.Database.Entities;
using PeliculasApi.Dtos;
using PeliculasApi.Shared.Helpers;

namespace PeliculasApi.Controllers;

[ApiController]
[Route("api/generos")]
public class GenerosController : CustomBaseController
{
  private readonly DataContext context;
  private readonly IMapper mapper;

  public GenerosController(DataContext context, IMapper mapper) : base(context, mapper)
  {
    this.context = context;
    this.mapper = mapper;
  }

  [HttpGet]
  public async Task<ActionResult<List<GeneroDto>>> List()
  {
    return await List<Genero,GeneroDto>();
  }

  [HttpGet("{id:int}", Name = "ObtenerGeneros")]
  public async Task<ActionResult<GeneroDto>> GetOne([FromRoute] int id)
  {
   return await GetOne<Genero,GeneroDto>(id);
  }

  [HttpPost]
  public async Task<ActionResult> Create([FromBody] GeneroCreateDto generoCreateDto)
  {
    return await Create<Genero,GeneroCreateDto,GeneroDto>(generoCreateDto,"ObtenerGeneros");
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult> Update([FromRoute] int id, [FromBody] GeneroCreateDto generoCreateDto)
  {
    return await Update<Genero,GeneroCreateDto>(id,generoCreateDto);
  }

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> Delete([FromRoute] int id)
  {
    return await Delete<Genero>(id);
  }

}
