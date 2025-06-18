import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CashierProductsPage() {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [search, setSearch] = useState("");
    const [selectedCategory, setSelectedCategory] = useState("");
    const [error, setError] = useState("");
    const printRef = useRef();

    useEffect(() => {
        fetchProducts();
        fetchCategories();
    }, []);

    const fetchProducts = () => {
        axios.get("http://localhost:5112/api/products/filter", {
            params: { orderBy: "product_name" },
            withCredentials: true
        })
            .then((res) => setProducts(res.data))
            .catch(() => setError("Не вдалося завантажити продукти"));
    };

    const fetchCategories = () => {
        axios.get("http://localhost:5112/api/categories", { withCredentials: true })
            .then((res) => setCategories(res.data))
            .catch(() => setError("Не вдалося завантажити категорії"));
    };

    const filtered = products.filter(p => {
        const matchesName = p.productName.toLowerCase().includes(search.toLowerCase());
        const matchesCategory = selectedCategory === "" || p.categoryName === selectedCategory;
        return matchesName && matchesCategory;
    });

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

    return (
        <div className="container mt-4">
            <h2 className="mb-3">Товари</h2>
            {error && <div className="alert alert-danger">{error}</div>}

            <div className="row mb-3">
                <div className="col-md-4">
                    <input
                        className="form-control"
                        type="text"
                        placeholder="Пошук за назвою"
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />
                </div>
                <div className="col-md-4">
                    <select
                        className="form-select"
                        value={selectedCategory}
                        onChange={(e) => setSelectedCategory(e.target.value)}
                    >
                        <option value="">Усі категорії</option>
                        {categories.map(c => (
                            <option key={c.categoryNumber} value={c.categoryName}>{c.categoryName}</option>
                        ))}
                    </select>
                </div>
                <div className="col-md-4 text-end">
                    <button className="btn btn-outline-primary" onClick={handlePrint}>
                        Друк звіту
                    </button>
                </div>
            </div>

            <div ref={printRef}>
                <table className="table table-bordered table-striped">
                    <thead>
                    <tr>
                        <th>Назва</th>
                        <th>Категорія</th>
                        <th>Характеристики</th>
                    </tr>
                    </thead>
                    <tbody>
                    {filtered.map(p => (
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
    );
}
