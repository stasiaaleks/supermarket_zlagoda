SELECT * FROM "check"
WHERE id_employee = @EmployeeId
  AND DATE(print_date) = CURRENT_DATE;
