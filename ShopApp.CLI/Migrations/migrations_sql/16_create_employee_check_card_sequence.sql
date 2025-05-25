CREATE SEQUENCE employee_id_employee_seq
    AS integer
    INCREMENT 1
    MINVALUE 1
    START WITH 1
    OWNED BY public.employee.id_employee;

CREATE SEQUENCE check_number_check_seq
    AS integer
    INCREMENT 1
    MINVALUE 1
    START WITH 1
    OWNED BY public."check".check_number;

CREATE SEQUENCE card_number_card_seq
    AS integer
    INCREMENT 1
    MINVALUE 1
    START WITH 1
    OWNED BY public.customer_card.card_number;
