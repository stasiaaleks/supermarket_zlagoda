SELECT SUM(c.sum_total) AS total_sum
FROM "check" c
WHERE c.print_date BETWEEN @StartDate AND @EndDate;
