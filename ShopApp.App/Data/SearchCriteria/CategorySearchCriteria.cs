namespace ShopApp.Data.SearchCriteria;

public class CategorySearchCriteria : DAL.Queries.SearchCriteria
{
    public string? CategoryName { get; set; }

    public CategorySearchCriteria() 
        : base(defaultOrderByField: "category_name",
            new Dictionary<string, string>
                {
                    { nameof(CategoryName), "category_name" }
                }
            )
    { }
}