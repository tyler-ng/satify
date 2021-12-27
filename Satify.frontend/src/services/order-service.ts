import axios from "axios";
import {ISalesOrder} from "@/types/Order";
import {IServiceResponse} from "@/types/ServiceResponse";

export default class OrderService {
    API_URL = process.env.VUE_APP_API_URL;
    
    public async getOrders(): Promise<ISalesOrder[]> {
        let result: any = await axios.get(`${this.API_URL}/order/`);
        return result.data;
    }
    
    public async markOrderComplete(id: number): Promise<any> {
        let result: any = await axios.patch(`${this.API_URL}/order/complete/${id}`)
        return result.data;
    }

    public async getOrdersByCustomer(customerId: number): Promise<ISalesOrder[]> {
        let result: any = await axios.get(`${this.API_URL}/order/${customerId}`);
        return result.data;
    }
}