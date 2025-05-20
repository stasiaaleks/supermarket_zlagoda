CREATE TABLE IF NOT EXISTS "user" (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(30) NOT NULL UNIQUE,
    password_hash BYTEA NOT NULL,
    id_employee VARCHAR(20) NOT NULL,

    FOREIGN KEY (id_employee)
        REFERENCES employee(id_employee)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

