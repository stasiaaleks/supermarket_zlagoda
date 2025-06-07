ALTER TABLE employee
ADD CONSTRAINT age_check CHECK (date_of_birth <= CURRENT_DATE - INTERVAL '18 years');

