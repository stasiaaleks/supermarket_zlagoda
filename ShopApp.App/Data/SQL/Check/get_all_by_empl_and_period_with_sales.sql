SELECT
    c.check_number,
    s.upc,
    p.product_name,
    s.product_number,
    s.selling_price,
    c.vat,
    c.sum_total,
    c.card_number,
    c.print_date
FROM sale s
         JOIN store_product sp ON s.upc = sp.upc
         JOIN product p ON sp.id_product = p.id_product
         JOIN "check" c ON c.check_number = s.check_number
WHERE c.id_employee = @EmployeeId AND c.print_date BETWEEN @StartDate AND @EndDate
ORDER BY print_date;
