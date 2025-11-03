import React, { useState, useEffect } from "react";
import { DataGrid, Column } from "devextreme-react/data-grid";
import { Button } from "devextreme-react/button";
import notify from "devextreme/ui/notify";
import api from "../Api/ApiClient";
import "./ItemsPage.css";


const ItemsPage = () => {
    const [items, setItems] = useState([]);
    const [newItem, setNewItem] = useState({ code: "", name: "", unit: "" });

    useEffect(() => {
        fetchItems();
    }, []);

    const fetchItems = async () => {
        try {
            const response = await api.get("/items");
            const mappedItems = response.data.map(item => ({
                ...item,
                _id: item._id
            }));
            setItems(mappedItems);
        } catch (error) {
            notify("Ürünler yüklenemedi!", "error", 2000);
        }
    };


    const addItem = async () => {
        if (!newItem.code || !newItem.name || !newItem.unit) {
            notify("Tüm alanları doldurun!", "warning", 2000);
            return;
        }
        try {
            const payload = { ...newItem };
            const response = await api.post("/items", payload);
            notify("Yeni ürün eklendi!", "success", 1500);
            setNewItem({ code: "", name: "", unit: "" });
            fetchItems();
        } catch (error) {
            notify("Ürün eklenemedi!", "error", 2000);
            console.error(error);
        }
    };

    return (
        <div className="page-container" style={{ padding: 20 }}>
            <h2>Ürün Listesi</h2>


            <div className="form-card">
                <input
                    placeholder="Ürün Kodu"
                    value={newItem.code}
                    onChange={(e) => setNewItem({ ...newItem, code: e.target.value })}
                />
                <input
                    placeholder="Ürün Adı"
                    value={newItem.name}
                    onChange={(e) => setNewItem({ ...newItem, name: e.target.value })}
                />
                <input
                    placeholder="Birim"
                    value={newItem.unit}
                    onChange={(e) => setNewItem({ ...newItem, unit: e.target.value })}
                />
                <Button text="Ekle" onClick={addItem} />
            </div>


            <DataGrid
                dataSource={items}
                keyExpr="id"
                showBorders={true}
                columnAutoWidth={true}
                hoverStateEnabled={true}
            >
                <Column dataField="code" caption="Ürün Kodu" />
                <Column dataField="name" caption="Ürün Adı" />
                <Column dataField="unit" caption="Birim" />
                <Column dataField="stockQuantity" caption="Stok Miktarı" />
                <Column
                    dataField="isActive"
                    caption="Durum"
                    cellRender={(cellData) => (cellData.data.isActive ? "Aktif" : "Pasif")}
                />
            </DataGrid>

        </div>
    );
};

export default ItemsPage;
