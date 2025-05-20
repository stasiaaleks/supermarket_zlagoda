CREATE TABLE IF NOT EXISTS "user" (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(30) NOT NULL UNIQUE,
    password_hash VARCHAR(64) NOT NULL,
    password_salt VARCHAR(64) NOT NULL,
    id_employee VARCHAR(10) NOT NULL,

    FOREIGN KEY (id_employee)
        REFERENCES employee(id_employee)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

