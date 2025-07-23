using System.Linq.Expressions;
using Firebase_Auth.Common.Filters;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, DynamicFilter? filter)
    {
        if (filter == null) return query;

        var predicate = BuildPredicate<T>(filter);
        return query.Where(predicate);
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string? sortBy, string sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy)) return query;

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? property;
        try
        {
            property = GetNestedProperty(parameter, sortBy);
        }
        catch
        {
            // Invalid property name
            return query;
        }

        var lambda = Expression.Lambda(property, parameter);
        var methodName = sortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase)
            ? "OrderByDescending"
            : "OrderBy";

        var methodCall = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.Type },
            query.Expression,
            Expression.Quote(lambda)
        );

        return query.Provider.CreateQuery<T>(methodCall);
    }

    private static Expression<Func<T, bool>> BuildPredicate<T>(DynamicFilter filter)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var body = BuildExpression(parameter, filter);

        if (body == null)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Constant(true), parameter);
        }

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression? BuildExpression(ParameterExpression param, DynamicFilter filter)
    {
        if (filter.Filters != null && filter.Filters.Any())
        {
            var expressions = filter.Filters
                                    .Select(f => BuildExpression(param, f))
                                    .Where(e => e != null)
                                    .ToList();

            if (!expressions.Any()) return null;

            return expressions.Aggregate((current, next) =>
                filter.Logic?.ToUpper() == "OR"
                    ? Expression.OrElse(current!, next!)
                    : Expression.AndAlso(current!, next!));
        }

        if (string.IsNullOrEmpty(filter.Field) || string.IsNullOrEmpty(filter.Operator))
        {
            return null;
        }

        Expression? property;
        try
        {
            property = GetNestedProperty(param, filter.Field);
        }
        catch
        {
            return null;
        }

        var value = ConvertValue(filter.Value, property.Type);
        if (value == null) return null;

        var constant = Expression.Constant(value, property.Type);

        return filter.Operator.ToLower() switch
        {
            "==" or "eq" => Expression.Equal(property, constant),
            "!=" or "ne" => Expression.NotEqual(property, constant),
            ">" or "gt" => Expression.GreaterThan(property, constant),
            ">=" or "gte" => Expression.GreaterThanOrEqual(property, constant),
            "<" or "lt" => Expression.LessThan(property, constant),
            "<=" or "lte" => Expression.LessThanOrEqual(property, constant),
            "contains" when property.Type == typeof(string) =>
                Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant),
            "startswith" when property.Type == typeof(string) =>
                Expression.Call(property, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, constant),
            "endswith" when property.Type == typeof(string) =>
                Expression.Call(property, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, constant),
            _ => null
        };
    }

    private static Expression GetNestedProperty(Expression parameter, string propertyPath)
    {
        var properties = propertyPath.Split('.');
        Expression current = parameter;

        foreach (var prop in properties)
        {
            current = Expression.Property(current, prop);
        }

        return current;
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        try
        {
            if (value == null) return null;

            var nonNullableType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (nonNullableType.IsEnum)
                return Enum.Parse(nonNullableType, value.ToString()!, ignoreCase: true);

            return Convert.ChangeType(value, nonNullableType);
        }
        catch
        {
            return null;
        }
    }
}
