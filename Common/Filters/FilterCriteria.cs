using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Firebase_Auth.Common.Filters;

public class DynamicFilter
{
    public string? Field { get; set; }
    public string? Operator { get; set; }
    public object? Value { get; set; }
    public string? Logic { get; set; }
    public List<DynamicFilter>? Filters { get; set; }
}


public class FilterRequest : PaginationFilter
{
    [ModelBinder(BinderType = typeof(JsonQueryModelBinder))]
    [FromQuery(Name = "filters")]
    public List<DynamicFilter> Filters { get; set; } = [];
    [FromQuery(Name = "sortBy")]
    public string? SortBy { get; set; }
    [FromQuery(Name = "sortDirection")]
    public string SortDirection { get; set; } = "ASC"; // ASC or DESC
    [FromQuery(Name = "search")]
    public string? Search { get; set; }
}


public class JsonQueryModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider
            .GetValue(bindingContext.FieldName ?? bindingContext.ModelName).FirstValue;

        if (string.IsNullOrEmpty(value))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        try
        {
            var result = JsonConvert.DeserializeObject(value, bindingContext.ModelType);
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (Exception ex)
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"Invalid JSON: {ex.Message}");
            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}

