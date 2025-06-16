import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CashierCheckPage() {
    const [checks, setChecks] = useState([]);
    const [expandedCheck, setExpandedCheck] = useState(null);
    const [checkSalesMap, setCheckSalesMap] = useState({});
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [error, setError] = useState("");
    const printRef = useRef();

    const fetchChecks = async () => {
        try {
            setError("");
            let url = "/api/checks";
            let params = {};
            if (startDate && endDate) {
                url = "/api/checks/sales";
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
            setChecks(checks.map(c => ({ ...c, printDate: new Date(c.printDate) })));
            setCheckSalesMap(map);
        } catch {
            setError("Не вдалося завантажити чеки");
        }
    };

    useEffect(() => {
        fetchChecks();
    }, []);

    const handleToggleDetails = async (checkNumber) => {
        if (expandedCheck === checkNumber) {
            setExpandedCheck(null);
            return;
        }
        if (!checkSalesMap[checkNumber]) {
            try {
                const res = await axios.get(`/api/checks/${checkNumber}/sales`, { withCredentials: true });
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

    return (
        <div className="container py-4">
            <h2 className="mb-4">Мої чеки</h2>
            {error && <div className="alert alert-danger">{error}</div>}

            <div className="row g-3 mb-4 align-items-end">
                <div className="col-md-3">
                    <label className="form-label">Початкова дата</label>
                    <input type="date" className="form-control" value={startDate} onChange={e => setStartDate(e.target.value)} />
                </div>
                <div className="col-md-3">
                    <label className="form-label">Кінцева дата</label>
                    <input type="date" className="form-control" value={endDate} onChange={e => setEndDate(e.target.value)} />
                </div>
                <div className="col-md-3 d-grid">
                    <button className="btn btn-outline-dark" onClick={fetchChecks}>Застосувати фільтр</button>
                </div>
                <div className="col-md-3 d-grid">
                    <button className="btn btn-outline-secondary" onClick={handlePrint}>Друк</button>
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
                                                    <td>{sale.sellingPrice.toFixed(2)} грн</td>
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
        </div>
    );
}
