import { Address } from "./user";

export interface OrderToCreate {
    basketId: string;
    deliveryMethodId:number;
    shipToAddress: Address;
}
export interface Order{
    id: number;
    buyerEmail?: any;
    orderDate: string;
    shipToAddress: Address;
    deliveryMethod: string;
    shippingPrice: number;
    orderItems: OrderItem[];
    subtotal: number;
    total: number;
  }
export interface OrderItem {
    productId: number;
    productName: string;
    pictureUrl: string;
    price: number;
    quantity: number;
  }
  