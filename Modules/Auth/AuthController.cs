using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PeliculasApi.Database;
using PeliculasApi.Dtos;
using PeliculasApi.Shared.Helpers;

namespace PeliculasApi;

[ApiController]
[Route("api/auth")]
public class AuthController : CustomBaseController
{
    private readonly DataContext context;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public AuthController(
      DataContext context, 
      IMapper mapper,
      IConfiguration configuration,
      UserManager<IdentityUser> userManager,
      SignInManager<IdentityUser> signInManager
    ) : base(context, mapper)
    {
        this.context = context;
        this.mapper = mapper;
        this.configuration = configuration;
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpPost("Crear")]
      public async Task<ActionResult<AuthTokenDto>> CreateUser([FromBody] AuthInfoDto authInfoDto)
      {
          var user = new IdentityUser { UserName = authInfoDto.Email, Email = authInfoDto.Email };
          var result = await userManager.CreateAsync(user, authInfoDto.Password!);

          if (result.Succeeded)
          {
              return await ConstruirToken(authInfoDto);
          }
          else
          {
              return BadRequest(result.Errors);
          }
      }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthTokenDto>> Login([FromBody] AuthInfoDto model)
    {
        var resultado = await signInManager.PasswordSignInAsync(model.Email!,
            model.Password!, isPersistent: false, lockoutOnFailure: false);

        if (resultado.Succeeded)
        {
            return await ConstruirToken(model);
        }
        else
        {
            return BadRequest("Invalid login attempt");
        }
    }

    [HttpPost("RenovarToken")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<AuthTokenDto>> Renovar()
    {
        var userInfo = new AuthInfoDto
        {
            Email = HttpContext.User.Identity!.Name
        };

        return await ConstruirToken(userInfo);
    }

    private async Task<AuthTokenDto> ConstruirToken(AuthInfoDto authInfoDto)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, authInfoDto.Email!),
            new Claim(ClaimTypes.Email, authInfoDto.Email!),
        };

        var identityUser = await userManager.FindByEmailAsync(authInfoDto.Email!);

        claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser!.Id));

        var claimsDB = await userManager.GetClaimsAsync(identityUser);

        claims.AddRange(claimsDB);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiracion = DateTime.UtcNow.AddYears(1);

        JwtSecurityToken token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
            expires: expiracion,
            signingCredentials: creds);

        return new AuthTokenDto()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expiracion
        };

    }

    [HttpGet("Usuarios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<List<UsuarioDto>>> Get([FromQuery] PaginarDto paginationDTO)
    {
        var queryable = context.Users.AsQueryable();
        queryable = queryable.OrderBy(x => x.Email);
        return await ListPaginacion<IdentityUser, UsuarioDto>(paginationDTO);
    }

    [HttpGet("Roles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<List<string>>> GetRoles()
    {
        return await context.Roles.Select(x => x.Name!).ToListAsync();
    }

    [HttpPost("AsignarRol")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult> AsignarRol(EditarRolDto editarRolDTO)
    {
        var user = await userManager.FindByIdAsync(editarRolDTO.UsuarioId!);
        if (user == null)
        {
            return NotFound();
        }

        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.NombreRol!));
        return NoContent();
    }

    [HttpPost("RemoveRol")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult> RemoverRol(EditarRolDto editarRolDTO)
    {
        var user = await userManager.FindByIdAsync(editarRolDTO.UsuarioId!);
        if (user == null)
        {
            return NotFound();
        }

        await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.NombreRol!));
        return NoContent();
    }

    [HttpPost("crear-admin")]
    public async Task<ActionResult> CrearAdmin()
    {
       var rolAdminId = "846c9ad1-09e7-45ad-9543-ed1c944b5d96";
       var usuarioAdminId = "070e6389-68b8-450b-a990-85a083ae1824";

       var rolAdmin = new IdentityRole()
       {
        Id = rolAdminId,
        Name = "Admin",
        NormalizedName = "Admin"
       };

      var passwordHasser = new PasswordHasher<IdentityUser>();
      var username = "mabel@hotmail.com";

      var usuarioAdmin = new IdentityUser() {
        Id = usuarioAdminId, UserName = username, NormalizedEmail= username,
        Email = username, NormalizedUserName = username,
      };
      // var PasswordHash = passwordHasser.HashPassword(null!,"Aa12345!")
      var result = await userManager.CreateAsync(usuarioAdmin,"Aa12345!");
      var user = await userManager.FindByIdAsync(usuarioAdminId);

      await userManager.AddClaimAsync(usuarioAdmin, new Claim(ClaimTypes.Role, "Admin"));


      if (result.Succeeded)
      {
          return Ok();
      }
      else
      {
          return BadRequest(result.Errors);
      }

    }
}
