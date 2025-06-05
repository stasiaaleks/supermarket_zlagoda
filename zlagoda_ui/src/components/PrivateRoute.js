import React from "react";
import { Navigate } from "react-router-dom";

export default function PrivateRoute({ children, role }) {
    const storedRole = localStorage.getItem("role");

    if (!storedRole || storedRole !== role) {
        return <Navigate to="/" replace />;
    }

    return children;
}
