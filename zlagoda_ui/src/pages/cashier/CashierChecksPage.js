import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CashierCheckPage() {
    const [checks, setChecks] = useState([]);
    const [expandedCheck, setExpandedCheck] = useState(null);
    const [checkSalesMap, setCheckSalesMap] = useState({});
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [error, setError] = useState("");
    const [cashierId, setCashierId] = useState(null);
    const printRef = useRef();
    const [showModal, setShowModal] = useState(false);
    const [newItem, setNewItem] = useState({ upc: "", productNumber: 1 });
    const [sales, setSales] = useState([]);
    const [newCheckData, setNewCheckData] = useState({ cardNumber: "" });
    const [modalError, setModalError] = useState("");
    const [allProducts, setAllProducts] = useState([]);


    useEffect(() => {
        axios.get("http://localhost:5112/api/employees/me", { withCredentials: true })
            .then(res => {
                setCashierId(res.data.idEmployee);
            })
            .catch(() => setError("Не вдалося отримати інформацію про користувача"));
    }, []);

    useEffect(() => {
        if (cashierId) {
// eslint-disable-next-line react-hooks/exhaustive-deps
            fetchChecks();
        }
    }, [cashierId]);

    const fetchChecks = async () => {

        try {
            setError("");

            let url = `http://localhost:5112/api/checks/cashiers/${cashierId}/sales`;
            let params = {};

            if (startDate && endDate) {
                params = { start: startDate, end: endDate };
            }

            const res = await axios.get(url, {
                params,
                withCredentials: true
            });

            const checks = res.data;
            const map = {};
            for (const c of checks) {
                if (c.sales) map[c.checkNumber] = c.sales;
            }
            setChecks(checks.map(c => ({
                ...c,
                printDate: new Date(c.printDate)
            })));
            setCheckSalesMap(map);
        } catch {
            setError("Не вдалося завантажити чеки");
        }
    };

    const handleToggleDetails = async (checkNumber) => {
        if (expandedCheck === checkNumber) {
            setExpandedCheck(null);
            return;
        }
        if (!checkSalesMap[checkNumber]) {
            try {
                const res = await axios.get(`http://localhost:5112/api/checks/${checkNumber}/sales`, {
                    withCredentials: true
                });
                setCheckSalesMap(prev => ({ ...prev, [checkNumber]: res.data.sales }));
            } catch {
                setError("Не вдалося завантажити деталі");
                return;
            }
        }
        setExpandedCheck(checkNumber);
    };

    const handlePrint = () => {
        const printContent = printRef.current.innerHTML;
        const printWindow = window.open("", "", "width=900,height=600");
        if (!printWindow) return;
        printWindow.document.write(`
            <html>
            <head>
                <title>Чеки</title>
                <style>
                    body { font-family: sans-serif; padding: 20px; }
                    table { width: 100%; border-collapse: collapse; margin-top: 20px; }
                    th, td { border: 1px solid #333; padding: 6px; text-align: left; }
                </style>
            </head>
            <body>
                ${printContent}
            </body>
            </html>
        `);
        printWindow.document.close();
        printWindow.focus();
        setTimeout(() => printWindow.print(), 500);
    };

    const handleToday = () => {
        const today = new Date().toISOString().split("T")[0];
        setStartDate(today);
        setEndDate(today);
        setTimeout(fetchChecks, 0);
    };

    const handleAddItem = async () => {
        if (!newItem.upc || newItem.productNumber <= 0) {
            setModalError("Введіть коректний UPC та кількість");
            return;
        }

        try {
            const res = await axios.get(
                `http://localhost:5112/api/store-products/availability/restricted/${newItem.upc}`,
                { withCredentials: true }
            );

            const product = res.data;

            if (!product || product.sellingPrice === undefined) {
                setModalError("Неправильний UPC");
                return;
            }

            const item = {
                upc: newItem.upc,
                productNumber: newItem.productNumber,
                sellingPrice: product.sellingPrice
            };

            setSales(prev => [...prev, item]);
            setNewItem({ upc: "", productNumber: 1 });
            setModalError(""); // очищаємо помилку
        } catch (err) {
            if (err.response?.status === 404) {
                setModalError("Неправильний UPC");
            } else {
                setModalError("Помилка при додаванні товару");
            }
        }

    };

    const submitCheck = async () => {
        if (sales.length === 0) {
            setModalError("Додайте хоча б один товар");
            return;
        }

        const total = sales.reduce((sum, s) => sum + s.sellingPrice * s.productNumber, 0);
        const vat = total * 0.2;

        const now = new Date().toISOString();

        const check = {
            idEmployee: cashierId,
            cardNumber: newCheckData.cardNumber || null,
            printDate: now,
            sumTotal: total,
            vat,
            sales
        };

        try {
            await axios.post("http://localhost:5112/api/checks", check, { withCredentials: true });
            setShowModal(false);
            fetchChecks();
        } catch {
            setError("Не вдалося створити чек");
        }
    };

    const openModal = () => {
        setShowModal(true);
        setModalError("");
        setNewItem({ upc: "", productNumber: 1 });
        setSales([]);

        axios.get("http://localhost:5112/api/store-products", { withCredentials: true })
            .then(res => setAllProducts(res.data))
            .catch(() => setModalError("Не вдалося завантажити товари"));
    };

    return (
        <div className="container py-4">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2 className="mb-0">Мої чеки</h2>
                <div className="d-flex gap-2">
                    <button
                        className="btn btn-outline-primary"
                        onClick={() => window.location.href = "/cashier"}
                    >
                        Головне меню
                    </button>
                    <button className="btn btn-outline-secondary" onClick={handlePrint}>
                        Друк
                    </button>
                </div>
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <div className="row g-3 mb-4 align-items-end">
                <div className="col-md-3">
                    <label className="form-label">Початкова дата</label>
                    <input
                        type="date"
                        className="form-control"
                        value={startDate}
                        onChange={e => setStartDate(e.target.value)}
                    />
                </div>
                <div className="col-md-3">
                    <label className="form-label">Кінцева дата</label>
                    <input
                        type="date"
                        className="form-control"
                        value={endDate}
                        onChange={e => setEndDate(e.target.value)}
                    />
                </div>
                <div className="col-md-2 d-grid">
                    <button className="btn btn-outline-dark" onClick={handleToday}>
                        Сьогодні
                    </button>
                </div>
                <div className="col-md-2 d-grid">
                    <button className="btn btn-outline-dark" onClick={fetchChecks}>
                        Застосувати фільтр
                    </button>
                </div>
                <div className="col-md-2 d-grid">
                    <button
                        className="btn btn-outline-secondary"
                        onClick={() => {
                            setStartDate("");
                            setEndDate("");
                            fetchChecks();
                        }}
                    >
                        Очистити
                    </button>
                </div>
            </div>



            <div ref={printRef} className="bg-white p-3 shadow-sm border rounded table-responsive">
                <table className="table table-hover w-100">
                    <thead className="table-light">
                    <tr>
                        <th>№ чека</th>
                        <th>Картка</th>
                        <th>Дата</th>
                        <th>Сума</th>
                        <th>ПДВ</th>
                        <th>Дії</th>
                    </tr>
                    </thead>
                    <tbody>
                    {checks.map((check, i) => (
                        <React.Fragment key={i}>
                            <tr>
                                <td>{check.checkNumber}</td>
                                <td>{check.cardNumber || "-"}</td>
                                <td>{check.printDate.toLocaleString()}</td>
                                <td>{check.sumTotal.toFixed(2)} грн</td>
                                <td>{check.vat.toFixed(2)} грн</td>
                                <td>
                                    <button className="btn btn-sm btn-outline-primary" onClick={() => handleToggleDetails(check.checkNumber)}>
                                        {expandedCheck === check.checkNumber ? "Сховати" : "Деталі"}
                                    </button>
                                </td>
                            </tr>
                            {expandedCheck === check.checkNumber && checkSalesMap[check.checkNumber] && (
                                <tr className="bg-light">
                                    <td colSpan="6">
                                        <table className="table table-sm table-bordered mb-0">
                                            <thead className="table-secondary">
                                            <tr>
                                                <th>Назва</th>
                                                <th>UPC</th>
                                                <th>Кількість</th>
                                                <th>Ціна</th>
                                                <th>Разом</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            {checkSalesMap[check.checkNumber].map((sale, j) => (
                                                <tr key={j}>
                                                    <td>{sale.productName}</td>
                                                    <td>{sale.upc}</td>
                                                    <td>{sale.productNumber}</td>
                                                    <td>
                                                        {sale.sellingPrice && sale.productNumber
                                                            ? (sale.productNumber * sale.sellingPrice).toFixed(2) + " грн"
                                                            : "—"}
                                                    </td>
                                                    <td>{(sale.productNumber * sale.sellingPrice).toFixed(2)} грн</td>
                                                </tr>
                                            ))}
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            )}
                        </React.Fragment>
                    ))}
                    </tbody>
                </table>
            </div>
            {showModal && (
                <div className="modal d-block" tabIndex="-1" style={{ backgroundColor: "rgba(0,0,0,0.5)" }}>
                    <div className="modal-dialog modal-lg">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Новий чек</h5>
                                <button type="button" className="btn-close" onClick={() => setShowModal(false)}></button>
                            </div>
                            <div className="modal-body">
                                {modalError && (
                                    <div className="alert alert-danger mt-2">{modalError}</div>
                                )}
                                <div className="mb-3">
                                    <label className="form-label">Картка клієнта (необовʼязково)</label>
                                    <input
                                        type="text"
                                        className="form-control"
                                        value={newCheckData.cardNumber}
                                        onChange={e => setNewCheckData({ cardNumber: e.target.value })}
                                    />
                                </div>

                                <div className="row g-2 align-items-end mb-3">
                                    <div className="col-md-8">
                                        <label className="form-label">Оберіть товар (UPC)</label>
                                        <select
                                            className="form-select"
                                            value={newItem.upc}
                                            onChange={(e) => setNewItem(prev => ({ ...prev, upc: e.target.value }))}
                                        >
                                            <option value="">-- Оберіть товар --</option>
                                            {allProducts.map(p => (
                                                <option key={p.upc} value={p.upc}>
                                                    {p.productName} ({p.upc})
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                    <div className="col-md-3">
                                        <label className="form-label">Кількість</label>
                                        <input
                                            type="number"
                                            className="form-control"
                                            value={newItem.productNumber}
                                            onChange={e => setNewItem(prev => ({ ...prev, productNumber: Number(e.target.value) }))}
                                        />
                                    </div>
                                    <div className="col-md-3">
                                        <button className="btn btn-outline-primary w-100" onClick={handleAddItem}>
                                            Додати товар
                                        </button>
                                    </div>
                                </div>

                                {sales.length > 0 && (
                                    <table className="table table-sm table-bordered">
                                        <thead className="table-secondary">
                                        <tr>
                                            <th>UPC</th>
                                            <th>Кількість</th>
                                            <th>Ціна</th>
                                            <th>Разом</th>
                                            <th></th>
                                        </tr>
                                        </thead>
                                        <tbody>
                                        {sales.map((s, i) => (
                                            <tr key={i}>
                                                <td>{s.upc}</td>
                                                <td>{s.productNumber}</td>
                                                <td>{s.sellingPrice.toFixed(2)} грн</td>
                                                <td>{(s.productNumber * s.sellingPrice).toFixed(2)} грн</td>
                                                <td>
                                                    <button className="btn btn-sm btn-outline-danger"
                                                            onClick={() => setSales(prev => prev.filter((_, j) => j !== i))}>
                                                        ×
                                                    </button>
                                                </td>
                                            </tr>
                                        ))}
                                        </tbody>
                                    </table>
                                )}
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-secondary" onClick={() => setShowModal(false)}>Скасувати</button>
                                <button className="btn btn-success" onClick={submitCheck}>Підтвердити</button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>


    );
}
