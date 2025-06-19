import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

export default function ChangePasswordPage() {
    const navigate = useNavigate();
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const username = user.username;

    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
        setSuccess("");

        try {
            await axios.post("http://localhost:5112/change-password", {
                username,
                password
            }, {
                withCredentials: true
            });

            // одразу вихід із системи
            await axios.post("http://localhost:5112/logout", {}, { withCredentials: true });
            localStorage.clear();
            navigate("/login");
        } catch (err) {
            setError("Не вдалося змінити пароль.");
        }
    };

    return (
        <div className="container py-5">
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <div className="bg-white p-4 shadow rounded border">
                        <h3 className="mb-4">Зміна паролю</h3>

                        {error && <div className="alert alert-danger">{error}</div>}
                        {success && <div className="alert alert-success">{success}</div>}

                        <form onSubmit={handleSubmit} className="row g-3">
                            <div className="col-12">
                                <label className="form-label">Новий пароль</label>
                                <input
                                    type="password"
                                    name="password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    className="form-control"
                                    placeholder="Введіть новий пароль"
                                    required
                                />
                            </div>
                            <div className="col-12 d-flex justify-content-between">
                                <button type="button" className="btn btn-outline-secondary" onClick={() => navigate(-1)}>
                                    Назад
                                </button>
                                <button type="submit" className="btn btn-primary">
                                    Змінити пароль
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}
