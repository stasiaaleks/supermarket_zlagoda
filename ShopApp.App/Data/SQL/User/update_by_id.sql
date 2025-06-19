UPDATE "user"
SET password_salt = @PasswordSalt,
    password_hash = @PasswordHash,
    username = @Username,
    id_employee = @IdEmployee
WHERE user_id = @UserId
