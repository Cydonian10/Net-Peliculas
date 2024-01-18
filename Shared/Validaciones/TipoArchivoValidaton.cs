using System.ComponentModel.DataAnnotations;
using PeliculasApi.Validaciones;

namespace PeliculasApi;

public class TipoArchivoValidaton : ValidationAttribute
{
  private readonly string[]? tiposValidos;

  public TipoArchivoValidaton(string[] tiposValidos)
  {
    this.tiposValidos = tiposValidos;
  }

  public TipoArchivoValidaton(ETipoArchivo tipoArchivo)
  {
    if (tipoArchivo == ETipoArchivo.Imagen)
    {
      tiposValidos = new string[] { "image/jpeg", "image.png", "image.gif" };
    }
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (tiposValidos == null)
    {
      return ValidationResult.Success;
    }

    if (value == null)
    {
      return ValidationResult.Success;
    }

    IFormFile? formFile = value as IFormFile;

    if (formFile == null)
    {
      return ValidationResult.Success;
    }

    if (!tiposValidos.Contains(formFile.ContentType))
    {
      return new ValidationResult($"Tipo de archivo deber ser {string.Join(", ", tiposValidos)}");
    }

    return ValidationResult.Success;

  }
}
