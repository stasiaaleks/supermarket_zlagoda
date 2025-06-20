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
        productsNumber: ""
    });
    const [editMode, setEditMode] = useState(false);
    const [allProducts, setAllProducts] = useState([]);
    const [showPromoModal, setShowPromoModal] = useState(false);
    const [promoForm, setPromoForm] = useState({ baseUpc: "", newUpc: "", quantity: "" });
    const [sortBy, setSortBy] = useState("products_number");
    const printRef = useRef();

    useEffect(() => {
        fetchAll();
        fetchProductList();
    }, []);

    const fetchAll = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/store-products/filter", { withCredentials: true });
            setProducts(res.data);
        } catch {
            setError("Помилка при завантаженні товарів");
        }
    };

    const fetchProductList = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/products", { withCredentials: true });
            setAllProducts(res.data);
        } catch {
            setError("Помилка при завантаженні списку товарів");
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
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
            const selectedProduct = allProducts.find(p => p.productName === form.productName);
            const dto = {
                upc: form.upc,
                idProduct: selectedProduct?.idProduct,
                sellingPrice: parseFloat(form.sellingPrice),
                productsNumber: parseInt(form.productsNumber),
                promotionalProduct: false
            };
            if (editMode) {
                await axios.put("http://localhost:5112/api/store-products", dto, { withCredentials: true });
            } else {
                await axios.post("http://localhost:5112/api/store-products", dto, { withCredentials: true });
            }
            setForm({ upc: "", productName: "", characteristics: "", sellingPrice: "", productsNumber: "" });
            setEditMode(false);
            fetchAll();
        } catch {
            setError("Помилка при збереженні товару");
        }
    };

    const handleFilter = async () => {
        try {
            const params = {};
            params.sortBy = sortBy === "product_name" ? "ProductName" : "ProductsNumber";
            if (filter.productName) params.productName = filter.productName;
            if (filter.categoryName) params.categoryName = filter.categoryName;
            if (filter.upc) {
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

    const handlePromoChange = (e) => {
        const { name, value } = e.target;
        setPromoForm(prev => ({ ...prev, [name]: value }));
    };

    const handleCreatePromo = async () => {
        try {
            const base = products.find(p => p.upc === promoForm.baseUpc);
            if (!base) return alert("Базовий товар не знайдено");

            const baseProduct = allProducts.find(p => p.productName === base.productName);
            const dto = {
                upc: base.upc,
                upcProm: promoForm.newUpc,
                idProduct: baseProduct?.idProduct || "",
                sellingPrice: +(base.sellingPrice * 0.8).toFixed(2),
                productsNumber: parseInt(promoForm.quantity),
                promotionalProduct: true,
            };
            await axios.post("http://localhost:5112/api/store-products", dto, { withCredentials: true });
            setShowPromoModal(false);
            setPromoForm({ baseUpc: "", newUpc: "", quantity: "" });
            fetchAll();
        } catch {
            setError("Помилка при створенні акційного товару");
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
            <head><title>Звіт по товарах</title>
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
                    <select
                        name="productName"
                        value={form.productName}
                        onChange={(e) => {
                            const selectedName = e.target.value;
                            const selectedProduct = allProducts.find(p => p.productName === selectedName);
                            setForm(prev => ({
                                ...prev,
                                productName: selectedName,
                                characteristics: selectedProduct?.characteristics || ""
                            }));
                        }}
                        className="form-select"
                        required>
                        <option value="">Оберіть товар</option>
                        {allProducts.map(p => (
                            <option key={p.idProduct} value={p.productName}>{p.productName} ({p.categoryName})</option>
                        ))}
                    </select>
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
                <div className="col-md-2 d-grid">
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
                <div className="col-md-2">
                    <label className="form-label">Сортувати за</label>
                    <select
                        className="form-select"
                        value={sortBy}
                        onChange={(e) => setSortBy(e.target.value)}
                    >
                        <option value="products_number">Кількістю</option>
                        <option value="product_name">Назвою</option>
                    </select>
                </div>
                <div className="col-md-2 d-grid">
                    <button type="submit" className="btn btn-outline-dark">Фільтрувати</button>
                </div>
                <div className="col-md-2 d-grid">
                    <button type="button" className="btn btn-secondary" onClick={() => {
                        setFilter({ productName: "", categoryName: "", upc: "" });
                        setPromoType("");
                        setSortBy("product_number");
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
                                <button className="btn btn-sm btn-outline-danger me-2" onClick={() => handleDelete(p.upc)}>Видалити</button>
                                {!p.promotionalProduct && (
                                    <button className="btn btn-sm btn-outline-warning" onClick={() => setPromoForm({ baseUpc: p.upc, newUpc: "", quantity: "" }) || setShowPromoModal(true)}>
                                        Зробити акційним
                                    </button>
                                )}
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>

            {showPromoModal && (
                <div className="modal d-block" tabIndex="-1" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Створення акційного товару</h5>
                                <button type="button" className="btn-close" onClick={() => setShowPromoModal(false)}></button>
                            </div>
                            <div className="modal-body">
                                <div className="mb-3">
                                    <label className="form-label">Новий UPC (акційний)</label>
                                    <input type="text" className="form-control" name="newUpc" value={promoForm.newUpc} onChange={handlePromoChange} />
                                </div>
                                <div className="mb-3">
                                    <label className="form-label">Кількість</label>
                                    <input type="number" className="form-control" name="quantity" value={promoForm.quantity} onChange={handlePromoChange} />
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={() => setShowPromoModal(false)}>Скасувати</button>
                                <button type="button" className="btn btn-primary" onClick={handleCreatePromo}>Створити</button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
