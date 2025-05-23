SELECT *
FROM product
WHERE category_number = @CategoryNumber
ORDER BY product_name;
