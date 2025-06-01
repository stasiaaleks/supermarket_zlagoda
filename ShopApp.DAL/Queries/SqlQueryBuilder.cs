namespace ShopApp.DAL.Queries;

public interface IQueryBuilder
{
    string Build(string baseQuery, ISearchCriteria criteria, IPredicate predicate);
}

public class SqlQueryBuilder : IQueryBuilder {
    public string Build(string baseQuery, ISearchCriteria criteria, IPredicate predicate)
    {
        var query = baseQuery;
        
        if (!string.IsNullOrEmpty(predicate.WhereClause))
        {
            if (query.ToUpper().Contains("WHERE"))
            {
                query += $" AND ({predicate.WhereClause})";
            }
            else
            {
                query += $" WHERE {predicate.WhereClause}";
            }
        }
        
        query += BuildOrderByClause(criteria);
        return query;
    }
    
    private string BuildOrderByClause(ISearchCriteria criteria)
    {
        var sortDirection = criteria.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";
        return $" ORDER BY {criteria.OrderByField} {sortDirection}";
    }
}