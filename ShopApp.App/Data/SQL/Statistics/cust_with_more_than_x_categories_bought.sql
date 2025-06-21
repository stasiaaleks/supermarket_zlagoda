SELECT
    cc.cust_surname as surname,
    cc.cust_name as name,
    COUNT(DISTINCT cat.category_name) AS category_count,
    SUM(s.product_number) AS product_count
FROM customer_card cc
         JOIN "check" c ON c.card_number = cc.card_number
         JOIN sale s ON s.check_number = c.check_number
         JOIN store_product sp ON s.upc = sp.upc
         JOIN product p ON sp.id_product = p.id_product
         JOIN category cat ON p.category_number = cat.category_number
GROUP BY cc.card_number, cc.cust_surname, cc.cust_name
HAVING COUNT(DISTINCT cat.category_name) >= @MinCategories
ORDER BY category_count DESC, product_count DESC;