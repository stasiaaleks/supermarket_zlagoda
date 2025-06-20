SELECT e.*, CASE WHEN u.user_id IS NULL THEN FALSE
               ELSE TRUE
            END AS has_account
FROM employee e
LEFT JOIN "user" u ON e.id_employee = u.id_employee
WHERE e.id_employee=@Id;