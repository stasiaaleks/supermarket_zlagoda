﻿SELECT surname, name, phone_number, city, street, zip_code
FROM employee
WHERE LOWER(surname) LIKE CONCAT('%', LOWER(@Surname), '%');