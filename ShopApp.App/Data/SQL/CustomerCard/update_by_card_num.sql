UPDATE customer_card
SET
    cust_surname = @CustSurname,
    cust_name = @CustName,
    cust_patronymic = @CustPatronymic,
    phone_number = @PhoneNumber,
    city = @City,
    street = @Street,
    zip_code = @ZipCode,
    percent = @Percent
WHERE card_number = @CardNumber;
