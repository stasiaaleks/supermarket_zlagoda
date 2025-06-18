import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CardsPage() {
    const [cards, setCards] = useState([]);
    const [error, setError] = useState("");
    const [filter, setFilter] = useState({ surname: "", discount: "" });
    const printRef = useRef();

    useEffect(() => {
        fetchCards();
    }, []);

    const fetchCards = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/cards/filter", { withCredentials: true });
            setCards(res.data);
        } catch (err) {
            setError("Помилка при завантаженні карт клієнтів");
        }
    };

    const handleFilter = async () => {
        try {
            const params = {};
            if (filter.surname) params.custSurname = filter.surname;
            if (filter.discount) params.percent = filter.discount;

            const res = await axios.get("http://localhost:5112/api/cards/filter", {
                params,
                withCredentials: true,
            });
            setCards(res.data);
        } catch (err) {
            setError("Помилка при фільтрації карт");
        }
    };

    const handlePrint = () => {
        const printWindow = window.open("", "_blank", "width=800,height=600");
        if (!printWindow) return;

        const cloned = printRef.current.cloneNode(true);
        const content = cloned.innerHTML;

        const html = `
            <html lang="uk">
            <head>
                <title>Звіт по клієнтах</title>
                <style>
                    body { font-family: sans-serif; padding: 20px; }
                    table { width: 100%; border-collapse: collapse; }
                    th, td { border: 1px solid #333; padding: 8px; text-align: left; }
                    th { background: #eee; }
                </style>
            </head>
            <body>
                <h2>Звіт по клієнтах</h2>
                ${content}
            </body>
            </html>
        `;

        printWindow.document.write(html);
        printWindow.document.close();
        printWindow.focus();
        setTimeout(() => printWindow.print(), 500);
    };

    return (
        <div className="container py-4">
            <div className="d-flex justify-content-between align-items-center mb-4 border-bottom pb-2">
                <div>
                    <h2 className="fw-bold mb-0">Клієнти</h2>
                    <small className="text-muted">Картки постійних клієнтів супермаркету</small>
                </div>
                <div>
                    <button onClick={handlePrint} className="btn btn-outline-dark">Друк звіту</button>
                </div>
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <form onSubmit={(e) => { e.preventDefault(); handleFilter(); }} className="row g-3 align-items-end mb-4">
                <div className="col-md-4">
                    <label className="form-label">Пошук за прізвищем</label>
                    <input
                        name="surname"
                        value={filter.surname}
                        onChange={(e) => setFilter({ ...filter, surname: e.target.value })}
                        className="form-control"
                        placeholder="Прізвище клієнта"
                    />
                </div>
                <div className="col-md-4">
                    <label className="form-label">Пошук за знижкою (%)</label>
                    <input
                        name="discount"
                        type="number"
                        min="0"
                        value={filter.discount}
                        onChange={(e) => setFilter({ ...filter, discount: e.target.value })}
                        className="form-control"
                        placeholder="Відсоток знижки"
                    />
                </div>
                <div className="col-md-2">
                    <button type="submit" className="btn btn-outline-dark w-100">Фільтрувати</button>
                </div>
                <div className="col-md-2">
                    <button
                        type="button"
                        className="btn btn-secondary w-100"
                        onClick={() => {
                            setFilter({ surname: "", discount: "" });
                            fetchCards();
                        }}
                    >
                        Очистити
                    </button>
                </div>
            </form>

            <div ref={printRef} className="bg-white p-3 shadow-sm rounded border">
                <table className="table table-hover">
                    <thead className="table-light">
                    <tr>
                        <th>Номер картки</th>
                        <th>Прізвище</th>
                        <th>Ім’я</th>
                        <th>По батькові</th>
                        <th>Телефон</th>
                        <th>Місто</th>
                        <th>Вулиця</th>
                        <th>Індекс</th>
                        <th>Знижка (%)</th>
                    </tr>
                    </thead>

                    <tbody>
                    {cards.map((card) => (
                        <tr key={card.cardNumber}>
                            <td>{card.cardNumber}</td>
                            <td>{card.custSurname}</td>
                            <td>{card.custName}</td>
                            <td>{card.custPatronymic}</td>
                            <td>{card.phoneNumber}</td>
                            <td>{card.city}</td>
                            <td>{card.street}</td>
                            <td>{card.zipCode}</td>
                            <td>{card.percent}</td>
                        </tr>
                    ))}
                    </tbody>

                </table>
            </div>
        </div>
    );
}
