import React, { useEffect } from "react";
import "./App.css";
import { BrowserRouter as Router, Routes, Route, Link, useLocation, useNavigate } from "react-router-dom";
import ItemsPage from "./Pages/ItemsPage";
import StockTransactionsPage from "./Pages/StockTransactionsPage";
import LoginPage from "./Pages/LoginPage";

const AppContent = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const isLoggedIn = localStorage.getItem("isLoggedIn") === "true";


    useEffect(() => {
        if (!isLoggedIn && location.pathname !== "/login") {
            navigate("/login");
        }
    }, [isLoggedIn, location.pathname, navigate]);

    const handleLogout = () => {
        localStorage.removeItem("isLoggedIn");
        navigate("/login");
    };


    if (location.pathname === "/login") {
        return <LoginPage />;
    }

    return (
        <div className="app-container">
            <nav>
                <h1 className="logo"></h1>
                <Link to="/items" className={location.pathname === "/items" ? "active" : ""}>
                    Ürünler
                </Link>
                <Link to="/transactions" className={location.pathname === "/transactions" ? "active" : ""}>
                    Stok Hareketleri
                </Link>
                <button className="logout-btn" onClick={handleLogout}>
                    Çıkış Yap
                </button>
            </nav>

            <Routes>
                <Route path="/items" element={<ItemsPage />} />
                <Route path="/transactions" element={<StockTransactionsPage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="*" element={<ItemsPage />} />
            </Routes>
        </div>
    );
};

function App() {
    return (
        <Router>
            <AppContent />
        </Router>
    );
}

export default App;
