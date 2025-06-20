import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CashierStoreProductsPage() {
    const [products, setProducts] = useState([]);
    const [error, setError] = useState("");
    const [filter, setFilter] = useState({ upc: "" });
    const [promoType, setPromoType] = useState(""); // "", "promo", "regular"
    const printRef = useRef();

    useEffect(() => {
        fetchAll();
    }, []);

    const fetchAll = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/store-products/filter", { withCredentials: true });
            setProducts(res.data);
        } catch {
            setError("Не вдалося завантажити товари");
        }
    };

    const handleFilter = async () => {
        try {
            if (filter.upc) {
                const res = await axios.get(`http://localhost:5112/api/store-products/${filter.upc}`, {
                    withCredentials: true
                });
                setProducts([res.data]);
                return;
            }

            let endpoint = "http://localhost:5112/api/store-products/filter";
            if (promoType === "promo") endpoint = "http://localhost:5112/api/store-products/promotional";
            if (promoType === "regular") endpoint = "http://localhost:5112/api/store-products/regular";

            const res = await axios.get(endpoint, { withCredentials: true });
            setProducts(res.data);
        } catch {
            setError("Помилка при фільтрації товарів");
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
              <title>Звіт по товарах у магазині</title>
              <style>
                body { font-family: sans-serif; padding: 20px; }
                h2 { text-align: center; margin-bottom: 20px; }
                table { width: 100%; border-collapse: collapse; margin-top: 20px; }
                th, td { border: 1px solid #333; padding: 8px; text-align: left; font-size: 14px; }
                th { background: #f0f0f0; }
              </style>
            </head>
            <body>
              <h2>Звіт по товарах у магазині</h2>
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
                    <h2 className="fw-bold mb-0">Товари в магазині</h2>
                    <small className="text-muted">Перегляд товарів для касира</small>
                </div>
                <div className="d-flex gap-2">
                    <button onClick={handlePrint} className="btn btn-outline-primary">Друк звіту</button>
                    <button onClick={() => window.location.href = "/cashier"} className="btn btn-outline-secondary">Головне меню</button>
                </div>
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <form onSubmit={(e) => { e.preventDefault(); handleFilter(); }} className="row g-3 align-items-end mb-4">
                <div className="col-md-3">
                    <label className="form-label">UPC товару</label>
                    <input
                        value={filter.upc}
                        onChange={(e) => setFilter({ ...filter, upc: e.target.value })}
                        className="form-control"
                        placeholder="Пошук за UPC"
                    />
                </div>
                <div className="col-md-3">
                    <label className="form-label">Тип товару</label>
                    <select className="form-select" value={promoType} onChange={(e) => setPromoType(e.target.value)}>
                        <option value="">Усі</option>
                        <option value="promo">Акційні</option>
                        <option value="regular">Звичайні</option>
                    </select>
                </div>
                <div className="col-md-3 d-grid">
                    <button type="submit" className="btn btn-outline-dark">Застосувати фільтр</button>
                </div>
                <div className="col-md-3 d-grid">
                    <button
                        type="button"
                        className="btn btn-secondary"
                        onClick={() => {
                            setFilter({ upc: "" });
                            setPromoType("");
                            fetchAll();
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
                        <th>Назва</th>
                        <th>UPC</th>
                        <th>Ціна</th>
                        <th>Кількість</th>
                        <th>Акційний</th>
                        <th>Характеристики</th>
                    </tr>
                    </thead>
                    <tbody>
                    {products.map((p) => (
                        <tr key={p.upc}>
                            <td>{p.productName}</td>
                            <td>{p.upc}</td>
                            <td>{p.sellingPrice.toFixed(2)} грн</td>
                            <td>{p.productsNumber}</td>
                            <td>{p.promotionalProduct ? "Так" : "Ні"}</td>
                            <td>{p.characteristics}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
