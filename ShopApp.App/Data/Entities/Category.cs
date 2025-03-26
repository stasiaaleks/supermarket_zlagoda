namespace ShopApp.Data.Entities;

public interface ICategory
{
    int Number { get; set; }
    string Name { get; set; }
}

public class Category : ICategory
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
}