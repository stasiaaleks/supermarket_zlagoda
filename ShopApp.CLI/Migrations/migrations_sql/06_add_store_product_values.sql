INSERT INTO store_product (
    upc, upc_prom, id_product, selling_price, products_number, promotional_product
) VALUES
('000111', '000112', 1, 50.00, 20, FALSE),
('000112', NULL, 1, 42.00, 10, TRUE),
('000113', '000114', 2, 27.50, 15, FALSE),
('000114', NULL, 2, 22.99, 5, TRUE),
('000115', '000116', 3, 60.00, 25, FALSE),
('000116', NULL, 3, 49.99, 10, TRUE),
('000117', NULL, 4, 33.33, 12, FALSE),
('000118', NULL, 5, 25.00, 10, FALSE),
('000119', NULL, 6, 18.75, 30, FALSE),
('000120', NULL, 7, 16.40, 40, FALSE),
('000121', NULL, 8, 12.99, 50, FALSE),
('000122', NULL, 9, 9.50, 35, FALSE),
('000123', NULL, 10, 11.30, 45, FALSE)
ON CONFLICT DO NOTHING;

