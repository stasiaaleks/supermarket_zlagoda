ALTER TABLE employee
ADD CONSTRAINT employee_phone_unique UNIQUE (phone_number);

ALTER TABLE customer_card
ADD CONSTRAINT customer_card_phone_unique UNIQUE (phone_number);