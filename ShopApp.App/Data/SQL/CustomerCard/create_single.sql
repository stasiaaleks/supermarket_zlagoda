INSERT INTO customer_card (
    card_number,
    cust_surname,
    cust_name,
    cust_patronymic,
    phone_number,
    city,
    street,
    zip_code,
    percent
)
VALUES (
           @CardNumber,
           @CustSurname,
           @CustName,
           @CustPatronymic,
           @PhoneNumber,
           @City,
           @Street,
           @ZipCode,
           @Percent
       )
RETURNING card_number;
