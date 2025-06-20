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
    const [userMap, setUserMap] = useState({});
    const [surnameSearch, setSurnameSearch] = useState("");
    const [contacts, setContacts] = useState([]);
    const contactsPrintRef = useRef();
    const [showRegisterModal, setShowRegisterModal] = useState(false);
    const [registerData, setRegisterData] = useState({ idEmployee: "", username: "", password: "" });
    const [registrationMessage, setRegistrationMessage] = useState("");
    const [accountStatusMap, setAccountStatusMap] = useState({});

    useEffect(() => {
        fetchEmployees();
    }, []);

    useEffect(() => {
        fetchUserMap();
    }, []);

    const fetchUserMap = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/employees/users", { withCredentials: true });
            const map = {};
            res.data.forEach(user => map[user.idEmployee] = true);
            setUserMap(map);
        } catch (err) {
            console.error("Помилка при завантаженні користувачів");
        }
    };


    const handleOpenRegisterModal = (emp) => {
        setRegisterData({ idEmployee: emp.idEmployee, username: "", password: "" });
        setShowRegisterModal(true);
    };

    const handleRegisterSubmit = async () => {
        try {
            await axios.post("http://localhost:5112/register/existing-employee", registerData, { withCredentials: true });
            setShowRegisterModal(false);
            setRegistrationMessage(`Користувача з прізвищем ${getSurnameById(registerData.idEmployee)} зареєстровано`);
            fetchUserMap();
        } catch (err) {
            alert("Помилка при реєстрації");
        }
    };

    const getSurnameById = (id) => {
        const emp = employees.find(e => e.idEmployee === id);
        return emp?.surname || "";
    };

    const fetchEmployees = async () => {
        try {
            const res = await axios.get("http://localhost:5112/api/employees", { withCredentials: true });
            setEmployees(res.data);

            const status = {};
            for (const emp of res.data) {
                try {
                    const accountRes = await axios.get(`http://localhost:5112/api/employees/${emp.idEmployee}`, {
                        withCredentials: true
                    });
                    status[emp.idEmployee] = accountRes.data.hasAccount;
                } catch {
                    status[emp.idEmployee] = false;
                }
            }
            setAccountStatusMap(status);

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
            setContacts(res.data);
        } catch (err) {
            setError("Контакти не знайдено");
        }
    };

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleEdit = (emp) => {
        setForm({
            ...emp,
            dateOfBirth: emp.dateOfBirth?.slice(0, 10),
            dateOfStart: emp.dateOfStart?.slice(0, 10),
        });
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
            setError(err?.response?.data?.error || "Помилка при збереженні");
        }
    };

    const handlePrint = () => {
        const cloned = printRef.current.cloneNode(true);
        cloned.querySelectorAll("tr").forEach(row => {
            const cells = row.querySelectorAll("th, td");
            if (cells.length >= 2) {
                cells[cells.length - 1].remove();
                cells[cells.length - 2].remove();
            }
        });
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
            <div className="container-fluid px-4">
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
                    <div className="col-md-2">
                        <input name="surname" value={form.surname} onChange={handleChange} className="form-control" placeholder="Прізвище" required />
                    </div>
                    <div className="col-md-2">
                        <input name="name" value={form.name} onChange={handleChange} className="form-control" placeholder="Імʼя" required />
                    </div>
                    <div className="col-md-2">
                        <input name="patronymic" value={form.patronymic} onChange={handleChange} className="form-control" placeholder="По батькові" />
                    </div>
                    <div className="col-md-2">
                        <select name="role" value={form.role} onChange={handleChange} className="form-select">
                            <option>Cashier</option>
                            <option>Manager</option>
                        </select>
                    </div>
                    <div className="col-md-2">
                        <input name="salary" type="number" value={form.salary} onChange={handleChange} className="form-control" placeholder="Зарплата" />
                    </div>
                    <div className="col-md-3">
                        <div className="input-group">
                            <span className="input-group-text">Дата народження</span>
                            <input
                                name="dateOfBirth"
                                type="date"
                                value={form.dateOfBirth}
                                onChange={handleChange}
                                className="form-control"
                            />
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="input-group">
                            <span className="input-group-text">Початок роботи</span>
                            <input
                                name="dateOfStart"
                                type="date"
                                value={form.dateOfStart}
                                onChange={handleChange}
                                className="form-control"
                            />
                        </div>
                    </div>
                    <div className="col-md-3">
                        <input name="phoneNumber" value={form.phoneNumber} onChange={handleChange} className="form-control" placeholder="Телефон" />
                    </div>
                    <div className="col-md-2">
                        <input name="city" value={form.city} onChange={handleChange} className="form-control" placeholder="Місто" />
                    </div>
                    <div className="col-md-2">
                        <input name="street" value={form.street} onChange={handleChange} className="form-control" placeholder="Вулиця" />
                    </div>
                    <div className="col-md-2">
                        <input name="zipCode" value={form.zipCode} onChange={handleChange} className="form-control" placeholder="Індекс" />
                    </div>
                    <div className="col-md-2 d-grid">
                        <button type="submit" className="btn btn-success">{editMode ? "Зберегти" : "Додати"}</button>
                    </div>
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
                {registrationMessage && (
                    <div className="alert alert-success">{registrationMessage}</div>
                )}

                {contacts.length > 0 && (
                    <div ref={contactsPrintRef} className="bg-white p-3 mb-4 shadow-sm rounded border">
                        <h5>Контакти працівників з прізвищем «{surnameSearch}»</h5>
                        <table className="table table-bordered mb-3">
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
                        <div className="text-start">
                            <button onClick={handlePrintContacts} className="btn btn-outline-dark">Друк контактів</button>
                        </div>
                    </div>
                )}

                <div ref={printRef} className="bg-white p-3 shadow-sm rounded border table-responsive">
                    <table className="table table-hover w-200">
                        <thead className="table-light">
                        <tr>
                            <th>Прізвище</th>
                            <th>Імʼя</th>
                            <th>По батькові</th>
                            <th>Посада</th>
                            <th>Зарплата</th>
                            <th>Дата народження</th>
                            <th>Дата старту</th>
                            <th>Телефон</th>
                            <th>Місто</th>
                            <th>Вулиця</th>
                            <th>Індекс</th>
                            <th className="d-print-none">Дії</th>
                            <th className="d-print-none">Акаунт</th>
                        </tr>
                        </thead>
                        <tbody>
                        {employees.map(emp => (
                            <tr key={emp.idEmployee}>
                                <td>{emp.surname}</td>
                                <td>{emp.name}</td>
                                <td>{emp.patronymic}</td>
                                <td>{emp.role.toLowerCase() === 'manager' ? 'Manager' : 'Cashier'}</td>
                                <td>{emp.salary} грн</td>
                                <td>{new Date(emp.dateOfBirth).toLocaleDateString()}</td>
                                <td>{new Date(emp.dateOfStart).toLocaleDateString()}</td>
                                <td>{emp.phoneNumber}</td>
                                <td>{emp.city}</td>
                                <td>{emp.street}</td>
                                <td>{emp.zipCode}</td>
                                <td className="d-print-none">
                                    <button className="btn btn-sm btn-outline-primary me-2" onClick={() => handleEdit(emp)}>Редагувати</button>
                                    <button className="btn btn-sm btn-outline-danger me-2" onClick={() => handleDelete(emp.idEmployee)}>Видалити</button>
                                </td>
                                <td className="d-print-none">
                                {accountStatusMap[emp.idEmployee] ? (
                                    <span className="text-success fw-semi">Зареєстровано</span>
                                    ) : (
                                    <button className="btn btn-sm btn-outline-success" onClick={() => handleOpenRegisterModal(emp)}>Зареєструвати</button>
                                )}
                                </td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>
            </div>
            {showRegisterModal && (
                <div className="modal d-block bg-dark bg-opacity-50">
                    <div className="modal-dialog">
                        <div className="modal-content p-3">
                            <h5>Реєстрація акаунта</h5>
                            <input
                                type="text"
                                className="form-control mb-2"
                                placeholder="Ім'я користувача"
                                value={registerData.username}
                                onChange={(e) => setRegisterData({ ...registerData, username: e.target.value })}
                            />
                            <input
                                type="password"
                                className="form-control mb-2"
                                placeholder="Пароль"
                                value={registerData.password}
                                onChange={(e) => setRegisterData({ ...registerData, password: e.target.value })}
                            />
                            <div className="d-flex justify-content-end gap-2">
                                <button className="btn btn-secondary" onClick={() => setShowRegisterModal(false)}>Скасувати</button>
                                <button className="btn btn-primary" onClick={handleRegisterSubmit}>Зареєструвати</button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
