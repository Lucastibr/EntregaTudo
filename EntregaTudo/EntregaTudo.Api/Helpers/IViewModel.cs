using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EntregaTudo.Api.Helpers;

public interface IViewModel
{
    Guid? Id { get; set; }

    bool IsValid(IServiceProvider serviceProvider, ModelStateDictionary modelState);

    Task BindAsync(IServiceProvider serviceProvider);
}