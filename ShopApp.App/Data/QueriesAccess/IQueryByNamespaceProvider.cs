namespace ShopApp.Data.QueriesAccess;

public interface IQueryByNamespaceProvider
{
    // get a unique namespace to get query content by filename
    string GetNamespace(string filename);
}