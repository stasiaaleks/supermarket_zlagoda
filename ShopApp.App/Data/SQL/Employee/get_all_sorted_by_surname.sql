SELECT employee.*, CASE WHEN u.user_id IS NULL THEN FALSE
                    ELSE TRUE
                    END AS has_account
FROM employee 
LEFT JOIN "user" u ON employee.id_employee = u.id_employee
ORDER BY surname;
