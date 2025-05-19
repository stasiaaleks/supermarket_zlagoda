INSERT INTO product (id_product, category_number, product_name, characteristics) VALUES
     (1, 1, 'Pepsi 1L', 'Carbonated drink, plastic bottle'),
     (2, 1, 'Morshynska 0.5L', 'Still water, plastic bottle'),
     (3, 2, 'Milk 2.5% Halychyna', 'Pasteurized milk, 900 ml'),
     (4, 2, 'Strawberry Yogurt Danone', 'Fruit yogurt, 300 g'),
     (5, 3, 'Roshen Dark Chocolate', '70% cocoa, 90 g'),
     (6, 3, 'Korivka Candies', 'Milk candies, 1 kg'),
     (7,4, 'Buckwheat 1 kg', 'Whole grain, packaged'),
     (8, 4, 'Chumak Pasta Spirals', '500 g, hard wheat'),
     (9, 5, 'Wheat Bread', 'Loaf 500 g, sliced'),
     (10,6, 'Salami Sausage', 'Dry-cured, 300 g')
ON CONFLICT (id_product) DO NOTHING;