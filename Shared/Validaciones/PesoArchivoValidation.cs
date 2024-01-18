using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Validaciones;

public class PesoArchivoValidation : ValidationAttribute
{
  private readonly int pesoMaximoenMegaBytes;

  public PesoArchivoValidation(int PesoMaximoenMegaBytes)
  {
    pesoMaximoenMegaBytes = PesoMaximoenMegaBytes;
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (value == null)
    {
      return ValidationResult.Success;
    }

    IFormFile? formFile = value as IFormFile;
    if (formFile == null)
    {
      return ValidationResult.Success;
    }

    var pesoMaximoEnBytes = pesoMaximoenMegaBytes * 1024 * 1024;

    if (formFile.Length > pesoMaximoEnBytes)
    {
      return new ValidationResult($"El peso del archivo no debe ser mayor a {pesoMaximoenMegaBytes} megabytes");
    }

    return ValidationResult.Success;

  }
}
