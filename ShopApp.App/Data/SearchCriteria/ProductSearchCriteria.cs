namespace ShopApp.Data.SearchCriteria;

public class ProductSearchCriteria : DAL.Queries.SearchCriteria
{
    public string? ProductName { get; set; }
    public string? CategoryName { get; set; }

    public ProductSearchCriteria() 
        : base(defaultOrderByField: "product_name",
            new Dictionary<string, string>
                {
                    { nameof(ProductName), "product_name" },
                    { nameof(CategoryName), "category_name" }
                }
            )
    { }
}