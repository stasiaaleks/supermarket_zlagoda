import React from "react";

export default function DashboardCashier() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");

    return (
        <div className="container mt-4">
            <h2>👋 Вітаю, {user.name} {user.surname}!</h2>
            <p><strong>Ваша роль:</strong> {user.role}</p>

            <hr />
            <h4>🧾 Панель касира</h4>
            <p>Тут буде можливість створювати чеки та бачити свої продажі.</p>
        </div>
    );
}
