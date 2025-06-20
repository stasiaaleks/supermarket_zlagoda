import React, {useState} from "react";
import axios from "axios";

export default function StatisticsPage() {
    const [surname, setSurname] = useState("");
    const [minChecks, setMinChecks] = useState(1);
    const [minProducts, setMinProducts] = useState(1);
    const [result1, setResult1] = useState([]);
    const [result2, setResult2] = useState([]);
    const [result3, setResult3] = useState([]);
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const handleQuery1 = async () => {
        try {
            setLoading(true);
            const res = await axios.get("http://localhost:5112/api/statistics/same-checks-as", {
                params: {surname},
                withCredentials: true
            });
            setResult1(res.data);
            setResult2([]);
            setResult3([]);
            setError("");
        } catch {
            setError("Помилка при виконанні запиту 1.");
            setResult1([]);
        } finally {
            setLoading(false);
        }
    };

    const handleQuery2 = async () => {
        try {
            setLoading(true);
            const res = await axios.get("http://localhost:5112/api/statistics/cashiers-min-checks-products", {
                params: {minChecks, minProducts},
                withCredentials: true
            });
            setResult2(res.data);
            setResult1([]);
            setResult3([]);
            setError("");
        } catch {
            setError("Помилка при виконанні запиту 2.");
            setResult2([]);
        } finally {
            setLoading(false);
        }
    };

    const handleQuery3 = async () => {
        try {
            setLoading(true);
            const res = await axios.get("http://localhost:5112/api/statistics/sold-only-promo-products", {
                withCredentials: true
            });
            setResult3(res.data);
            setResult1([]);
            setResult2([]);
            setError("");
        } catch {
            setError("Помилка при виконанні запиту 3.");
            setResult3([]);
        } finally {
            setLoading(false);
        }
    };

    const renderTable = (data) => (
        <div className="card-body">
            <h5 className="card-title mt-4">Результати</h5>
            <div className="table-responsive">
                <table className="table table-bordered table-hover">
                    <thead className="table-light">
                    <tr>
                        {Object.keys(data[0]).map((key) => (
                            <th key={key}>{key}</th>
                        ))}
                    </tr>
                    </thead>
                    <tbody>
                    {data.map((row, i) => (
                        <tr key={i}>
                            {Object.values(row).map((val, j) => (
                                <td key={j}>{val}</td>
                            ))}
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );

    return (
        <div className="container my-5">
            <h1 className="text-center mb-4">Статистика продажів</h1>

            {/* Запит 1 */}
            <div className="card mb-4 shadow-sm">
                <div className="card-body">
                    <h5 className="card-title mb-4">
                        Знайти всіх працівників та їхні чеки, які продавали в точності такі самі товари, що і
                        працівник <strong>@Surname</strong>, і ніякі інші.
                    </h5>
                    <div className="row g-2 align-items-end">
                        <div className="col">
                            <input
                                type="text"
                                className="form-control"
                                placeholder="Введіть прізвище працівника"
                                value={surname}
                                onChange={(e) => setSurname(e.target.value)}
                            />
                        </div>
                        <div className="col-auto">
                            <button
                                className="btn btn-primary"
                                style={{width: "180px"}}
                                onClick={handleQuery1}
                                disabled={!surname.trim()}
                            >
                                Виконати запит
                            </button>
                        </div>
                    </div>
                </div>
                {loading && result1.length === 0 && <p className="text-center text-muted mt-3">Завантаження...</p>}
                {error && result1.length === 0 && <div className="alert alert-danger text-center">{error}</div>}
                {result1.length > 0 && renderTable(result1)}
            </div>

            {/* Запит 2 */}
            <div className="card mb-4 shadow-sm">
                <div className="card-body">
                    <h5 className="card-title mb-4">
                        Знайти всіх працівників, у яких є хоча б <strong>@MinChecks</strong> чеки, в кожному з яких є
                        хоча б <strong>@MinProducts</strong> товари.
                    </h5>
                    <div className="row g-2 align-items-end">
                        <div className="col">
                            <input
                                type="number"
                                className="form-control"
                                placeholder="MinChecks"
                                value={minChecks}
                                onChange={(e) => setMinChecks(Number(e.target.value))}
                            />
                        </div>
                        <div className="col">
                            <input
                                type="number"
                                className="form-control"
                                placeholder="MinProducts"
                                value={minProducts}
                                onChange={(e) => setMinProducts(Number(e.target.value))}
                            />
                        </div>
                        <div className="col-auto">
                            <button
                                className="btn btn-primary"
                                style={{width: "180px"}}
                                onClick={handleQuery2}
                                disabled={minChecks < 1 || minProducts < 1}
                            >
                                Виконати запит
                            </button>
                        </div>
                    </div>
                </div>
                {loading && result2.length === 0 && <p className="text-center text-muted mt-3">Завантаження...</p>}
                {error && result2.length === 0 && <div className="alert alert-danger text-center">{error}</div>}
                {result2.length > 0 && renderTable(result2)}
            </div>

            {/* Запит 3 */}
            <div className="card mb-4 shadow-sm">
                <div className="card-body">
                    <div className="row align-items-end">
                        <div className="col">
                            <h5 className="card-title">
                                Знайти лише ті товари, які продавалися <strong>тільки під час акції</strong> і не були
                                продані за звичайною ціною.
                            </h5>
                        </div>
                        <div className="col-auto">
                            <button
                                className="btn btn-primary"
                                style={{width: "180px"}}
                                onClick={handleQuery3}
                            >
                                Виконати запит
                            </button>
                        </div>
                    </div>
                </div>
                {loading && result3.length === 0 && <p className="text-center text-muted mt-3">Завантаження...</p>}
                {error && result3.length === 0 && <div className="alert alert-danger text-center">{error}</div>}
                {result3.length > 0 && renderTable(result3)}
            </div>
        </div>
    );
}
