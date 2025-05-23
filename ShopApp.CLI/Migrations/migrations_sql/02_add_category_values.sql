INSERT INTO category (category_name) VALUES
      ('Beverages'),
      ('Dairy Products'),
      ('Sweets'),
      ('Grains and Pasta'),
      ('Bakery'),
      ('Meat and Sausages'),
      ('Fruits and Vegetables'),
      ('Canned Goods'),
      ('Frozen Foods'),
      ('Hygiene and Household') 
ON CONFLICT (category_number) DO NOTHING;

