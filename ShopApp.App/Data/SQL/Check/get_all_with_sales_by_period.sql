SELECT
    c.check_number,
    c.id_employee,
    c.card_number,
    c.print_date,
    c.vat,
    c.sum_total,
    s.upc,
    s.product_number,
    s.selling_price,
    p.product_name
FROM "check" c
         JOIN sale s ON c.check_number = s.check_number
         JOIN store_product sp ON s.upc = sp.upc
         JOIN product p ON sp.id_product = p.id_product
WHERE c.print_date BETWEEN @StartDate AND @EndDate
ORDER BY c.print_date;
