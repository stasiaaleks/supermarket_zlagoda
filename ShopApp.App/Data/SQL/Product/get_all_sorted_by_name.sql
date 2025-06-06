SELECT *
FROM product p
INNER JOIN category c on c.category_number = p.category_number
ORDER BY product_name;
