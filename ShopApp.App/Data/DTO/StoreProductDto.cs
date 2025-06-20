namespace ShopApp.Data.DTO;


public class StoreProductInfoDto {
    public string? UPC { get; set; }
    public decimal SellingPrice { get; set; }
    public int ProductsNumber { get; set; }
    public string ProductName { get; set; }
    public string Characteristics { get; set; }
    public bool PromotionalProduct { get; set; }
}

public class SaveStoreProductDto {
    public string UPC { get; set; }   
    public string? UPCProm { get; set; }  
    public int IdProduct { get; set; }  
    public decimal SellingPrice { get; set; }  
    public int ProductsNumber { get; set; } 
    public bool PromotionalProduct { get; set; } 
}


public class StoreProductPriceNumberDto
{
    public string? UPC { get; set; }
    public decimal SellingPrice { get; set; }
    public int ProductsNumber { get; set; }
}
