SELECT e.id_employee, e.name, e.surname, c.check_number
FROM "check" AS c
         INNER JOIN employee AS e ON c.id_employee = e.id_employee
WHERE e.surname != @Surname
  AND NOT EXISTS (
    SELECT s.upc
    FROM sale AS s
    WHERE s.check_number = c.check_number
      AND s.upc NOT IN (
        SELECT sp.upc
        FROM check AS cp
                 INNER JOIN sale AS sp ON cp.check_number = sp.check_number
        WHERE cp.id_employee IN (
            SELECT id_employee
            FROM employee
            WHERE surname = @Surname
        )
    )
)
  AND NOT EXISTS (
    SELECT cp.check_number
    FROM check AS cp
             INNER JOIN sale AS sp ON cp.check_number = sp.check_number
    WHERE cp.id_employee IN (
        SELECT id_employee
        FROM employee
        WHERE surname = @Surname
    )
      AND sp.upc NOT IN (
        SELECT s.upc
        FROM sale AS s
        WHERE s.check_number = c.check_number
    )
);
