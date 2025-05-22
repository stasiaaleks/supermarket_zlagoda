DELETE FROM "user" WHERE user_id=@Id;
SELECT SETVAL('public.user_user_id_seq',(SELECT COUNT(*) FROM "user")); -- to reset the ids sequence