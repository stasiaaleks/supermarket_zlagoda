import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CustomerCardsPage() {
    const [cards, setCards] = useState([]);
    const [search, setSearch] = useState("");
    const [error, setError] = useState("");
    const [form, setForm] = useState({
        custSurname: "", custName: "", custPatronymic: "", phoneNumber: "",
        city: "", street: "", zipCode: "", percent: 0, cardNumber: ""
    });
    const [isEditing, setIsEditing] = useState(false);
    const printRef = useRef();

    useEffect(() => {
        fetchSortedCards();
    }, []);

    const fetchSortedCards = () => {
        axios.get("http://localhost:5112/api/cards/filter", {
            params: { orderBy: "cust_surname" },
            withCredentials: true
        })
            .then(res => setCards(res.data))
            .catch(() => setError("Не вдалося завантажити клієнтів"));
    };

    const handleSearch = () => {
        if (!search.trim()) return fetchSortedCards();
        axios.get(`http://localhost:5112/api/cards/filter`, {
            params: {
                custSurname: search,
                orderBy: "cust_surname"
            },
            withCredentials: true
        })
            .then(res => setCards(res.data))
            .catch(() => setError("Не вдалося знайти клієнтів"));
    };

    const handleEdit = (card) => {
        setForm({ ...card });
        setIsEditing(true);
    };

    const handleFormChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = () => {
        const method = isEditing ? axios.put : axios.post;
        const url = isEditing
            ? `http://localhost:5112/api/cards?number=${form.cardNumber}`
            : "http://localhost:5112/api/cards";

        method(url, form, { withCredentials: true })
            .then(() => {
                fetchSortedCards();
                setForm({
                    custSurname: "", custName: "", custPatronymic: "",
                    phoneNumber: "", city: "", street: "", zipCode: "", percent: 0, cardNumber: ""
                });
                setIsEditing(false);
            })
            .catch(() => setError("Помилка збереження"));
    };

    const handlePrint = () => {
        const printWindow = window.open("", "_blank", "width=800,height=600");
        if (!printWindow) return;

        const cloned = printRef.current.cloneNode(true);
        cloned.querySelectorAll("th:last-child, td:last-child").forEach(el => el.remove());
        const content = cloned.innerHTML;

        const html = `
        <html lang="uk">
        <head>
          <title>Клієнти</title>
          <style>
            body { font-family: sans-serif; padding: 20px; }
            h2 { text-align: center; margin-bottom: 20px; }
            table { width: 100%; border-collapse: collapse; margin-top: 20px; }
            th, td { border: 1px solid #333; padding: 8px; text-align: left; font-size: 14px; }
            th { background: #f0f0f0; }
          </style>
        </head>
        <body>
          <h2>Список клієнтів</h2>
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
                    <small className="text-muted">Управління клієнтськими картками</small>
                </div>
                <div className="d-flex gap-2">
                    <button onClick={handlePrint} className="btn btn-outline-primary">Друк звіту</button>
                    <button onClick={() => window.location.href = "/cashier"} className="btn btn-outline-secondary">Головне меню</button>
                </div>
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <div className="card p-3 mb-4">
                <h5 className="mb-3">{isEditing ? "Редагувати клієнта" : "Додати клієнта"}</h5>
                <div className="row g-3">
                    {["custSurname", "custName", "custPatronymic", "phoneNumber", "city", "street", "zipCode", "percent"].map(name => (
                        <div className="col-md-3" key={name}>
                            <input
                                name={name}
                                value={form[name]}
                                onChange={handleFormChange}
                                className="form-control"
                                placeholder={{
                                    custSurname: "Прізвище",
                                    custName: "Імʼя",
                                    custPatronymic: "По-батькові",
                                    phoneNumber: "Телефон",
                                    city: "Місто",
                                    street: "Вулиця",
                                    zipCode: "Індекс",
                                    percent: "Знижка (%)"
                                }[name]}
                            />
                        </div>
                    ))}
                    <div className="col-md-3 d-grid">
                        <button className="btn btn-success" onClick={handleSubmit}>
                            {isEditing ? "Зберегти" : "Додати"}
                        </button>
                    </div>
                </div>
            </div>

            <form className="row g-3 align-items-end mb-4" onSubmit={(e) => { e.preventDefault(); handleSearch(); }}>
                <div className="col-md-4">
                    <label className="form-label">Пошук за прізвищем</label>
                    <input
                        className="form-control"
                        type="text"
                        placeholder="Введіть прізвище"
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />
                </div>
                <div className="col-md-2 d-grid">
                    <button className="btn btn-outline-dark" type="submit">Застосувати фільтр</button>
                </div>
                <div className="col-md-2 d-grid">
                    <button className="btn btn-secondary" type="button" onClick={() => {
                        setSearch("");
                        fetchSortedCards();
                    }}>Очистити</button>
                </div>
            </form>

            <div ref={printRef} className="bg-white p-3 shadow-sm rounded border">
                <table className="table table-hover">
                    <thead className="table-light">
                    <tr>
                        <th>Прізвище</th>
                        <th>Імʼя</th>
                        <th>По-батькові</th>
                        <th>Телефон</th>
                        <th>Місто</th>
                        <th>Вулиця</th>
                        <th>Індекс</th>
                        <th>Знижка (%)</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>
                    {cards.map(c => (
                        <tr key={c.cardNumber}>
                            <td>{c.custSurname}</td>
                            <td>{c.custName}</td>
                            <td>{c.custPatronymic}</td>
                            <td>{c.phoneNumber}</td>
                            <td>{c.city}</td>
                            <td>{c.street}</td>
                            <td>{c.zipCode}</td>
                            <td>{c.percent}</td>
                            <td>
                                <button className="btn btn-sm btn-outline-secondary" onClick={() => handleEdit(c)}>Редагувати</button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
