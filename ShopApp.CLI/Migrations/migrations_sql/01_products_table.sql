CREATE TABLE IF NOT EXISTS products (
    barcode SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);