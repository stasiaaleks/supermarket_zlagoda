import React, { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function CategoriesPage() {
    const [categories, setCategories] = useState([]);
    const [error, setError] = useState("");
    const [form, setForm] = useState({ categoryNumber: "", categoryName: "" });
    const [filter, setFilter] = useState({ categoryName: "" });
    const [editMode, setEditMode] = useState(false);
    const printRef = useRef();

    useEffect(() => {
        fetchCategories();
    }, []);

    const fetchCategories = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/categories/filter", {
                withCredentials: true
            });
            setCategories(res.data);
        } catch (err) {
            setError("Помилка при завантаженні категорій");
        }
    };

    const handleFilter = async () => {
        try {
            const params = {};
            if (filter.categoryName) params.CategoryName = filter.categoryName;

            const res = await axios.get("http://localhost:5112/api/categories/filter", {
                params,
                withCredentials: true,
            });
            setCategories(res.data);
        } catch (err) {
            setError("Помилка при фільтрації");
        }
    };

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleEdit = (c) => {
        setForm(c);
        setEditMode(true);
    };

    const handleDelete = async (id) => {
        if (!window.confirm("Ви впевнені, що хочете видалити цю категорію?")) return;
        try {
            await axios.delete(`http://localhost:5112/api/categories/${id}`, { withCredentials: true });
            fetchCategories();
        } catch (err) {
            setError("Помилка при видаленні");
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (editMode) {
                await axios.put("http://localhost:5112/api/categories", form, { withCredentials: true });
            } else {
                await axios.post("http://localhost:5112/api/categories", form, { withCredentials: true });
            }
            setForm({ categoryNumber: "", categoryName: "" });
            setEditMode(false);
            fetchCategories();
        } catch (err) {
            setError("Помилка при збереженні");
        }
    };

    const handlePrint = () => {
        const printWindow = window.open("", "_blank", "width=800,height=600");
        if (!printWindow) return;

        const cloned = printRef.current.cloneNode(true);
        cloned.querySelectorAll("th:last-child, td:last-child").forEach(el => el.remove());
        const content = cloned.innerHTML;
        const html = `
            <html>
            <head>
                <title>Звіт про категорії</title>
                <style>
                    body { font-family: sans-serif; padding: 20px; }
                    table { width: 100%; border-collapse: collapse; }
                    th, td { border: 1px solid #333; padding: 8px; text-align: left; }
                    th { background: #eee; }
                </style>
            </head>
            <body>
                <h2>Звіт про категорії товарів</h2>
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
                        <h2 className="fw-bold mb-0">Категорії товарів</h2>
                        <small className="text-muted">Керування категоріями супермаркету</small>
                    </div>
                    <div className="d-flex gap-2">
                        <button onClick={handlePrint} className="btn btn-outline-dark">Друк звіту</button>
                        <button onClick={() => window.location.href = "/manager"} className="btn btn-outline-secondary">Головне меню</button>
                    </div>
                </div>

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={(e) => { e.preventDefault(); handleFilter(); }} className="row g-3 mb-4">
                    <div className="col-md-6">
                        <input
                            name="categoryName"
                            value={filter.categoryName}
                            onChange={(e) => setFilter({ categoryName: e.target.value })}
                            className="form-control"
                            placeholder="Назва категорії для фільтру"
                        />
                    </div>
                    <div className="col-md-2">
                        <button type="submit" className="btn btn-outline-dark w-100">Фільтрувати</button>
                    </div>
                    <div className="col-md-2">
                        <button type="button" onClick={() => { setFilter({ categoryName: "" }); fetchCategories(); }} className="btn btn-secondary w-100">Очистити</button>
                    </div>
                </form>

                <form onSubmit={handleSubmit} className="row g-3 mb-4">
                    <div className="col-md-6">
                        <input
                            name="categoryName"
                            value={form.categoryName}
                            onChange={handleChange}
                            className="form-control"
                            placeholder="Назва категорії"
                            required
                        />
                    </div>

                    <div className="col-md-2">
                        <button type="submit" className="btn btn-success w-100">
                            {editMode ? "Зберегти" : "Додати"}
                        </button>
                    </div>
                </form>

                <div ref={printRef} className="bg-white p-3 shadow-sm rounded border">
                    <table className="table table-hover">
                        <thead className="table-light">
                        <tr>
                            <th>Назва</th>
                            <th>Дії</th>
                        </tr>
                        </thead>
                        <tbody>
                        {categories.map((c) => (
                            <tr key={c.categoryNumber}>
                                <td>{c.categoryName}</td>
                                <td>
                                    <button className="btn btn-sm btn-outline-primary me-2" onClick={() => handleEdit(c)}>Редагувати</button>
                                    <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(c.categoryNumber)}>Видалити</button>
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
