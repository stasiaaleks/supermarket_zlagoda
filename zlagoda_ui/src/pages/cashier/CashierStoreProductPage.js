import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CashierStoreProductsPage() {
    const [products, setProducts] = useState([]);
    const [filters, setFilters] = useState({ search: "", upc: "", promo: "" });
    const [filtered, setFiltered] = useState([]);
    const [error, setError] = useState("");
    const printRef = useRef();

    useEffect(() => {
        fetchProducts();
    }, []);

    const fetchProducts = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/store-products/filter", {
                params: { SortBy: "ProductName" },
                withCredentials: true
            });
            setProducts(res.data);
            setFiltered(res.data); // первинне завантаження
        } catch {
            setError("Не вдалося завантажити товари");
        }
    };

    const handleFilter = (e) => {
        e.preventDefault();
        const { search, upc, promo } = filters;
        const lowerName = search.toLowerCase();
        const lowerUpc = upc.toLowerCase();

        const result = products.filter(p => {
            const matchName = p.productName.toLowerCase().includes(lowerName);
            const matchUpc = p.upc.toLowerCase().includes(lowerUpc);
            const matchPromo =
                promo === "" ||
                (promo === "promo" && p.promotionalProduct) ||
                (promo === "regular" && !p.promotionalProduct);
            return matchName && matchUpc && matchPromo;
        });

        setFiltered(result);
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
        <div className="container-fluid bg-light min-vh-100 py-4">
            <div className="container">
                <div className="d-flex justify-content-between align-items-center mb-4 border-bottom pb-2">
                    <div>
                        <h2 className="fw-bold mb-0">Товари у магазині</h2>
                        <small className="text-muted">Перегляд товарів з бази супермаркету</small>
                    </div>
                    <div className="d-flex gap-2">
                        <button className="btn btn-outline-primary" onClick={handlePrint}>
                            Друк звіту
                        </button>
                        <button className="btn btn-outline-secondary" onClick={() => window.location.href = "/cashier"}>
                            Головне меню
                        </button>
                    </div>
                </div>

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleFilter} className="row g-3 align-items-end mb-4">
                    <div className="col-md-3">
                        <label className="form-label">Назва товару</label>
                        <input
                            type="text"
                            className="form-control"
                            value={filters.search}
                            onChange={(e) => setFilters({ ...filters, search: e.target.value })}
                            placeholder="Пошук за назвою"
                        />
                    </div>
                    <div className="col-md-3">
                        <label className="form-label">UPC</label>
                        <input
                            type="text"
                            className="form-control"
                            value={filters.upc}
                            onChange={(e) => setFilters({ ...filters, upc: e.target.value })}
                            placeholder="Пошук за UPC"
                        />
                    </div>
                    <div className="col-md-2">
                        <label className="form-label">Тип товару</label>
                        <select
                            className="form-select"
                            value={filters.promo}
                            onChange={(e) => setFilters({ ...filters, promo: e.target.value })}
                        >
                            <option value="">Усі товари</option>
                            <option value="promo">Акційні</option>
                            <option value="regular">Звичайні</option>
                        </select>
                    </div>
                    <div className="col-md-2">
                        <button type="submit" className="btn btn-outline-dark w-100">Застосувати фільтр</button>
                    </div>
                    <div className="col-md-2">
                        <button
                            type="button"
                            className="btn btn-secondary w-100"
                            onClick={() => {
                                setFilters({ categoryName: "" });
                                fetchProducts();
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
                        {filtered.map(p => (
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
        </div>
    );
}
