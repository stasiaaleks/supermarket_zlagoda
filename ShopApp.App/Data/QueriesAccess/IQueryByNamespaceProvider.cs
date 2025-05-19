namespace ShopApp.Data.QueriesAccess;

public interface IQueryByNamespaceProvider
{
    string GetNamespace(string filename);
}