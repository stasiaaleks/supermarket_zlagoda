namespace ShopApp.DAL.Queries;

public static class SqlPredicateBuilder
{
    public static SqlPredicate And(this SqlPredicate predicate, string condition, string paramName, object? value)
    {
        if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            return predicate;

        if (!string.IsNullOrWhiteSpace(predicate.WhereClause) && !predicate.WhereClause.TrimEnd().EndsWith("("))
            predicate.WhereClause += " AND ";
        
        predicate.WhereClause += condition;
        predicate.Parameters[paramName] = value;
        
        return predicate;
    }
    
    public static SqlPredicate BeginGroup(this SqlPredicate predicate)
    {
        if (!string.IsNullOrEmpty(predicate.WhereClause))
            predicate.WhereClause += " AND (";
        else
            predicate.WhereClause += "(";
    
        return predicate;
    }
    
    public static SqlPredicate EndGroup(this SqlPredicate predicate)
    {
        predicate.WhereClause += ")";
        return predicate;
    }

    public static SqlPredicate Or(this SqlPredicate predicate, string condition, string paramName, object? value)
    {
        if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            return predicate;

        if (!string.IsNullOrEmpty(predicate.WhereClause))
            predicate.WhereClause += " OR ";
        
        predicate.WhereClause += condition;
        predicate.Parameters[paramName] = value;
        
        return predicate;
    }
}