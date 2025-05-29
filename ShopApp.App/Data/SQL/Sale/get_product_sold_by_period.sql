SELECT SUM(s.product_number) AS total_sold
FROM sale s
         JOIN "check" c ON s.check_number = c.check_number
WHERE s.upc = @UPC
  AND c.print_date BETWEEN @StartDate AND @EndDate;
