SELECT
    s.upc,
    s.product_number,
    s.selling_price,
    p.product_name
FROM sale s
         JOIN store_product sp ON s.upc = sp.upc
         JOIN product p ON sp.id_product = p.id_product
WHERE s.check_number = @CheckNumber;
