using System.ComponentModel.DataAnnotations;

namespace Backend;

public class ItemValidator : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(arg => arg is Item) is not Item item) return next(context);
        
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(item);

        if (!Validator.TryValidateObject(item, validationContext, validationResults, true))
        {
            return ValueTask.FromResult<object?>(Results.BadRequest(validationResults));
        }

        return next(context);
    }
}