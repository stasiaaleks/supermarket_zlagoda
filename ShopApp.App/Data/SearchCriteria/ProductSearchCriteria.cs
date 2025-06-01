using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopApp.DAL.Queries;

namespace ShopApp.Data.SearchCriteria;

public class ProductSearchCriteria : ISearchCriteria
{
    public string? ProductName { get; set; }
    public string? CategoryName { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    
    [BindNever]
    public string OrderByField { get; init; }

    private readonly Dictionary<string, string> _toTableFieldMap = new()
    {
        { nameof(ProductName), "product_name" },
        { nameof(CategoryName), "category_name" }
    };

    public ProductSearchCriteria() => OrderByField = GetOrderByField(SortBy);
    
    public IPredicate ToPredicate()
    {
        var predicate = new SqlPredicate();

        foreach (var (propertyName, columnName) in _toTableFieldMap)
        {
            var property = GetType().GetProperty(propertyName);
            var value = property?.GetValue(this) as string;

            if (!string.IsNullOrWhiteSpace(value))
            {
                predicate.And($"{columnName} ILIKE @{propertyName}", propertyName, $"%{value}%");
            }
        }

        return predicate;
    }

    private string GetOrderByField(string? field)
    {
        var defaultField = "product_name";
        return string.IsNullOrWhiteSpace(field) ? defaultField : _toTableFieldMap.GetValueOrDefault(field, defaultField);
    }
}