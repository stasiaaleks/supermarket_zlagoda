ALTER TABLE employee
ADD CONSTRAINT salary_check CHECK (salary > 0);

ALTER TABLE customer_card
ADD CONSTRAINT percent_check CHECK (percent > 0);

ALTER TABLE sale
ADD CONSTRAINT number_check CHECK (product_number > 0),
ADD CONSTRAINT price_check CHECK (selling_price > 0);

ALTER TABLE store_product
ADD CONSTRAINT number_check CHECK (products_number > 0),
ADD CONSTRAINT price_check CHECK (selling_price > 0);

ALTER TABLE "check"
ADD CONSTRAINT sum_check CHECK (sum_total > 0),
ADD CONSTRAINT vat_check CHECK (vat > 0);
