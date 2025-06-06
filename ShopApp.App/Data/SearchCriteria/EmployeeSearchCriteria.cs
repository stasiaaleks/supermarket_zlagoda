using ShopApp.DAL.Queries;

namespace ShopApp.Data.SearchCriteria;

public class EmployeeSearchCriteria : DAL.Queries.SearchCriteria
{
    public string? PhoneNumber { get; set; }
    public string? Surname { get; set; }
    public string? Role { get; set; }

    public EmployeeSearchCriteria() 
        : base(defaultOrderByField: "surname",
            new Dictionary<string, string>
            {
                { nameof(Surname), "surname" },
                { nameof(PhoneNumber), "phone_number" },
                { nameof(Role), "role" }
            }
        )
    { }
    
    public override IPredicate ToPredicate()
    {
        var predicate = new SqlPredicate();

        foreach (var (key, column) in ToTableFieldMap)
        {
            var propertyInfo = GetType().GetProperty(key);
            if (propertyInfo?.GetValue(this) is not string value)
                continue;

            if (string.IsNullOrWhiteSpace(value))
                continue;

            predicate.And($"{column} ILIKE @{key}", key, $"%{value}%");
        }
        
        return predicate;
    }
}