// src/pages/CashierStoreProductsPage.js
import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CashierStoreProductsPage() {
    const [products, setProducts] = useState([]);
    const [search, setSearch] = useState("");
    const [promoFilter, setPromoFilter] = useState("");
    const [error, setError] = useState("");
    const printRef = useRef();

    useEffect(() => {
        axios.get("http://localhost:5112/api/store-products", { withCredentials: true })
            .then((res) => setProducts(res.data))
            .catch(() => setError("Не вдалося завантажити товари"));
    }, []);

    const filteredProducts = products.filter(p => {
        const matchesSearch = p.productName.toLowerCase().includes(search.toLowerCase());
        const matchesPromo = promoFilter === ""
            || (promoFilter === "promo" && p.promotionalProduct)
            || (promoFilter === "regular" && !p.promotionalProduct);
        return matchesSearch && matchesPromo;
    });

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
        <div className="container mt-4">
            <h2 className="mb-3">Товари у магазині</h2>
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
                        value={promoFilter}
                        onChange={(e) => setPromoFilter(e.target.value)}
                    >
                        <option value="">Усі товари</option>
                        <option value="promo">Акційні</option>
                        <option value="regular">Звичайні</option>
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
                        <th>UPC</th>
                        <th>Ціна</th>
                        <th>Кількість</th>
                        <th>Акційний</th>
                        <th>Характеристики</th>
                    </tr>
                    </thead>
                    <tbody>
                    {filteredProducts.map((p) => (
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
