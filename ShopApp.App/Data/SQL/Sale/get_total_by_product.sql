SELECT SUM(product_number) AS total_units_sold
FROM sale
WHERE upc = @UPC;
