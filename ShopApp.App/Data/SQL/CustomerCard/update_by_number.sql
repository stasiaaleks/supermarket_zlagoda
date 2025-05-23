UPDATE customer_card
SET
    cust_surname = @Surname,
    cust_name = @Name,
    cust_patronymic = @Patronymic,
    phone_number = @PhoneNumber,
    city = @City,
    street = @Street,
    zip_code = @ZipCode,
    percent = @Percent
WHERE card_number = @CardNumber;
