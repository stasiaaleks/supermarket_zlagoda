INSERT INTO category (category_number, category_name) VALUES
      (1, 'Beverages'),
      (2, 'Dairy Products'),
      (3, 'Sweets'),
      (4, 'Grains and Pasta'),
      (5, 'Bakery'),
      (6, 'Meat and Sausages'),
      (7, 'Fruits and Vegetables'),
      (8, 'Canned Goods'),
      (9, 'Frozen Foods'),
      (10, 'Hygiene and Household') 
ON CONFLICT (category_number) DO NOTHING;

