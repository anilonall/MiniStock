
import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5145/api",
});

export default api;
