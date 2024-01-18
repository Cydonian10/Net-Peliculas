using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace PeliculasApi.Shared.Helpers;

// ! Para modelar los datos en un Form Form 
public class TypeBinder<T> : IModelBinder
{
  public Task BindModelAsync(ModelBindingContext bindingContext)
  {
    var nameProperty = bindingContext.ModelName;
    var proveedorDeValores = bindingContext.ValueProvider.GetValue(nameProperty);

    if (proveedorDeValores == ValueProviderResult.None)
    {
      return Task.CompletedTask;
    }

    try
    {
      var valorDeserializado = JsonConvert.DeserializeObject<T>(proveedorDeValores.FirstValue!);
      bindingContext.Result = ModelBindingResult.Success(valorDeserializado);
    }
    catch
    {
      bindingContext.ModelState.TryAddModelError(nameProperty, "Valor invalido para tipo list<int>");
    }

    return Task.CompletedTask;
  }
}
