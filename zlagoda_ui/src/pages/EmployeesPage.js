import React, { useEffect, useState, useRef } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

export default function EmployeesPage() {
    const [employees, setEmployees] = useState([]);
    const [form, setForm] = useState({
        idEmployee: "",
        surname: "",
        name: "",
        patronymic: "",
        role: "Cashier",
        salary: "",
        dateOfBirth: "",
        dateOfStart: "",
        phoneNumber: "",
        city: "",
        street: "",
        zipCode: ""
    });
    const [editMode, setEditMode] = useState(false);
    const [error, setError] = useState("");
    const printRef = useRef();
    const navigate = useNavigate();

    const [surnameSearch, setSurnameSearch] = useState("");
    const [contacts, setContacts] = useState([]);
    const contactsPrintRef = useRef();

    useEffect(() => {
        fetchEmployees();
    }, []);

    const fetchEmployees = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/employees", { withCredentials: true });
            setEmployees(res.data);
        } catch (err) {
            setError("Помилка при завантаженні працівників");
        }
    };

    const fetchCashiers = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/employees/cashiers", { withCredentials: true });
            setEmployees(res.data);
        } catch (err) {
            setError("Помилка при завантаженні касирів");
        }
    };

    const handleFindContacts = async () => {
        try {
            const res = await axios.get(`http://localhost:5112/api/employees/contacts?surname=${surnameSearch}`, {
                withCredentials: true,
            });
            setContacts([res.data]);
        } catch (err) {
            setError("Контакти не знайдено");
        }
    };

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleEdit = (emp) => {
        setForm(emp);
        setEditMode(true);
    };

    const handleDelete = async (id) => {
        if (!window.confirm("Видалити цього працівника?")) return;
        try {
            await axios.delete(`http://localhost:5112/api/employees/${id}`, { withCredentials: true });
            fetchEmployees();
        } catch (err) {
            setError("Помилка при видаленні");
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (editMode) {
                await axios.put("http://localhost:5112/api/employees", form, { withCredentials: true });
            } else {
                await axios.post("http://localhost:5112/api/employees", form, { withCredentials: true });
            }
            setForm({ idEmployee: "", surname: "", name: "", patronymic: "", role: "Cashier", salary: "", dateOfBirth: "", dateOfStart: "", phoneNumber: "", city: "", street: "", zipCode: "" });
            setEditMode(false);
            fetchEmployees();
        } catch (err) {
            setError("Помилка при збереженні");
        }
    };

    const handlePrint = () => {
        const cloned = printRef.current.cloneNode(true);
        cloned.querySelectorAll("th:last-child, td:last-child").forEach(el => el.remove());
        const content = cloned.innerHTML;

        const printWindow = window.open("", "_blank", "width=800,height=600");
        if (!printWindow) return;

        const html = `
          <html>
            <head>
              <title>Звіт про працівників</title>
              <style>
                body { font-family: sans-serif; padding: 20px; }
                table { width: 100%; border-collapse: collapse; }
                th, td { border: 1px solid #333; padding: 8px; text-align: left; }
                th { background: #eee; }
              </style>
            </head>
            <body>
              <h2>Звіт про працівників</h2>
              ${content}
            </body>
          </html>
        `;

        printWindow.document.write(html);
        printWindow.document.close();
        printWindow.focus();
        setTimeout(() => printWindow.print(), 500);
    };

    const handlePrintContacts = () => {
        const cloned = contactsPrintRef.current.cloneNode(true);
        cloned.querySelector("button")?.remove();
        const content = cloned.innerHTML;
        const printWindow = window.open("", "_blank", "width=800,height=600");
        if (!printWindow) return;

        const html = `
          <html>
            <head>
              <title>Контакти працівників</title>
              <style>
                body { font-family: sans-serif; padding: 20px; }
                table { width: 100%; border-collapse: collapse; }
                th, td { border: 1px solid #333; padding: 8px; text-align: left; }
                th { background: #eee; }
              </style>
            </head>
            <body>
              <h2>Контакти працівників</h2>
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
                        <h2 className="fw-bold mb-0">Працівники</h2>
                        <small className="text-muted">Керування персоналом</small>
                    </div>
                    <div className="d-flex gap-2">
                        <button onClick={handlePrint} className="btn btn-outline-dark">Друк звіту</button>
                        <button onClick={() => navigate("/manager")} className="btn btn-outline-secondary">Головне меню</button>
                    </div>
                </div>

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit} className="row g-2 mb-4">
                    <div className="col-md-2"><input name="surname" value={form.surname} onChange={handleChange} className="form-control" placeholder="Прізвище" required /></div>
                    <div className="col-md-2"><input name="name" value={form.name} onChange={handleChange} className="form-control" placeholder="Імʼя" required /></div>
                    <div className="col-md-2"><input name="patronymic" value={form.patronymic} onChange={handleChange} className="form-control" placeholder="По батькові" /></div>
                    <div className="col-md-2"><select name="role" value={form.role} onChange={handleChange} className="form-select"><option>Cashier</option><option>Manager</option></select></div>
                    <div className="col-md-2"><input name="salary" type="number" value={form.salary} onChange={handleChange} className="form-control" placeholder="Зарплата" /></div>
                    <div className="col-md-3"><input name="dateOfBirth" type="date" value={form.dateOfBirth} onChange={handleChange} className="form-control" /></div>
                    <div className="col-md-3"><input name="dateOfStart" type="date" value={form.dateOfStart} onChange={handleChange} className="form-control" /></div>
                    <div className="col-md-3"><input name="phoneNumber" value={form.phoneNumber} onChange={handleChange} className="form-control" placeholder="Телефон" /></div>
                    <div className="col-md-2"><input name="city" value={form.city} onChange={handleChange} className="form-control" placeholder="Місто" /></div>
                    <div className="col-md-2"><input name="street" value={form.street} onChange={handleChange} className="form-control" placeholder="Вулиця" /></div>
                    <div className="col-md-2"><input name="zipCode" value={form.zipCode} onChange={handleChange} className="form-control" placeholder="Індекс" /></div>
                    <div className="col-md-2 d-grid"><button type="submit" className="btn btn-success">{editMode ? "Зберегти" : "Додати"}</button></div>
                </form>

                <div className="d-flex gap-2 mb-3">
                    <button onClick={fetchCashiers} className="btn btn-outline-primary">Показати лише касирів</button>
                    <button onClick={fetchEmployees} className="btn btn-outline-secondary">Показати всіх працівників</button>
                </div>

                <div className="input-group mb-3" style={{ maxWidth: 400 }}>
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Пошук за прізвищем"
                        value={surnameSearch}
                        onChange={(e) => setSurnameSearch(e.target.value)}
                    />
                    <button onClick={handleFindContacts} className="btn btn-dark">Знайти контакти</button>
                </div>

                {contacts.length > 0 && (
                    <div ref={contactsPrintRef} className="bg-white p-3 mb-4 shadow-sm rounded border">
                        <h5>Контакти працівників з прізвищем «{surnameSearch}»</h5>
                        <table className="table table-bordered">
                            <thead className="table-light">
                            <tr>
                                <th>Прізвище</th>
                                <th>Імʼя</th>
                                <th>Телефон</th>
                                <th>Місто</th>
                                <th>Вулиця</th>
                                <th>Індекс</th>
                            </tr>
                            </thead>
                            <tbody>
                            {contacts.map((c, i) => (
                                <tr key={i}>
                                    <td>{c.surname}</td>
                                    <td>{c.name}</td>
                                    <td>{c.phoneNumber}</td>
                                    <td>{c.city}</td>
                                    <td>{c.street}</td>
                                    <td>{c.zipCode}</td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                        <button onClick={handlePrintContacts} className="btn btn-outline-dark">Друк контактів</button>
                    </div>
                )}

                <div ref={printRef} className="bg-white p-3 shadow-sm rounded border">
                    <table className="table table-hover">
                        <thead className="table-light">
                        <tr>
                            <th>ПІБ</th><th>Посада</th><th>Телефон</th><th>Зарплата</th><th>Дата старту</th><th>Вулиця</th><th>Індекс</th><th>Дії</th>
                        </tr>
                        </thead>
                        <tbody>
                        {employees.map(emp => (
                            <tr key={emp.idEmployee}>
                                <td>{emp.surname} {emp.name} {emp.patronymic}</td>
                                <td>{emp.role}</td>
                                <td>{emp.phoneNumber}</td>
                                <td>{emp.salary} грн</td>
                                <td>{new Date(emp.dateOfStart).toLocaleDateString()}</td>
                                <td>{emp.street}</td>
                                <td>{emp.zipCode}</td>
                                <td>
                                    <button className="btn btn-sm btn-outline-primary me-2" onClick={() => handleEdit(emp)}>Редагувати</button>
                                    <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(emp.idEmployee)}>Видалити</button>
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
