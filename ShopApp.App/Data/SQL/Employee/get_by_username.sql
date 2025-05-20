SELECT * 
FROM employee
INNER JOIN "user" as u ON employee.id_employee = u.id_employee
WHERE u.username=@Username;
