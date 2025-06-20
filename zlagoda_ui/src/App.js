import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";

import LoginPage from "./pages/shared/LoginPage";
import DashboardManager from "./pages/manager/DashboardManager";
import DashboardCashier from "./pages/cashier/DashboardCashier";
import PrivateRoute from "./components/PrivateRoute";
import NotFoundPage from "./pages/shared/NotFoundPage";
import ProductsPage from "./pages/manager/ProductsPage";
import EmployeesPage from "./pages/manager/EmployeesPage";
import CategoriesPage from "./pages/manager/CategoriesPage";
import ChecksPage from "./pages/manager/ChecksPage";
import CardsPage from "./pages/manager/CardsPage";
import StoreProductPage from "./pages/manager/StoreProductPage";
import CashierChecksPage from "./pages/cashier/CashierChecksPage";
import CashierStoreProductPage from "./pages/cashier/CashierStoreProductPage";
import CashierProductsPage from "./pages/cashier/CashierProductsPage";
import CashierCustomerPage from "./pages/cashier/CashierCustomerCardsPage";
import ChangePasswordPage from "./pages/shared/ChangePasswordPage";
import AnalyticsPage from "./pages/manager/AnalyticsPage";
import StatisticsPage from "./pages/manager/StatisticsPage";


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


                <Route
                    path="manager/checks"
                    element={
                        <PrivateRoute role="Manager">
                            <ChecksPage />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/manager/store-products"
                    element={
                        <PrivateRoute role="Manager">
                            <StoreProductPage />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/manager/cards"
                    element={
                        <PrivateRoute role="Manager">
                            <CardsPage />
                        </PrivateRoute>
                    }
                />


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

                <Route
                    path="/manager/change-password"
                    element={
                        <PrivateRoute role="Manager">
                            <ChangePasswordPage />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/manager/analytics"
                    element={
                        <PrivateRoute role="Manager">
                            <AnalyticsPage />
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/manager/statistics"
                    element={
                        <PrivateRoute role="Manager">
                            <StatisticsPage />
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

                {/* shared */}
                <Route path="*" element={<NotFoundPage />} />
                <Route path="/change-password" element={<ChangePasswordPage />} />
            </Routes>
        </BrowserRouter>
    );
}
