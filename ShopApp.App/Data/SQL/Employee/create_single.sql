INSERT INTO employee 
(
    id_employee, surname, name, patronymic,
     role, salary, date_of_birth, date_of_start,
    phone_number, city, street, zip_code
) 
VALUES
(
     @IdEmployee, @Surname, @Name, @Patronymic, @Role, @Salary, @DateOfBirth,
     @DateOfStart, @PhoneNumber, @City, @Street, @ZipCode
)
RETURNING id_employee;