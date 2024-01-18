using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PeliculasApi.Dtos;
using PeliculasApi.Database;
using PeliculasApi.Shared.Helpers;
using PeliculasApi.Database.Entities;

namespace PeliculasApi.Controllers;

[ApiController]
[Route("api/salas-de-cine")]
public class SalaDeCineController : CustomBaseController
{
  private readonly DataContext context;
  private readonly IMapper mapper;

  public SalaDeCineController(DataContext context, IMapper mapper) : base(context, mapper)
  {
      this.context = context;
      this.mapper = mapper;
  }
  
  [HttpGet]
  public async Task<ActionResult<List<SalaDeCineDto>>> List()
  {
    return await List<SalaDeCine,SalaDeCineDto>();
  }

  [HttpGet("{id:int},", Name = "ObtnerSalaCine")]
  public async Task<ActionResult<SalaDeCineDto>> GetOne([FromRoute] int id)
  {
    return await GetOne<SalaDeCine,SalaDeCineDto>(id);
  }

  [HttpPost]
  public async Task<ActionResult> Create([FromBody] SalaCreacionDto salaCreacionDto)
  {
    return await Create<SalaDeCine, SalaCreacionDto, SalaDeCineDto>(salaCreacionDto,"ObtnerSalaCine");
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult> Update([FromRoute] int id, [FromBody] SalaCreacionDto salaCreacionDto)
  {
    return await Update<SalaDeCine,SalaCreacionDto>(id,salaCreacionDto);
  }

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> Delete([FromRoute] int id)
  {
    return await Delete<SalaDeCine>(id);
  }
}
