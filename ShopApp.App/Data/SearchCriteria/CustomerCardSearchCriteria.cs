using ShopApp.DAL.Queries;

namespace ShopApp.Data.SearchCriteria;

public class CustomerCardSearchCriteria : DAL.Queries.SearchCriteria
{
    public string? PhoneNumber { get; set; }
    public string? CustSurname { get; set; }
    public int? Percent { get; set; }

    public CustomerCardSearchCriteria() 
        : base(defaultOrderByField: "cust_surname",
            new Dictionary<string, string>
            {
                { nameof(CustSurname), "cust_surname" },
                { nameof(PhoneNumber), "phone_number" },
                { nameof(Percent), "percent" }
            }
        )
    { }
    
    public override IPredicate ToPredicate()
    {
        // TODO: add clean search by surname
        var predicate = new SqlPredicate();

        if (!string.IsNullOrWhiteSpace(PhoneNumber))
        {
            predicate.And(
                $"{ToTableFieldMap.GetValueOrDefault(nameof(PhoneNumber))} ILIKE @{nameof(PhoneNumber)}",
                nameof(PhoneNumber),
                $"%{PhoneNumber}%"
            );
        }

        if (Percent == null) return predicate;
        
        predicate.And(
            $"{ToTableFieldMap.GetValueOrDefault(nameof(Percent))} = @{nameof(Percent)}",
            nameof(Percent),
            Percent
        );
        predicate.Parameters[nameof(Percent)] = Percent;

        return predicate;
    }
}