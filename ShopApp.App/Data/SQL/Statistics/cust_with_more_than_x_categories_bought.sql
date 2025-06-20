SELECT
    cc.cust_surname,
    cc.cust_name,
    COUNT(DISTINCT cat.category_name) AS category_count,
    SUM(s.product_number) AS total_items_bought
FROM customer_card cc
         JOIN "check" c ON c.card_number = cc.card_number
         JOIN sale s ON s.check_number = c.check_number
         JOIN store_product sp ON s.upc = sp.upc
         JOIN product p ON sp.id_product = p.id_product
         JOIN category cat ON p.category_number = cat.category_number
GROUP BY cc.card_number, cc.cust_surname, cc.cust_name
HAVING COUNT(DISTINCT cat.category_name) >= @minCategories
ORDER BY category_count DESC, total_items_bought DESC;