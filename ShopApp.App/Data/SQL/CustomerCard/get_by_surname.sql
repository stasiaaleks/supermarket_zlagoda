SELECT * FROM customer_card
WHERE LOWER(cust_surname) = LOWER(@Surname);
