using ShopApp.DAL.Queries;

namespace ShopApp.Data.SearchCriteria;

public class StoreProductSearchCriteria : DAL.Queries.SearchCriteria
{
    public string? ProductName { get; set; }
    public int? ProductsNumberFrom { get; set; }
    public int? ProductsNumberTo { get; set; }
    public int? ProductsNumber { get; set; }

    public StoreProductSearchCriteria() 
        : base(defaultOrderByField: "products_number",
            new Dictionary<string, string>
            {
                { nameof(ProductName), "product_name" },
                { nameof(ProductsNumber), "products_number" },
                { nameof(ProductsNumberFrom), "products_number" },
                { nameof(ProductsNumberTo), "products_number" }
            }
        )
    { }
    
    public override IPredicate ToPredicate()
    {
        var predicate = new SqlPredicate();

        if (!string.IsNullOrWhiteSpace(ProductName))
        {
            predicate.And(
                $"{ToTableFieldMap.GetValueOrDefault(nameof(ProductName))} ILIKE @{nameof(ProductName)}",
                nameof(ProductName),
                $"%{ProductName}%"
            );
        }

        if (ProductsNumberFrom == null || ProductsNumberTo == null) return predicate;
        
        predicate.And(
            $"{ToTableFieldMap.GetValueOrDefault(nameof(ProductsNumber))} >= @{nameof(ProductsNumberFrom)}",
            nameof(ProductsNumber),
            ProductsNumberFrom
        );
        predicate.And(
            $"{ToTableFieldMap.GetValueOrDefault(nameof(ProductsNumber))} <= @{nameof(ProductsNumberTo)}",
            nameof(ProductsNumber),
            ProductsNumberTo
        );
        predicate.Parameters[nameof(ProductsNumberTo)] = ProductsNumberTo;
        predicate.Parameters[nameof(ProductsNumberFrom)] = ProductsNumberFrom;

        return predicate;
    }
}