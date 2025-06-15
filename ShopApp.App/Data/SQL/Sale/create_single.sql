INSERT INTO sale (upc, check_number, product_number, selling_price)
VALUES (@UPC, @CheckNumber, @ProductNumber, @SellingPrice)
RETURNING upc;
