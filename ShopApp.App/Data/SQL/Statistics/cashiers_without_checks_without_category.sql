SELECT e.surname, e.name, e.role
FROM employee e
WHERE e.role = 'Cashier'
  AND EXISTS (
    SELECT 1
    FROM "check" c
    WHERE c.id_employee = e.id_employee
)
  AND NOT EXISTS (
    SELECT 1
    FROM "check" c
    WHERE c.id_employee = e.id_employee
      AND NOT EXISTS (
        SELECT 1
        FROM sale s
                 JOIN store_product sp ON s.upc = sp.upc
                 JOIN product p ON sp.id_product = p.id_product
                 JOIN category cat ON p.category_number = cat.category_number
        WHERE s.check_number = c.check_number
          AND cat.category_name = :categoryName
    )
);
