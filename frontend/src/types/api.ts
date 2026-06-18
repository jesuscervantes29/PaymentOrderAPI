import type { Product } from './product';
import type { Order } from './order';

export interface CreateOrderRequest {
  paymentMode: string;
  products: Product[];
}

export type OrderResponse = Order;

export interface ErrorResponse {
  code: string;
  message: string;
}
