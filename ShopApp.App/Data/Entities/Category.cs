namespace ShopApp.Data.Entities; public interface ICategory
{
    int CategoryNumber { get; set; }
    string CategoryName { get; set; }
}
public class Category : ICategory
{
    public int CategoryNumber { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
