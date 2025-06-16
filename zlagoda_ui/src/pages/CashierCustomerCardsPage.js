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
    const [sortAsc, setSortAsc] = useState(true);
    const printRef = useRef();

    useEffect(() => {
        if (search.trim() === "") {
            fetchAll();
        }
    }, [search]);

    const fetchAll = () => {
        axios.get("http://localhost:5112/api/cards", { withCredentials: true })
            .then(res => setCards(res.data))
            .catch(() => setError("Не вдалося завантажити клієнтів"));
    };

    const handleSearch = () => {
        if (!search.trim()) return fetchAll();
        axios.get(`http://localhost:5112/api/cards/filter?custSurname=${encodeURIComponent(search)}`, { withCredentials: true })
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
        const method = isEditing ? axios.put : axios.post;
        const url = "http://localhost:5112/api/cards";
        const payload = { ...form };
        if (isEditing) payload.cardNumber = selectedCard;

        method(url, payload, { withCredentials: true })
            .then(() => {
                fetchAll();
                setForm({
                    custSurname: "", custName: "", custPatronymic: "",
                    phoneNumber: "", city: "", street: "", zipCode: "", percent: 0
                });
                setIsEditing(false);
                setSelectedCard(null);
            })
            .catch(() => setError("Помилка збереження"));
    };

    const handleSort = () => {
        const sorted = [...cards].sort((a, b) => {
            if (a.custSurname < b.custSurname) return sortAsc ? -1 : 1;
            if (a.custSurname > b.custSurname) return sortAsc ? 1 : -1;
            return 0;
        });
        setCards(sorted);
        setSortAsc(!sortAsc);
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
                    {[
                        { name: "custSurname", placeholder: "Прізвище" },
                        { name: "custName", placeholder: "Імʼя" },
                        { name: "custPatronymic", placeholder: "По-батькові" },
                        { name: "phoneNumber", placeholder: "Телефон" },
                        { name: "city", placeholder: "Місто" },
                        { name: "street", placeholder: "Вулиця" },
                        { name: "zipCode", placeholder: "Індекс" },
                        { name: "percent", placeholder: "Знижка (%)" }
                    ].map(({ name, placeholder }) => (
                        <div className="col-md-3" key={name}>
                            <input
                                name={name}
                                value={form[name]}
                                onChange={handleFormChange}
                                className="form-control"
                                placeholder={placeholder}
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
                        <th onClick={handleSort} style={{ cursor: "pointer" }}>
                            Прізвище {sortAsc ? "↑" : "↓"}
                        </th>
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
