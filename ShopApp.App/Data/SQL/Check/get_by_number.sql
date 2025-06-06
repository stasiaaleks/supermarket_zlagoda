SELECT
    c.check_number,
    c.id_employee,
    c.card_number,
    c.print_date,
    c.sum_total,
    c.vat,

    p.product_name,
    p.characteristics,
    p.manufacturer,

    s.product_number AS quantity,
    s.selling_price,
    (s.product_number * s.selling_price) AS total_price_per_product

FROM "check" c
         JOIN sale s ON c.check_number = s.check_number
         JOIN store_product sp ON s.upc = sp.upc
         JOIN product p ON sp.id_product = p.id_product

WHERE c.check_number = @CheckNumber;