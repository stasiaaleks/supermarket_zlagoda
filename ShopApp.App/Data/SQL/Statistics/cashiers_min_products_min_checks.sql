SELECT e.id_employee, e.surname, e.name, COUNT(*) AS checks_amount
FROM employee e
         INNER JOIN "check" c ON e.id_employee = c.id_employee
WHERE c.check_number IN (
    SELECT s.check_number
    FROM sale s
    GROUP BY s.check_number
    HAVING COUNT(*) >= @MinProducts
)
GROUP BY e.id_employee, e.surname, e.name
HAVING COUNT(*) >= @MinChecks;