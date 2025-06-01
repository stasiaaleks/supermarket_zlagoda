namespace ShopApp.DAL.Queries;

public interface IPredicate
{
    string WhereClause { get; set; }
    Dictionary<string, object?> Parameters { get; set; }
}

public class SqlPredicate: IPredicate
{
    public string WhereClause { get; set; } = "";
    public Dictionary<string, object?> Parameters { get; set; } = new();
}