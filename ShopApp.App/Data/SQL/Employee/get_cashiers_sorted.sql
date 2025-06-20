SELECT *,  CASE WHEN u.user_id IS NULL THEN FALSE
                ELSE TRUE
                END AS has_account
FROM employee
LEFT JOIN "user" u ON employee.id_employee = u.id_employee
WHERE role = 'cashier'
ORDER BY surname;