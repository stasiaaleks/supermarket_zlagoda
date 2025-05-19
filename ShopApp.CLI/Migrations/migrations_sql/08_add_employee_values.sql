INSERT INTO employee (
    id_employee, empl_surname, empl_name, empl_patronymic,
    empl_role, salary, date_of_birth, date_of_start,
    phone_number, city, street, zip_code
) VALUES
      ('E001', 'Ivanov', 'Ivan', 'Ivanovych', 'manager', 20000.00, '1990-05-15', '2020-01-01', '+380931234567', 'Kyiv', 'Khreshchatyk 1', '01001'),
      ('E002', 'Petrenko', 'Olena', 'Serhiivna', 'cashier', 15000.00, '1995-08-20', '2022-03-10', '+380671112233', 'Lviv', 'Shevchenka 12', '79000'),
      ('E003', 'Shevchenko', 'Andrii', 'Mykolaiovych', 'cashier', 16000.00, '1998-12-01', '2023-06-01', '+380503334455', 'Odesa', 'Derybasivska 7', '65000')
ON CONFLICT DO NOTHING;
