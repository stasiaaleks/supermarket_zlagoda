﻿INSERT INTO "check" (
    check_number,
    id_employee,
    card_number,
    print_date,
    sum_total,
    vat
)
VALUES (
           @CheckNumber,
           @IdEmployee,
           @CardNumber,
           @PrintDate,
           @SumTotal,
           @VAT
       )
RETURNING check_number;
