using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace ShopApp.DAL.Queries;

public interface ISearchCriteria
{
    string? SortBy { get; set; }
    string? SortOrder { get; set; }
    string OrderByField { get; }
    Dictionary<string, string> ToTableFieldMap { get; }
    IPredicate ToPredicate();
}


public class SearchCriteria: ISearchCriteria
{
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    [BindNever] public virtual string OrderByField => 
        _orderByField ?? GetOrderByField(SortBy, _defaultOrderByField);
    [BindNever] public virtual Dictionary<string, string> ToTableFieldMap { get; }

    private string? _orderByField;
    private readonly string _defaultOrderByField;
    
    public SearchCriteria(string defaultOrderByField, Dictionary<string, string> toTableFieldMap)
    {
        ToTableFieldMap = toTableFieldMap;
        _defaultOrderByField = defaultOrderByField;
    } 
    
    public virtual IPredicate ToPredicate()
    {
        var predicate = new SqlPredicate();

        foreach (var (propertyName, columnName) in ToTableFieldMap)
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

    protected string GetOrderByField(string? field, string defaultField)
    {
        return string.IsNullOrWhiteSpace(field)
            ? defaultField
            : ToTableFieldMap.GetValueOrDefault(field, defaultField);
    }
}



