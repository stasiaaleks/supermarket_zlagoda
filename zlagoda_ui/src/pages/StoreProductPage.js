import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function StoreProductsPage() {
    const [products, setProducts] = useState([]);
    const [error, setError] = useState("");
    const [filter, setFilter] = useState({ productName: "", categoryName: "" });
    const printRef = useRef();

    useEffect(() => {
        fetchAll();
    }, []);

    const fetchAll = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/store-products/filter", { withCredentials: true });
            setProducts(res.data);
        } catch (err) {
            setError("Помилка при завантаженні товарів");
        }
    };

    const fetchFiltered = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/store-products/filter", {
                params: filter,
                withCredentials: true,
            });
            setProducts(res.data);
        } catch {
            setError("Помилка при фільтрації товарів");
        }
    };

    const fetchPromo = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/store-products/promotional", { withCredentials: true });
            setProducts(res.data);
        } catch {
            setError("Помилка при отриманні акційних товарів");
        }
    };

    const fetchRegular = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/store-products/regular", { withCredentials: true });
            setProducts(res.data);
        } catch {
            setError("Помилка при отриманні звичайних товарів");
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
            table { width: 100%; border-collapse: collapse; }
            th, td { border: 1px solid #333; padding: 8px; text-align: left; }
            th { background: #eee; }
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
                    <small className="text-muted">Список усіх товарів, наявних у магазині</small>
                </div>
                <div>
                    <button onClick={handlePrint} className="btn btn-outline-dark">Друк звіту</button>
                </div>
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <form
                onSubmit={(e) => {
                    e.preventDefault();
                    fetchFiltered();
                }}
                className="row g-3 align-items-end mb-4"
            >
                <div className="col-md-4">
                    <label className="form-label">Назва товару</label>
                    <input
                        name="productName"
                        value={filter.productName}
                        onChange={(e) => setFilter({ ...filter, productName: e.target.value })}
                        className="form-control"
                        placeholder="Назва товару"
                    />
                </div>
                <div className="col-md-4">
                    <label className="form-label">Назва категорії</label>
                    <input
                        name="categoryName"
                        value={filter.categoryName}
                        onChange={(e) => setFilter({ ...filter, categoryName: e.target.value })}
                        className="form-control"
                        placeholder="Категорія"
                    />
                </div>
                <div className="col-md-2">
                    <button type="submit" className="btn btn-outline-dark w-100">Фільтрувати</button>
                </div>
                <div className="col-md-2 d-flex gap-1">
                    <button type="button" className="btn btn-secondary w-100" onClick={() => {
                        setFilter({ productName: "", categoryName: "" });
                        fetchAll();
                    }}>Очистити</button>
                </div>
            </form>

            <div className="mb-4 d-flex gap-2">
                <button className="btn btn-outline-primary" onClick={fetchPromo}>Акційні</button>
                <button className="btn btn-outline-primary" onClick={fetchRegular}>Звичайні</button>
                <button className="btn btn-outline-primary" onClick={fetchAll}>Усі</button>
            </div>

            <div ref={printRef} className="bg-white p-3 shadow-sm rounded border">
                <table className="table table-hover">
                    <thead className="table-light">
                    <tr>
                        <th>UPC</th>
                        <th>Назва товару</th>
                        <th>Характеристики</th>
                        <th>Ціна (₴)</th>
                        <th>Кількість</th>
                    </tr>
                    </thead>
                    <tbody>
                    {products.map((p) => (
                        <tr key={p.upc}>
                            <td>{p.upc}</td>
                            <td>{p.productName}</td>
                            <td>{p.characteristics}</td>
                            <td>{p.sellingPrice.toFixed(2)}</td>
                            <td>{p.productsNumber}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
