import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function ChecksPage() {
    const [checks, setChecks] = useState([]);
    const [cashiers, setCashiers] = useState([]);
    const [selectedCashier, setSelectedCashier] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [expandedCheck, setExpandedCheck] = useState(null);
    const [checkSalesMap, setCheckSalesMap] = useState({});
    const [totalSum, setTotalSum] = useState(null);
    const [error, setError] = useState("");
    const printRef = useRef();

    useEffect(() => {
        axios.get("http://localhost:5112/api/employees/cashiers", { withCredentials: true })
            .then(res => setCashiers(res.data))
            .catch(() => setError("Не вдалося завантажити касирів"));
    }, []);

    useEffect(() => {
        fetchChecks();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const fetchChecks = async () => {
        try {
            setError("");
            setTotalSum(null);
            console.log("ЗАПИТ:", { selectedCashier, startDate, endDate });

            let checks = [];

            if (selectedCashier) {
                const res = await axios.get(
                    `http://localhost:5112/api/checks/cashiers/${selectedCashier}/sales`,
                    {
                        params: {
                            ...(startDate && { start: startDate }),
                            ...(endDate && { end: endDate })
                        },
                        withCredentials: true
                    }
                );
                checks = res.data;
            } else if (startDate && endDate) {
                const res = await axios.get(
                    `http://localhost:5112/api/checks/sales`,
                    {
                        params: { start: startDate, end: endDate },
                        withCredentials: true
                    }
                );
                checks = res.data;
            } else {
                const res = await axios.get("http://localhost:5112/api/checks", { withCredentials: true });
                checks = res.data;
            }

            // якщо є sales — зберігаємо їх одразу
            const map = {};
            for (const c of checks) {
                if (c.sales) {
                    map[c.checkNumber] = c.sales;
                }
            }

            setChecks(checks.map(c => ({ ...c, printDate: new Date(c.printDate) })));
            setCheckSalesMap(map);
        } catch (e) {
            console.error(e);
            setError("Помилка при завантаженні чеків");
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
                    withCredentials: true,
                });

                setCheckSalesMap(prev => ({
                    ...prev,
                    [checkNumber]: res.data.sales
                }));
            } catch {
                setError("Не вдалося завантажити деталі для чека");
                return;
            }
        }

        setExpandedCheck(checkNumber);
    };


    const handlePrint = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/checks", { withCredentials: true });
            const allChecks = res.data;

            const detailedChecks = await Promise.all(
                allChecks.map(async (check) => {
                    try {
                        const res = await axios.get(`http://localhost:5112/api/checks/${check.checkNumber}/sales`, {
                            withCredentials: true,
                        });
                        return res.data;
                    } catch {
                        return { ...check, sales: [] };
                    }
                })
            );

            const content = detailedChecks.map((check) => `
            <h4>Чек №${check.checkNumber}</h4>
            <p>
                <strong>Касир:</strong> ${check.idEmployee} <br/>
                <strong>Картка:</strong> ${check.cardNumber || "-"} <br/>
                <strong>Дата:</strong> ${new Date(check.printDate).toLocaleString()} <br/>
                <strong>Сума:</strong> ${check.sumTotal.toFixed(2)} грн <br/>
                <strong>ПДВ:</strong> ${check.vat.toFixed(2)} грн
            </p>

            <table style="width: 100%; border-collapse: collapse; margin-bottom: 30px;">
                <thead style="background: #eee;">
                    <tr>
                        <th style="border: 1px solid #333; padding: 6px;">Назва товару</th>
                        <th style="border: 1px solid #333; padding: 6px;">К-сть</th>
                        <th style="border: 1px solid #333; padding: 6px;">Ціна</th>
                        <th style="border: 1px solid #333; padding: 6px;">Разом</th>
                    </tr>
                </thead>
                <tbody>
                    ${check.sales.map(sale => `
                        <tr>
                            <td style="border: 1px solid #333; padding: 6px;">${sale.productName}</td>
                            <td style="border: 1px solid #333; padding: 6px;">${sale.productNumber}</td>
                            <td style="border: 1px solid #333; padding: 6px;">${sale.sellingPrice.toFixed(2)} грн</td>
                            <td style="border: 1px solid #333; padding: 6px;">${(sale.productNumber * sale.sellingPrice).toFixed(2)} грн</td>
                        </tr>
                    `).join("")}
                </tbody>
            </table>
        `).join("<hr/>");

            const html = `
            <html>
            <head>
                <title>Звіт про всі чеки</title>
                <style>
                    body { font-family: sans-serif; padding: 20px; }
                    h4 { margin-bottom: 0.2em; }
                    p { margin-top: 0; line-height: 1.4; }
                </style>
            </head>
            <body>
                <h2>Звіт про всі чеки</h2>
                ${content}
            </body>
            </html>
        `;

            const printWindow = window.open("", "_blank", "width=900,height=600");
            if (!printWindow) return;
            printWindow.document.write(html);
            printWindow.document.close();
            printWindow.focus();
            setTimeout(() => printWindow.print(), 500);
        } catch {
            setError("Не вдалося згенерувати звіт");
        }
    };


    return (
        <div className="container py-4">
            <h2 className="mb-4">Перегляд чеків</h2>
            {error && <div className="alert alert-danger">{error}</div>}

            <div className="row g-3 mb-4 align-items-end">
                <div className="col-md-3">
                    <label className="form-label">Касир</label>
                    <select className="form-select" value={selectedCashier} onChange={e => setSelectedCashier(e.target.value)}>
                        <option value="">Усі касири</option>
                        {cashiers.map(c => (
                            <option key={c.idEmployee} value={c.idEmployee}>
                                {c.surname} {c.name}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="col-md-2">
                    <label className="form-label">Початкова дата</label>
                    <input type="date" className="form-control" value={startDate} onChange={e => setStartDate(e.target.value)} />
                </div>
                <div className="col-md-2">
                    <label className="form-label">Кінцева дата</label>
                    <input type="date" className="form-control" value={endDate} onChange={e => setEndDate(e.target.value)} />
                </div>
                <div className="col-md-3 d-grid">
                    <button
                        className="btn btn-outline-dark"
                        onClick={() => {
                            console.log("Натиснуто фільтр:", selectedCashier, startDate, endDate);
                            fetchChecks();
                        }}
                    >
                        Застосувати фільтр
                    </button>
                </div>
                <div className="col-md-2 d-grid">
                    <button className="btn btn-outline-secondary" onClick={handlePrint}>Друк</button>
                </div>
            </div>


            {totalSum !== null && (
                <div className="alert alert-info">Загальна сума: {totalSum.toFixed(2)} грн</div>
            )}

            <div ref={printRef} className="bg-white p-3 shadow-sm border rounded table-responsive">
                <table className="table table-hover w-100">
                    <thead className="table-light">
                    <tr>
                        <th>№ чека</th>
                        <th>Касир</th>
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
                                <td>{check.idEmployee}</td>
                                <td>{check.cardNumber || "-"}</td>
                                <td>{new Date(check.printDate).toLocaleString()}</td>
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
                                    <td colSpan="7">
                                        <table className="table table-sm table-bordered mb-0">
                                            <thead className="table-secondary">
                                            <tr>
                                                <th>UPC товару</th>
                                                <th>К-сть</th>
                                                <th>Ціна за одиницю</th>
                                                <th>Загальна ціна</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            {checkSalesMap[check.checkNumber].map((sale, j) => (
                                                <tr key={j}>
                                                    <td>{sale.upc}</td>
                                                    <td>{sale.productNumber}</td>
                                                    <td>{sale.productName}</td>
                                                    <td>{sale.sellingPrice.toFixed(2)} грн</td>

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
