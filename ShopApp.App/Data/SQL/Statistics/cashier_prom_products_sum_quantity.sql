SELECT
    e.id_employee,
    e.surname,
    e.name,
    e.patronymic,
    COALESCE(SUM(CASE WHEN sp.promotional_product THEN s.product_number ELSE 0 END), 0) AS promo_product_quantity,
    COALESCE(SUM(CASE WHEN sp.promotional_product THEN s.selling_price * s.product_number ELSE 0 END), 0) AS total_promo
FROM employee e
         LEFT JOIN "check" c ON e.id_employee = c.id_employee
         LEFT JOIN sale s ON c.check_number = s.check_number
         LEFT JOIN store_product sp ON s.upc = sp.upc
WHERE e.role = 'cashier'
  AND e.id_employee = @IdEmployee
GROUP BY
    e.id_employee,
    e.surname,
    e.name,
    e.patronymic;
