using System.Linq.Expressions;
using Firebase_Auth.Common.Filters;
namespace Firebase_Auth.Common.Extensions;

public static class QueryableExtensions
{
    private static readonly Dictionary<string, string> OperatorMappings = new()
    {
        ["eq"] = "==",
        ["ne"] = "!=",
        ["gt"] = ">",
        ["gte"] = ">=",
        ["lt"] = "<",
        ["lte"] = "<=",
        ["=="] = "==",
        ["!="] = "!=",
        [">"] = ">",
        [">="] = ">=",
        ["<"] = "<",
        ["<="] = "<="
    };

    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, DynamicFilter? filter)
    {
        if (filter == null) return query;

        var predicate = BuildPredicate<T>(filter);
        return predicate != null ? query.Where(predicate) : query;
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string? sortBy, string sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy)) return query;

        try
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = GetNestedProperty(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = sortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase)
                ? "OrderByDescending" : "OrderBy";

            var methodCall = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), property.Type },
                query.Expression,
                Expression.Quote(lambda)
            );

            return query.Provider.CreateQuery<T>(methodCall);
        }
        catch (ArgumentException)
        {
            // Invalid property name, return original query
            return query;
        }
    }

    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, string? searchTerm,
        params string[] searchableFields)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchableFields.Length == 0)
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? searchExpression = null;

        foreach (var field in searchableFields)
        {
            try
            {
                var property = GetNestedProperty(parameter, field);
                if (property.Type != typeof(string)) continue;

                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchValue = Expression.Constant(searchTerm, typeof(string));
                var containsCall = Expression.Call(property, containsMethod!, searchValue);

                searchExpression = searchExpression == null
                    ? containsCall
                    : Expression.OrElse(searchExpression, containsCall);
            }
            catch (ArgumentException)
            {
                // Skip invalid field names
                continue;
            }
        }

        if (searchExpression != null)
        {
            var searchLambda = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
            query = query.Where(searchLambda);
        }

        return query;
    }

    private static Expression<Func<T, bool>>? BuildPredicate<T>(DynamicFilter filter)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var body = BuildExpression(parameter, filter);

        return body != null ? Expression.Lambda<Func<T, bool>>(body, parameter) : null;
    }

    private static Expression? BuildExpression(ParameterExpression parameter, DynamicFilter filter)
    {
        // Handle nested filters
        if (filter.HasNestedFilters)
        {
            return BuildNestedExpression(parameter, filter);
        }

        // Handle single filter
        if (!filter.IsValidFilter) return null;

        try
        {
            var property = GetNestedProperty(parameter, filter.Field!);
            return BuildComparisonExpression(property, filter.Operator!, filter.Value);
        }
        catch (ArgumentException)
        {
            // Invalid property name
            return null;
        }
    }

    private static Expression? BuildNestedExpression(ParameterExpression parameter, DynamicFilter filter)
    {
        var expressions = filter.Filters!
            .Select(f => BuildExpression(parameter, f))
            .Where(e => e != null)
            .ToList();

        if (!expressions.Any()) return null;

        var isOrLogic = filter.Logic?.Equals("OR", StringComparison.OrdinalIgnoreCase) == true;

        return expressions.Aggregate((current, next) =>
            isOrLogic ? Expression.OrElse(current!, next!) : Expression.AndAlso(current!, next!));
    }

    private static Expression? BuildComparisonExpression(Expression property, string operatorType, object? value)
    {
        var convertedValue = ConvertValue(value, property.Type);
        if (convertedValue == null && value != null) return null;

        var constant = Expression.Constant(convertedValue, property.Type);

        return operatorType.ToLowerInvariant() switch
        {
            var op when OperatorMappings.ContainsKey(op) => BuildArithmeticComparison(property, constant, OperatorMappings[op]),
            "contains" when property.Type == typeof(string) => BuildStringMethod(property, constant, "Contains"),
            "startswith" when property.Type == typeof(string) => BuildStringMethod(property, constant, "StartsWith"),
            "endswith" when property.Type == typeof(string) => BuildStringMethod(property, constant, "EndsWith"),
            "isnull" => Expression.Equal(property, Expression.Constant(null, property.Type)),
            "isnotnull" => Expression.NotEqual(property, Expression.Constant(null, property.Type)),
            _ => null
        };
    }

    private static Expression BuildArithmeticComparison(Expression property, Expression constant, string op)
    {
        return op switch
        {
            "==" => Expression.Equal(property, constant),
            "!=" => Expression.NotEqual(property, constant),
            ">" => Expression.GreaterThan(property, constant),
            ">=" => Expression.GreaterThanOrEqual(property, constant),
            "<" => Expression.LessThan(property, constant),
            "<=" => Expression.LessThanOrEqual(property, constant),
            _ => throw new ArgumentException($"Unsupported operator: {op}")
        };
    }

    private static Expression BuildStringMethod(Expression property, Expression constant, string methodName)
    {
        var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
        return Expression.Call(property, method!, constant);
    }

    private static Expression GetNestedProperty(Expression parameter, string propertyPath)
    {
        var properties = propertyPath.Split('.');
        var current = parameter;

        foreach (var prop in properties)
        {
            current = Expression.Property(current, prop);
        }

        return current;
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value == null) return null;

        try
        {
            var nonNullableType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (nonNullableType.IsEnum)
            {
                return Enum.Parse(nonNullableType, value.ToString()!, ignoreCase: true);
            }

            if (nonNullableType == typeof(Guid))
            {
                return Guid.Parse(value.ToString()!);
            }

            return Convert.ChangeType(value, nonNullableType);
        }
        catch (Exception)
        {
            return null;
        }
    }
}