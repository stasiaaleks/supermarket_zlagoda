SELECT * FROM "check"
WHERE id_employee = @EmployeeId
  AND print_date BETWEEN @StartDate AND @EndDate
ORDER BY print_date;
