using ShopApp.Data.Entities;

namespace ShopApp.Data.DTO;

public class CategoryDto: ICategory
{
    public int CategoryNumber { get; set; }
    public string CategoryName { get; set; }
}