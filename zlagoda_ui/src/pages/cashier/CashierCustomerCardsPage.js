import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CustomerCardsPage() {
    const [cards, setCards] = useState([]);
    const [search, setSearch] = useState("");
    const [error, setError] = useState("");
    const [form, setForm] = useState({
        custSurname: "", custName: "", custPatronymic: "", phoneNumber: "",
        city: "", street: "", zipCode: "", percent: 0
    });
    const [isEditing, setIsEditing] = useState(false);
    const [selectedCard, setSelectedCard] = useState(null);
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
        setSelectedCard(card.cardNumber);
    };

    const handleFormChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = () => {
        const payload = { ...form };

        const method = isEditing ? axios.put : axios.post;
        const url = isEditing
            ? `http://localhost:5112/api/cards?number=${form.cardNumber}`
            : "http://localhost:5112/api/cards";

        method(url, payload, { withCredentials: true })
            .then(() => {
                fetchSortedCards();
                setForm({
                    custSurname: "", custName: "", custPatronymic: "",
                    phoneNumber: "", city: "", street: "", zipCode: "", percent: 0,
                    cardNumber: "" // якщо дозволяєш користувачу вводити його вручну
                });
                setIsEditing(false);
                setSelectedCard(null);
            })
            .catch(() => setError("Помилка збереження"));
    };


    const handlePrint = () => {
        const printWindow = window.open("", "_blank", "width=800,height=600");
        if (!printWindow) return;

        const cloned = printRef.current.cloneNode(true);
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
        <div className="container mt-4">
            <h2 className="mb-3">Клієнти</h2>
            {error && <div className="alert alert-danger">{error}</div>}

            <div className="row mb-3">
                <div className="col-md-4">
                    <input
                        className="form-control"
                        type="text"
                        placeholder="Пошук за прізвищем"
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />
                </div>
                <div className="col-md-2">
                    <button className="btn btn-outline-secondary w-100" onClick={handleSearch}>Пошук</button>
                </div>
                <div className="col-md-2">
                    <button className="btn btn-outline-primary w-100" onClick={handlePrint}>Друк</button>
                </div>
            </div>

            <div className="card p-3 mb-4">
                <h5>{isEditing ? "Редагувати клієнта" : "Додати клієнта"}</h5>
                <div className="row g-2">
                    {[ "custSurname", "custName", "custPatronymic", "phoneNumber", "city", "street", "zipCode", "percent" ].map(name => (
                        <div className="col-md-3" key={name}>
                            <input
                                name={name}
                                value={form[name]}
                                onChange={handleFormChange}
                                className="form-control"
                                placeholder={
                                    {
                                        custSurname: "Прізвище",
                                        custName: "Імʼя",
                                        custPatronymic: "По-батькові",
                                        phoneNumber: "Телефон",
                                        city: "Місто",
                                        street: "Вулиця",
                                        zipCode: "Індекс",
                                        percent: "Знижка (%)"
                                    }[name]
                                }
                            />
                        </div>
                    ))}
                    <div className="col-md-2">
                        <button className="btn btn-success w-100" onClick={handleSubmit}>
                            {isEditing ? "Зберегти" : "Додати"}
                        </button>
                    </div>
                </div>
            </div>

            <div ref={printRef}>
                <table className="table table-bordered table-striped">
                    <thead>
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
