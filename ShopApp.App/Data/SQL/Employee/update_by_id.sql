UPDATE employee
SET
    surname = @Surname,
    name = @Name,
    patronymic = @Patronymic,
    role = @Role,
    salary = @Salary,
    date_of_birth = @DateOfBirth,
    date_of_start = @DateOfStart,
    phone_number = @PhoneNumber,
    city = @City,
    street = @Street,
    zip_code = @ZipCode
WHERE id_employee = @IdEmployee
RETURNING id_employee;