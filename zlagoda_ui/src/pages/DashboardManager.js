import React from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import { useNavigate } from "react-router-dom";

export default function DashboardManager() {
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
        <div className="container-fluid bg-light min-vh-100 py-4">
            <div className="container">

                <div className="d-flex justify-content-between align-items-center mb-4 border-bottom pb-2">
                    <div>
                        <h2 className="fw-bold mb-0">Панель менеджера</h2>
                        <small className="text-muted">Користувач: {user.surname} {user.name}</small>
                    </div>
                    <div className="d-flex align-items-center gap-2">
                        <span className="badge bg-secondary p-2">{user.role?.toUpperCase()}</span>
                        <button onClick={handleLogout} className="btn btn-sm btn-outline-danger">Вийти</button>
                    </div>
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

                <div className="row g-3">
                    {[
                        { label: "Працівники", to: "/manager/employees" },
                        { label: "Клієнти", to: "/manager/clients" },
                        { label: "Категорії товарів", to: "/manager/categories" },
                        { label: "Товари", to: "/manager/products" },
                        { label: "Товари в магазині", to: "/manager/store-products" },
                        { label: "Чеки", to: "/manager/checks" },
                        { label: "Продажі / Статистика", to: "/manager/sales" },
                    ].map(({ label, to }) => (
                        <div className="col-md-4" key={label}>
                            <Link to={to} className="text-decoration-none">
                                <div className="bg-white border rounded p-3 shadow-sm h-100 d-flex align-items-center justify-content-center hover-effect">
                                    <span className="text-dark fw-medium">{label}</span>
                                </div>
                            </Link>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}
