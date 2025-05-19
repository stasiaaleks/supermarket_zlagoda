CREATE TABLE IF NOT EXISTS sale (
      upc VARCHAR(12) NOT NULL,
      check_number VARCHAR(10) NOT NULL,
      product_number INT NOT NULL,
      selling_price DECIMAL(13,4) NOT NULL,

      PRIMARY KEY (upc, check_number),

      FOREIGN KEY (upc)
          REFERENCES store_product(upc)
          ON UPDATE CASCADE
          ON DELETE NO ACTION,

      FOREIGN KEY (check_number)
          REFERENCES "check"(check_number)
          ON UPDATE CASCADE
          ON DELETE CASCADE
);
