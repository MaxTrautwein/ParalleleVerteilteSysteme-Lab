using System.ComponentModel.DataAnnotations;

namespace Backend;

public class ItemValidator : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var item = context.GetArgument<Item>(0);
        
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(item);

        if (!Validator.TryValidateObject(item, validationContext, validationResults, true))
        {
            return ValueTask.FromResult<object?>(Results.BadRequest(validationResults));
        }

        return next(context);
    }
}