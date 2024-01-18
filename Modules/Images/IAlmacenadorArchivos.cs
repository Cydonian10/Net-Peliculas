namespace PeliculasApi.Images;

public interface IAlmacenadorArchivos
{
  Task<string> SaveImage(byte[] contendio, string extension, string contenedor, string contentType);
  Task<string> UpdateImage(byte[] contendio, string extension, string contenedor, string contentType, string ruta);
  Task DeleteImage(string ruta, string contenedor);
}
