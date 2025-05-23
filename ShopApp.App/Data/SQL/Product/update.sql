UPDATE product
SET category_number = @CategoryNumber,
    product_name = @ProductName,
    characteristics = @Characteristics
WHERE id_product = @Id;
