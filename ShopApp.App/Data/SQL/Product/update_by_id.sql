UPDATE product
SET category_number = @CategoryNumber,
    product_name = @ProductName,
    characteristics = @Characteristics,
    manufacturer = @Manufacturer
WHERE id_product = @IdProduct
RETURNING id_product;
