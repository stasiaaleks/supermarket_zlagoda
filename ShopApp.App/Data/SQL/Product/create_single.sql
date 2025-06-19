INSERT INTO product (category_number, product_name, characteristics, manufacturer)
VALUES (@CategoryNumber, @ProductName, @Characteristics, @Manufacturer)
RETURNING id_product;
