import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CashierProductsPage() {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [filter, setFilter] = useState({ categoryName: "" });
    const [error, setError] = useState("");
    const printRef = useRef();

    useEffect(() => {
        fetchCategories();
        fetchProducts();
    }, []);

    const fetchProducts = async (categoryName = "") => {
        try {
            const params = {};
            if (categoryName) params.categoryName = categoryName;

            const res = await axios.get("http://localhost:5112/api/products/filter", {
                params,
                withCredentials: true
            });

            setProducts(res.data);
        } catch {
            setError("Не вдалося завантажити продукти");
        }
    };

    const fetchCategories = () => {
        axios.get("http://localhost:5112/api/categories", { withCredentials: true })
            .then((res) => setCategories(res.data))
            .catch(() => setError("Не вдалося завантажити категорії"));
    };

    const handlePrint = () => {
        const printWindow = window.open("", "_blank", "width=800,height=600");
        if (!printWindow) return;

        const cloned = printRef.current.cloneNode(true);
        const content = cloned.innerHTML;

        const html = `
        <html lang="uk">
        <head>
          <title>Звіт по продуктах</title>
          <style>
            body { font-family: sans-serif; padding: 20px; }
            h2 { text-align: center; margin-bottom: 20px; }
            table { width: 100%; border-collapse: collapse; margin-top: 20px; }
            th, td { border: 1px solid #333; padding: 8px; text-align: left; font-size: 14px; }
            th { background: #f0f0f0; }
          </style>
        </head>
        <body>
          <h2>Звіт по продуктах</h2>
          ${content}
        </body>
        </html>
        `;

        printWindow.document.write(html);
        printWindow.document.close();
        printWindow.focus();
        setTimeout(() => printWindow.print(), 500);
    };

    const handleFilter = (e) => {
        e.preventDefault();
        fetchProducts(filter.categoryName);
    };

    return (
        <div className="container-fluid bg-light min-vh-100 py-4">
            <div className="container">
                <div className="d-flex justify-content-between align-items-center mb-4 border-bottom pb-2">
                    <div>
                        <h2 className="fw-bold mb-0">Товари</h2>
                        <small className="text-muted">Перегляд товарів у магазині</small>
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
                    <div className="col-md-6">
                        <label className="form-label">Категорія товару</label>
                        <select
                            className="form-select"
                            value={filter.categoryName}
                            onChange={(e) => setFilter({ categoryName: e.target.value })}
                        >
                            <option value="">Усі категорії</option>
                            {categories.map(c => (
                                <option key={c.categoryNumber} value={c.categoryName}>
                                    {c.categoryName}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="col-md-3">
                        <button type="submit" className="btn btn-outline-dark w-100">Застосувати фільтр</button>
                    </div>
                    <div className="col-md-3">
                        <button
                            type="button"
                            className="btn btn-secondary w-100"
                            onClick={() => {
                                setFilter({ categoryName: "" });
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
                            <th>Категорія</th>
                            <th>Характеристики</th>
                        </tr>
                        </thead>
                        <tbody>
                        {products.map(p => (
                            <tr key={p.idProduct}>
                                <td>{p.productName}</td>
                                <td>{p.categoryName}</td>
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
