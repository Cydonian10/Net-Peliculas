namespace PeliculasApi.Database.Entities;

public class SalaDeCine:IId
{
  public int Id { get; set; }
  public string? Nombre { get; set; }
  public double Latitud {get; set;}
  public double Longitud {get; set;}
  public virtual List<PeliculasSalasDeCine>? PeliculasSalasDeCines { get; set; }
}
