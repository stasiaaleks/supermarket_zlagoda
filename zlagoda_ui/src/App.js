import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";

import LoginPage from "./pages/LoginPage";
import DashboardManager from "./pages/DashboardManager";
import DashboardCashier from "./pages/DashboardCashier";
import PrivateRoute from "./components/PrivateRoute";
import NotFoundPage from "./pages/NotFoundPage";
import ProductsPage from "./pages/ProductsPage";
import EmployeesPage from "./pages/EmployeesPage";
import CategoriesPage from "./pages/CategoriesPage";
import ChecksPage from "./pages/ChecksPage";
import CardsPage from "./pages/CardsPage";
import StoreProductPage from "./pages/StoreProductPage";
import CashierChecksPage from "./pages/CashierChecksPage";
import CashierStoreProductPage from "./pages/CashierStoreProductPage";
import CashierProductsPage from "./pages/CashierProductsPage";
import CashierCustomerPage from "./pages/CashierCustomerCardsPage";

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<LoginPage />} />

                <Route path="/" element={<LoginPage />} />

                {/* Панель менеджера */}
                <Route
                    path="/manager"
                    element={
                        <PrivateRoute role="Manager">
                            <DashboardManager />
                        </PrivateRoute>
                    }
                />

                <Route path="manager/checks" element={<ChecksPage />} />

                <Route path="/manager/store-products" element={<StoreProductPage />} />

                <Route path="/manager/cards" element={<CardsPage />} />

                <Route
                    path="manager/categories"
                    element={
                        <PrivateRoute role="Manager">
                            <CategoriesPage />
                        </PrivateRoute>
                    }
                />


                <Route
                    path="/manager/employees"
                    element={
                        <PrivateRoute role="Manager">
                            <EmployeesPage />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/manager/products"
                    element={
                        <PrivateRoute role="Manager">
                            <ProductsPage />
                        </PrivateRoute>
                    }
                />


                {/* Панель касира */}
                <Route
                    path="/cashier"
                    element={
                        <PrivateRoute role="Cashier">
                            <DashboardCashier />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/cashier/checks"
                    element={
                        <PrivateRoute role="Cashier">
                            <CashierChecksPage />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/cashier/products"
                    element={
                        <PrivateRoute role="Cashier">
                            <CashierProductsPage />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/cashier/store-products"
                    element={
                        <PrivateRoute role="Cashier">
                            <CashierStoreProductPage />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/cashier/clients"
                    element={
                        <PrivateRoute role="Cashier">
                            <CashierCustomerPage />
                        </PrivateRoute>
                    }
                />

                {/* 404 */}
                <Route path="*" element={<NotFoundPage />} />
            </Routes>
        </BrowserRouter>
    );
}
