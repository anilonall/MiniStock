import React, {useEffect, useState} from "react";
import { DataGrid, Column } from "devextreme-react/data-grid";
import {Button} from "devextreme-react/button";
import notify from "devextreme/ui/notify";
import api from "../Api/ApiClient";
import "./ItemsPage.css"


const StockTransactionsPage = () => {

    const [transactions, setTransactions] = useState([]);
    const [items, setItems] = useState([]);

    useEffect(() => {
        fetchTransactions();
        fetchItems();
    }, []);


    const fetchTransactions = async () => {
        try {
            const response = await api.get("/stocktransaction");
            setTransactions(response.data);
        } catch (error) {
            notify("Stok hareketleri yüklenemedi!", "error", 2000);
            console.error(error);
        }

    };
    const fetchItems = async () => {
    try{
        const response = await api.get("/items");
        setItems(response.data);
    } catch (error) {
        notify("Ürün listesi alınamadı!", "error", 2000);
        console.error(error);
    }
    };
    const addTransaction = async (itemId, type, quantity, note) => {
        try {
            const payload = {
                itemId,
                type,
                quantity: parseFloat(quantity),
                note,
                date: new Date().toISOString()
            };

            await api.post("/stocktransaction", payload);
            notify("İşlem başarıyla eklendi", "success", 1500);
            fetchTransactions();
        } catch (error) {
            notify("İşlem eklenemedi!", "error", 2000);
            console.error(error);
        }
    };
    return (
        <div  className="page-container" style={{ padding: 20 }}>
            <h2> Stok Hareketleri</h2>


            <div style={{ display: "flex", gap: 10, marginBottom: 20 }}>
                <select id="itemSelect" style={{ padding: 5 }}>
                    <option value="">Ürün seçin</option>
                    {items.map((item) => (
                        <option key={item.id} value={item.id}>
                            {item.name}
                        </option>
                    ))}
                </select>

                <select id="typeSelect" style={{ padding: 5 }}>
                    <option value="IN">Giriş</option>
                    <option value="OUT">Çıkış</option>
                </select>

                <input id="qtyInput" type="number" placeholder="Miktar" style={{ padding: 5, width: 100 }} />
                <input id="noteInput" type="text" placeholder="Not (opsiyonel)" style={{ padding: 5, width: 200 }} />

                <Button
                    text="Ekle"
                    type="default"
                    onClick={() =>
                        addTransaction(
                            document.getElementById("itemSelect").value,
                            document.getElementById("typeSelect").value,
                            document.getElementById("qtyInput").value,
                            document.getElementById("noteInput").value
                        )
                    }
                />
            </div>

            <DataGrid
                dataSource={transactions}
                keyExpr="id"
                showBorders={true}
                columnAutoWidth={true}
                hoverStateEnabled={true}
                searchPanel={{ visible: true, placeholder: "Ara..." }}
                headerFilter={{ visible: true }}

            >
                <Column dataField="itemName" caption="Ürün Adı" />
                <Column dataField="itemId" caption="Ürün ID" />
                <Column
                    dataField="type"
                    caption="Tür"
                    cellRender={(cellData) => (
                        <span style={{
                            color: cellData.data.type === "IN" ? "green" : "red",
                            fontWeight: "600"
                        }}>
      {cellData.data.type}
    </span>
                    )}
                />
                <Column dataField="quantity" caption="Miktar" />
                <Column dataField="note" caption="Not" />
                <Column
                    dataField="date"
                    caption="Tarih"
                    dataType="date"
                    format="dd/MM/yyyy HH:mm"
                />
            </DataGrid>

        </div>
    );

};
export default StockTransactionsPage;
