CREATE TABLE IF NOT EXISTS employee (
    id_employee VARCHAR(10) PRIMARY KEY,
    surname VARCHAR(50) NOT NULL,
    name VARCHAR(50) NOT NULL,
    patronymic VARCHAR(50) NOT NULL,
    role VARCHAR(10) NOT NULL,
    salary DECIMAL(13,4) NOT NULL,
    date_of_birth DATE NOT NULL,
    date_of_start DATE NOT NULL,
    phone_number VARCHAR(13) NOT NULL,
    city VARCHAR(50),
    street VARCHAR(50),
    zip_code VARCHAR(9)
);