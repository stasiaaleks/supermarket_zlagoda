import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function StoreProductsPage() {
    const [products, setProducts] = useState([]);
    const [error, setError] = useState("");
    const [filter, setFilter] = useState({ productName: "", categoryName: "", upc: "" });
    const [promoType, setPromoType] = useState("");
    const [form, setForm] = useState({
        upc: "",
        productName: "",
        characteristics: "",
        sellingPrice: "",
        productsNumber: "",
        promotionalProduct: false
    });
    const [editMode, setEditMode] = useState(false);
    const printRef = useRef();

    useEffect(() => { fetchAll(); }, []);

    const fetchAll = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/store-products/filter", { withCredentials: true });
            setProducts(res.data);
        } catch {
            setError("Помилка при завантаженні товарів");
        }
    };

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setForm(prev => ({ ...prev, [name]: type === "checkbox" ? checked : value }));
    };

    const handleEdit = (product) => {
        setForm(product);
        setEditMode(true);
    };

    const handleDelete = async (upc) => {
        if (!window.confirm("Ви впевнені, що хочете видалити товар?")) return;
        try {
            await axios.delete(`http://localhost:5112/api/store-products/${upc}`, { withCredentials: true });
            fetchAll();
        } catch {
            setError("Помилка при видаленні товару");
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const dto = {
                ...form,
                sellingPrice: parseFloat(form.sellingPrice),
                productsNumber: parseInt(form.productsNumber),
            };
            if (editMode) {
                await axios.put("http://localhost:5112/api/store-products", dto, { withCredentials: true });
            } else {
                await axios.post("http://localhost:5112/api/store-products", dto, { withCredentials: true });
            }
            setForm({ upc: "", productName: "", characteristics: "", sellingPrice: "", productsNumber: "", promotionalProduct: false });
            setEditMode(false);
            fetchAll();
        } catch {
            setError("Помилка при збереженні товару");
        }
    };

    const handleFilter = async () => {
        try {
            const params = {};
            if (filter.productName) params.productName = filter.productName;
            if (filter.categoryName) params.categoryName = filter.categoryName;
            if (filter.upc) {
                // Пошук за конкретним UPC
                const res = await axios.get(`http://localhost:5112/api/store-products/${filter.upc}`, { withCredentials: true });
                setProducts([res.data]);
                return;
            }

            let endpoint = "http://localhost:5112/api/store-products/filter";
            if (promoType === "promo") endpoint = "http://localhost:5112/api/store-products/promotional";
            if (promoType === "regular") endpoint = "http://localhost:5112/api/store-products/regular";

            const res = await axios.get(endpoint, { params, withCredentials: true });
            setProducts(res.data);
        } catch {
            setError("Помилка при фільтрації товарів");
        }
    };

    const handlePrint = () => {
        const printWindow = window.open("", "_blank", "width=800,height=600");
        if (!printWindow) return;

        const cloned = printRef.current.cloneNode(true);
        cloned.querySelectorAll("th:last-child, td:last-child").forEach(el => el.remove());
        const content = cloned.innerHTML;

        const html = `
            <html lang="uk">
            <head>
                <title>Звіт по товарах</title>
                <style>
                    body { font-family: sans-serif; padding: 20px; }
                    table { width: 100%; border-collapse: collapse; }
                    th, td { border: 1px solid #333; padding: 8px; text-align: left; }
                    th { background: #eee; }
                </style>
            </head>
            <body>
                <h2>Звіт по товарах</h2>
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
                    <small className="text-muted">Управління товарами супермаркету</small>
                </div>
                <div className="d-flex gap-2">
                    <button onClick={handlePrint} className="btn btn-outline-dark">Друк звіту</button>
                    <button onClick={() => window.location.href = "/manager"} className="btn btn-outline-secondary">Головне меню</button>
                </div>
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <form onSubmit={handleSubmit} className="row g-3 mb-4">
                <div className="col-md-2">
                    <input name="upc" value={form.upc} onChange={handleChange} className="form-control" placeholder="UPC" required />
                </div>
                <div className="col-md-2">
                    <input name="productName" value={form.productName} onChange={handleChange} className="form-control" placeholder="Назва" required />
                </div>
                <div className="col-md-2">
                    <input name="characteristics" value={form.characteristics} onChange={handleChange} className="form-control" placeholder="Характеристики" />
                </div>
                <div className="col-md-2">
                    <input name="sellingPrice" type="number" value={form.sellingPrice} onChange={handleChange} className="form-control" placeholder="Ціна" required />
                </div>
                <div className="col-md-2">
                    <input name="productsNumber" type="number" value={form.productsNumber} onChange={handleChange} className="form-control" placeholder="Кількість" required />
                </div>
                <div className="col-md-1 form-check d-flex align-items-center">
                    <input name="promotionalProduct" type="checkbox" checked={form.promotionalProduct} onChange={handleChange} className="form-check-input me-2" />
                    <label className="form-check-label">Акція</label>
                </div>
                <div className="col-md-1 d-grid">
                    <button type="submit" className="btn btn-success">{editMode ? "Оновити" : "Додати"}</button>
                </div>
            </form>

            <form onSubmit={(e) => { e.preventDefault(); handleFilter(); }} className="row g-3 align-items-end mb-4">
                <div className="col-md-2">
                    <label className="form-label">UPC</label>
                    <input name="upc" value={filter.upc} onChange={(e) => setFilter({ ...filter, upc: e.target.value })} className="form-control" />
                </div>
                <div className="col-md-2">
                    <label className="form-label">Тип товару</label>
                    <select className="form-select" value={promoType} onChange={(e) => setPromoType(e.target.value)}>
                        <option value="">Усі</option>
                        <option value="promo">Акційні</option>
                        <option value="regular">Не акційні</option>
                    </select>
                </div>
                <div className="col-md-2 d-grid">
                    <button type="submit" className="btn btn-outline-dark">Фільтрувати</button>
                </div>
                <div className="col-md-2 d-grid">
                    <button type="button" className="btn btn-secondary" onClick={() => {
                        setFilter({ productName: "", categoryName: "", upc: "" });
                        setPromoType("");
                        fetchAll();
                    }}>Очистити</button>
                </div>
            </form>

            <div ref={printRef} className="bg-white p-3 shadow-sm rounded border">
                <table className="table table-hover">
                    <thead className="table-light">
                    <tr>
                        <th>UPC</th>
                        <th>Назва</th>
                        <th>Характеристики</th>
                        <th>Ціна (₴)</th>
                        <th>Кількість</th>
                        <th>Акція</th>
                        <th>Дії</th>
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
                            <td>{p.promotionalProduct ? "Так" : "Ні"}</td>
                            <td>
                                <button className="btn btn-sm btn-outline-primary me-2" onClick={() => handleEdit(p)}>Редагувати</button>
                                <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(p.upc)}>Видалити</button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
