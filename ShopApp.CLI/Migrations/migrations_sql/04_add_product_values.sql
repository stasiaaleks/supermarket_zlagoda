INSERT INTO product (id_product, category_number, product_name, characteristics, manufacturer) VALUES
   (1, 1, 'Pepsi 1L', 'Carbonated drink, plastic bottle', 'PepsiCo'),
   (2, 1, 'Morshynska 0.5L', 'Still water, plastic bottle', 'Morshynska'),
   (3, 2, 'Milk 2.5% Halychyna', 'Pasteurized milk, 900 ml', 'Halychyna'),
   (4, 2, 'Strawberry Yogurt Danone', 'Fruit yogurt, 300 g', 'Danone'),
   (5, 3, 'Roshen Dark Chocolate', '70% cocoa, 90 g', 'Roshen'),
   (6, 3, 'Korivka Candies', 'Milk candies, 1 kg', 'Roshen'),
   (7, 4, 'Buckwheat 1 kg', 'Whole grain, packaged', 'Zerno Ukrainy'),
   (8, 4, 'Chumak Pasta Spirals', '500 g, hard wheat', 'Chumak'),
   (9, 5, 'Wheat Bread', 'Loaf 500 g, sliced', 'Kyivkhlib'),
   (10, 6, 'Salami Sausage', 'Dry-cured, 300 g', 'Globino')
ON CONFLICT (id_product) DO NOTHING;