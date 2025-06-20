import React, { useEffect, useState } from "react";
import axios from "axios";

export default function AnalyticsPage() {
    const [cashiers, setCashiers] = useState([]);
    const [storeProducts, setStoreProducts] = useState([]);
    const [selectedCashier, setSelectedCashier] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [selectedUpc, setSelectedUpc] = useState("");
    const [message, setMessage] = useState("");

    useEffect(() => {
        axios.get("http://localhost:5112/api/employees", { withCredentials: true })
            .then(res => setCashiers(res.data.filter(e => e.role.toLowerCase() === "cashier")))
            .catch(() => setCashiers([]));

        axios.get("http://localhost:5112/api/store-products", { withCredentials: true })
            .then(res => setStoreProducts(res.data))
            .catch(() => setStoreProducts([]));
    }, []);

    const fetchTotalSumByCashier = async () => {
        try {
            const res = await axios.get(`http://localhost:5112/api/checks/cashiers/${selectedCashier}/sum`, {
                params: { start: startDate, end: endDate },
                withCredentials: true
            });
            setMessage(`Загальна сума продажів касира за період: ${res.data.totalSum.toFixed(2)} грн`);
        } catch {
            setMessage("Не вдалося отримати суму для касира");
        }
    };

    const fetchTotalSumAll = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/checks/sum", {
                params: { start: startDate, end: endDate },
                withCredentials: true
            });
            setMessage(`Загальна сума продажів усіх касирів за період: ${res.data.totalSum.toFixed(2)} грн`);
        } catch {
            setMessage("Не вдалося отримати суму");
        }
    };

    const fetchTotalSoldProduct = async () => {
        try {
            const res = await axios.get(`http://localhost:5112/api/sales/${selectedUpc}/total`, {
                params: { start: startDate, end: endDate },
                withCredentials: true
            });
            setMessage(`Загальна кількість проданого товару (UPC: ${selectedUpc}) за період: ${res.data.totalSold}`);
        } catch {
            setMessage("Не вдалося отримати кількість проданого товару");
        }
    };

    return (
        <div className="container py-4">
            <h2 className="mb-4">Аналітика продажів</h2>

            <div className="row g-3 align-items-end mb-4">
                <div className="col-md-3">
                    <label className="form-label">Касир</label>
                    <select className="form-select" value={selectedCashier} onChange={e => setSelectedCashier(e.target.value)}>
                        <option value="">Оберіть касира</option>
                        {cashiers.map(c => (
                            <option key={c.idEmployee} value={c.idEmployee}>{c.surname} {c.name}</option>
                        ))}
                    </select>
                </div>
                <div className="col-md-2">
                    <label className="form-label">Від</label>
                    <input type="date" className="form-control" value={startDate} onChange={e => setStartDate(e.target.value)} />
                </div>
                <div className="col-md-2">
                    <label className="form-label">До</label>
                    <input type="date" className="form-control" value={endDate} onChange={e => setEndDate(e.target.value)} />
                </div>
                <div className="col-md-3 d-grid">
                    <button className="btn btn-outline-primary" onClick={fetchTotalSumByCashier}>Сума по касиру</button>
                </div>
            </div>

            <div className="row g-3 align-items-end mb-4">
                <div className="col-md-2">
                    <label className="form-label">Від</label>
                    <input type="date" className="form-control" value={startDate} onChange={e => setStartDate(e.target.value)} />
                </div>
                <div className="col-md-2">
                    <label className="form-label">До</label>
                    <input type="date" className="form-control" value={endDate} onChange={e => setEndDate(e.target.value)} />
                </div>
                <div className="col-md-3 d-grid">
                    <button className="btn btn-outline-success" onClick={fetchTotalSumAll}>Сума по всіх</button>
                </div>
            </div>

            <div className="row g-3 align-items-end mb-3">
                <div className="col-md-4">
                    <label className="form-label">UPC товару</label>
                    <select className="form-select" value={selectedUpc} onChange={e => setSelectedUpc(e.target.value)}>
                        <option value="">Оберіть товар</option>
                        {storeProducts.map(p => (
                            <option key={p.upc} value={p.upc}>{p.upc} ({p.productName})</option>
                        ))}
                    </select>
                </div>
                <div className="col-md-2">
                    <label className="form-label">Від</label>
                    <input type="date" className="form-control" value={startDate} onChange={e => setStartDate(e.target.value)} />
                </div>
                <div className="col-md-2">
                    <label className="form-label">До</label>
                    <input type="date" className="form-control" value={endDate} onChange={e => setEndDate(e.target.value)} />
                </div>
                <div className="col-md-3 d-grid">
                    <button className="btn btn-outline-warning" onClick={fetchTotalSoldProduct}>Кількість товару</button>
                </div>
            </div>

            {message && <div className="alert alert-info mt-4">{message}</div>}
        </div>
    );
}
