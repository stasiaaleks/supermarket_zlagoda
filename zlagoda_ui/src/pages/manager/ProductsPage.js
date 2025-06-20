import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function ProductsPage() {
    const [products, setProducts] = useState([]);
    const [error, setError] = useState("");
    const [form, setForm] = useState({
        categoryNumber: "",
        productName: "",
        manufacturer: "",
        characteristics: ""
    });
    const [editMode, setEditMode] = useState(false);
    const printRef = useRef();
    const [filter, setFilter] = useState({ productName: "", categoryName: "" });
    const [categoryOptions, setCategoryOptions] = useState([]);

    useEffect(() => {
        axios.get("http://localhost:5112/api/categories", { withCredentials: true })
            .then(res => setCategoryOptions(res.data))
            .catch(() => setError("Не вдалося завантажити список категорій"));
    }, []);



    useEffect(() => {
        fetchProducts();
    }, []);

    const fetchProducts = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/products/filter", { withCredentials: true });
            setProducts(res.data);
        } catch (err) {
            setError("Помилка при завантаженні товарів");
        }
    };

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleEdit = (p) => {
        setForm(p);
        setEditMode(true);
    };

    const handleDelete = async (id) => {
        if (!window.confirm("Ви впевнені, що хочете видалити цей товар?")) return;
        try {
            await axios.delete(`http://localhost:5112/api/products/${id}`, { withCredentials: true });
            fetchProducts();
        } catch (err) {
            setError("Помилка при видаленні");
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const payload = {
                ...form,
                categoryNumber: parseInt(form.categoryNumber, 10),
                idProduct: parseInt(form.idProduct, 10),
            };

            if (editMode) {
                await axios.put("http://localhost:5112/api/products", payload, { withCredentials: true });
            } else {
                await axios.post("http://localhost:5112/api/products", payload, { withCredentials: true });
            }

            setForm({
                idProduct: "",
                categoryNumber: "",
                productName: "",
                manufacturer: "",
                characteristics: ""
            });
            setEditMode(false);
            fetchProducts();
        } catch (err) {
            setError("Помилка при збереженні");
        }
    };

    const handleFilter = async () => {
        try {
            const params = {};
            if (filter.productName) params.productName = filter.productName;
            if (filter.categoryName) params.categoryName = filter.categoryName;

            const res = await axios.get("http://localhost:5112/api/products/filter", {
                params,
                withCredentials: true,
            });

            setProducts(res.data);
        } catch (err) {
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
          <title>Звіт про товари</title>
          <style>
            body { font-family: sans-serif; padding: 20px; }
            table { width: 100%; border-collapse: collapse; }
            th, td { border: 1px solid #333; padding: 8px; text-align: left; }
            th { background: #eee; }
          </style>
        </head>
        <body>
          <h2>Звіт про товари</h2>
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
                        <h2 className="fw-bold mb-0">Товари</h2>
                        <small className="text-muted">Керування товарами супермаркету</small>
                    </div>
                    <div className="d-flex gap-2">
                        <button onClick={handlePrint} className="btn btn-outline-dark">Друк звіту</button>
                        <button onClick={() => window.location.href = "/manager"} className="btn btn-outline-secondary">Головне меню</button>
                    </div>
                </div>


                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit} className="row g-3 mb-4">
                    <div className="col-md-2">
                        <select
                            name="categoryNumber"
                            value={form.categoryNumber || ""}
                            onChange={handleChange}
                            className="form-select"
                            required
                        >
                            <option value="">Оберіть категорію</option>
                            {categoryOptions.map((cat) => (
                                <option key={cat.categoryNumber} value={cat.categoryNumber}>
                                    {cat.categoryName}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="col-md-2">
                        <input
                            name="productName"
                            value={form.productName}
                            onChange={handleChange}
                            className="form-control"
                            placeholder="Назва товару"
                            required
                        />
                    </div>
                    <div className="col-md-2">
                        <input
                            name="manufacturer"
                            value={form.manufacturer || ""}
                            onChange={handleChange}
                            className="form-control"
                            placeholder="Виробник"
                            required
                        />
                    </div>
                    <div className="col-md-2">
                        <input
                            name="characteristics"
                            value={form.characteristics}
                            onChange={handleChange}
                            className="form-control"
                            placeholder="Характеристики"
                        />
                    </div>
                    <div className="col-md-3 d-flex align-items-end">
                        <button type="submit" className="btn btn-success w-100">
                            {editMode ? "Зберегти" : "Додати"}
                        </button>
                    </div>
                </form>

                <form
                    onSubmit={(e) => {
                        e.preventDefault();
                        handleFilter();
                    }}
                    className="row g-3 align-items-end mb-4"
                >
                    <div className="col-md-4">
                        <label className="form-label">Пошук за назвою товару</label>
                        <input
                            name="productName"
                            value={filter.productName}
                            onChange={(e) => setFilter({ ...filter, productName: e.target.value })}
                            className="form-control"
                            placeholder="Назва товару"
                        />
                    </div>
                    <div className="col-md-4">
                        <label className="form-label">Пошук за категорією</label>
                        <select
                            name="categoryName"
                            value={filter.categoryName}
                            onChange={(e) => setFilter({ ...filter, categoryName: e.target.value })}
                            className="form-select"
                        >
                            <option value="">Усі категорії</option>
                            {categoryOptions.map((cat) => (
                                <option key={cat.categoryNumber} value={cat.categoryName}>
                                    {cat.categoryName}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="col-md-2">
                        <button type="submit" className="btn btn-outline-dark w-100">Застосувати фільтр</button>
                    </div>
                    <div className="col-md-2">
                        <button
                            type="button"
                            onClick={() => {
                                setFilter({ productName: "", categoryNumber: "" });
                                fetchProducts();
                            }}
                            className="btn btn-secondary w-100"
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
                            <th>Виробник</th>
                            <th>Характеристики</th>
                            <th>Дії</th>
                        </tr>
                        </thead>
                        <tbody>
                        {products.map((prod) => (
                            <tr key={prod.idProduct}>
                                <td>{prod.productName}</td>
                                <td>{prod.categoryName}</td>
                                <td>{prod.manufacturer}</td>
                                <td>{prod.characteristics}</td>
                                <td>
                                    <button className="btn btn-sm btn-outline-primary me-2" onClick={() => handleEdit(prod)}>Редагувати</button>
                                    <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(prod.idProduct)}>Видалити</button>
                                </td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}