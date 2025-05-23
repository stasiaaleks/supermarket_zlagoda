UPDATE employee
SET surname = @Surname,
    name = @Name,
    patronymic = @Patronymic,
    role = @Role,
    salary = @Salary,
    date_of_birth = @Date_of_birth,
    phone_number = @Phone_number,
    city = @City,
    street = @Street,
    zip_code = @Zip_code
WHERE id_employee = @Id
