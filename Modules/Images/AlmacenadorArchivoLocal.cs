
namespace PeliculasApi.Images;

public class AlmacenadorArchivoLocal : IAlmacenadorArchivos
{
  private readonly IWebHostEnvironment environment;
  private readonly IHttpContextAccessor httpContextAccessor;

  public AlmacenadorArchivoLocal(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
  {
    this.environment = environment;
    this.httpContextAccessor = httpContextAccessor;
  }

  public Task DeleteImage(string ruta, string contenedor)
  {
    if (ruta != null)
    {
      var nombreArchivo = Path.GetFileName(ruta);
      string directorioArchivo = Path.Combine(environment.WebRootPath, contenedor, nombreArchivo);

      if (File.Exists(directorioArchivo))
      {
        File.Delete(directorioArchivo);
      }
    }

    return Task.FromResult(0);
  }

  public async Task<string> SaveImage(byte[] contendio, string extension, string contenedor, string contentType)
  {
    var nombreArchivo = $"{Guid.NewGuid()}{extension}";
    var folder = Path.Combine(environment.WebRootPath, contenedor);

    if (!Directory.Exists(folder))
    {
      Directory.CreateDirectory(folder);
    }

    string ruta = Path.Combine(folder, nombreArchivo);
    await File.WriteAllBytesAsync(ruta, contendio);

    var urlActual = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
    var urlForDB = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/ ");
    return urlForDB;

  }

  public async Task<string> UpdateImage(byte[] contendio, string extension, string contenedor, string contentType, string ruta)
  {
    await DeleteImage(ruta, contenedor);
    return await SaveImage(contendio, extension, contenedor, contentType);
  }
}
