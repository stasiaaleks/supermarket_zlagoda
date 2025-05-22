CREATE SEQUENCE employee_id_employee_seq
    AS integer
    INCREMENT 1
    MINVALUE 1
    START WITH 1
    OWNED BY public.employee.id_employee
