import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

export default function LoginPage() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setError("");

        try {
            await axios.post(
                "http://localhost:5112/login",
                { username, password },
                { withCredentials: true }
            );

            const res = await axios.get("http://localhost:5112/api/employees/me", {
                withCredentials: true,
            });

            const role = res.data.role.toLowerCase() === "manager" ? "Manager" : "Cashier";
            localStorage.setItem("role", role);
            localStorage.setItem("user", JSON.stringify(res.data));


            navigate(`/${role.toLowerCase()}`);
        } catch (err) {
            console.error(err);
            setError("Невірний логін або пароль");
        }
    };

    return (
        <div className="d-flex align-items-center justify-content-center vh-100 bg-light">
            <div className="card shadow p-4" style={{ minWidth: "350px", maxWidth: "400px" }}>
                <h2 className="text-center mb-4">Вхід у систему ZLAGODA</h2>
                {error && <div className="alert alert-danger">{error}</div>}
                <form onSubmit={handleLogin}>
                    <div className="form-group mb-3">
                        <label>Логін</label>
                        <input
                            type="text"
                            className="form-control"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
                    </div>
                    <div className="form-group mb-4">
                        <label>Пароль</label>
                        <input
                            type="password"
                            className="form-control"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>
                    <button type="submit" className="btn btn-primary w-100">
                        Увійти
                    </button>
                </form>
            </div>
        </div>
    );
}
