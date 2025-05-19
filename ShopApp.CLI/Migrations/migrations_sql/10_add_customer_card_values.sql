INSERT INTO customer_card (
    card_number, cust_surname, cust_name, cust_patronymic,
    phone_number, city, street, zip_code, percent
) VALUES
      ('CARD001', 'Bondarenko', 'Oksana', 'Ihorivna', '+380931112233', 'Kyiv', 'Volodymyrska 10', '01030', 5),
      ('CARD002', 'Kovalenko', 'Mykhailo', 'Petrovych', '+380972223344', 'Kharkiv', 'Pushkinska 3', '61000', 10),
      ('CARD003', 'Melnyk', 'Kateryna', 'Andriivna', '+380961234567', 'Lviv', 'Franko 12', '79000', 7),
      ('CARD004', 'Tkachenko', 'Viktor', 'Ivanovych', '+380951112233', 'Odesa', 'Prymorska 1', '65000', 8),
      ('CARD005', 'Lysenko', 'Svitlana', 'Petrovna', '+380671234555', 'Dnipro', 'Gagarina 20', '49000', 6),
      ('CARD006', 'Rudenko', 'Nazar', 'Serhiovych', '+380501119988', 'Zaporizhzhia', 'Soborna 3', '69000', 9),
      ('CARD007', 'Hnatyuk', 'Iryna', 'Stepanivna', '+380931223344', 'Vinnytsia', 'Kotsiubynskoho 4', '21000', 4),
      ('CARD008', 'Horbach', 'Dmytro', 'Olehovych', '+380991234123', 'Poltava', 'Zhovtneva 17', '36000', 5),
      ('CARD009', 'Boiko', 'Yuliia', 'Tarasivna', '+380931119911', 'Ternopil', 'Shevchenka 8', '46000', 6),
      ('CARD010', 'Savchenko', 'Taras', 'Mykolaiovych', '+380951198877', 'Cherkasy', 'Pobedy 2', '18000', 7)
ON CONFLICT DO NOTHING;
