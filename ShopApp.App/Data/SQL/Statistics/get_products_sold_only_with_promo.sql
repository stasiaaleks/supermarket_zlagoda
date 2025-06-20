SELECT
    sp.upc,
    p.id_product,
    p.product_name,
    SUM(s.product_number) AS total_quantity_sold
FROM product p
         JOIN store_product sp ON p.id_product = sp.id_product
         JOIN sale s ON sp.upc = s.upc
WHERE sp.upc IN (
    SELECT sp1.upc_prom
    FROM store_product sp1
    WHERE sp1.upc_prom IS NOT NULL
)
  AND p.id_product NOT IN (
    SELECT DISTINCT sp2.id_product
    FROM sale s2
             JOIN store_product sp2 ON s2.upc = sp2.upc
    WHERE sp2.upc NOT IN (
        SELECT sp3.upc_prom
        FROM store_product sp3
        WHERE sp3.upc_prom IS NOT NULL
    )
)
GROUP BY sp.upc, p.product_name, s.product_number, p.id_product;
;
