INSERT INTO category (category_name)
VALUES (@CategoryName)
RETURNING category_number;
