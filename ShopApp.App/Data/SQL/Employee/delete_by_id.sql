DELETE FROM employee WHERE id_employee=@Id;
SELECT SETVAL('public.employee_id_employee_seq',(SELECT COUNT(*) FROM employee)); -- to reset the ids sequence