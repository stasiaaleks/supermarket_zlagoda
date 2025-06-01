namespace ShopApp.DAL.Queries;

public interface ISearchCriteria
{
    string? SortBy { get; set; }
    string? SortOrder { get; set; }
    string OrderByField { get; init; }
    IPredicate ToPredicate();
}

