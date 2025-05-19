CREATE TABLE IF NOT EXISTS product (
     id_product SERIAL PRIMARY KEY,
     category_number INT NOT NULL,
     product_name VARCHAR(50) NOT NULL,
     characteristics VARCHAR(100) NOT NULL,

     FOREIGN KEY (category_number)
         REFERENCES category(category_number)
         ON UPDATE CASCADE
         ON DELETE NO ACTION
);
