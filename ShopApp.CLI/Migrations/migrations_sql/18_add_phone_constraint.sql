ALTER TABLE employee
ADD CONSTRAINT phone_check CHECK (employee.phone_number ~ '^\+\d{12}$');

ALTER TABLE customer_card
ADD CONSTRAINT phone_check CHECK (customer_card.phone_number ~ '^\+\d{12}$');

