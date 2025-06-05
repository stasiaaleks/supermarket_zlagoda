import axios from "./api";

export async function getAllEmployees() {
    const res = await axios.get("/api/employees");
    return res.data;
}

export async function deleteEmployeeById(id) {
    return await axios.delete(`/api/employees/${id}`);
}
