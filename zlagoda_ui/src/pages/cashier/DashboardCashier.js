import React from "react";
import {Link, useNavigate} from "react-router-dom";
import axios from "axios";

export default function DashboardCashier() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");

    const formatDate = (str) =>
        new Date(str).toLocaleDateString("uk-UA", {
            day: "2-digit",
            month: "2-digit",
            year: "numeric",
        });

    const navigate = useNavigate();

    const handleLogout = async () => {
        try {
            await axios.post("http://localhost:5112/logout", {}, { withCredentials: true });
            localStorage.clear();
            navigate("/login");
        } catch (err) {
            console.error("Помилка при виході з акаунта:", err);
        }
    };

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-4 border-bottom pb-2">
                <div>
                    <h2>Вітаю, {user.name} {user.surname}!</h2>
                    <p><strong>Ваша роль:</strong> {user.role?.toUpperCase()}</p>
                </div>
                <button onClick={handleLogout} className="btn btn-sm btn-outline-danger">Вийти</button>
            </div>

            <div className="row mb-4">
                <div className="col-md-6">
                    <div className="bg-white rounded shadow-sm p-4 h-100 border">
                        <h5 className="mb-3 text-secondary">Особисті дані</h5>
                        <p className="mb-1"><strong>ПІБ:</strong> {user.surname} {user.name} {user.patronymic}</p>
                        <p className="mb-1"><strong>Телефон:</strong> {user.phoneNumber}</p>
                        <p className="mb-1"><strong>Зарплата:</strong> {user.salary} грн</p>
                        <p className="mb-1"><strong>Дата початку роботи:</strong> {formatDate(user.dateOfStart)}</p>
                        <p className="mb-1"><strong>Дата народження:</strong> {formatDate(user.dateOfBirth)}</p>
                        <p className="mb-0"><strong>Адреса:</strong> {user.city}, {user.street}, {user.zipCode}</p>
                    </div>
                </div>
            </div>


            <hr/>
            <h4>Панель касира</h4>

            <div className="row g-3">   
                {[
                    { label: "Товари", to: "/cashier/products" },
                    { label: "Товари в магазині", to: "/cashier/store-products" },
                    { label: "Клієнти", to: "/cashier/clients" },
                    { label: "Мої чеки", to: "/cashier/checks" },
                ].map(({label, to}) => (
                    <div className="col-md-4" key={label}>
                        <Link to={to} className="text-decoration-none">
                            <div
                                className="bg-white border rounded p-3 shadow-sm h-100 d-flex align-items-center justify-content-center hover-effect">
                                <span className="text-dark fw-medium">{label}</span>
                            </div>
                        </Link>
                    </div>
                ))}
            </div>
        </div>
    );
}
