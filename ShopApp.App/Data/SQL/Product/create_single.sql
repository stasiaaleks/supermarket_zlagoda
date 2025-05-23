INSERT INTO product (category_number, product_name, characteristics)
VALUES (@CategoryNumber, @ProductName, @Characteristics)
RETURNING id_product;
