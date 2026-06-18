import axios from 'axios';
import type { CreateOrderRequest, OrderResponse } from '../types/api';
import type { Product } from '../types/product';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
});

api.interceptors.response.use(
  res => res,
  err => {
    const message = err.response?.data?.message ?? 'Error inesperado. Intenta de nuevo.';
    return Promise.reject(new Error(message));
  }
);

export const orderService = {
  getProducts: (): Promise<Product[]> =>
    api.get<Product[]>('/api/products').then(r => r.data),

  createOrder: (req: CreateOrderRequest): Promise<OrderResponse> =>
    api.post<OrderResponse>('/api/orders', req).then(r => r.data),

  getOrders: (): Promise<OrderResponse[]> =>
    api.get<OrderResponse[]>('/api/orders').then(r => r.data),

  getOrderById: (id: number): Promise<OrderResponse> =>
    api.get<OrderResponse>(`/api/orders/${id}`).then(r => r.data),

  cancelOrder: (id: number): Promise<void> =>
    api.patch(`/api/orders/${id}/cancel`).then(() => undefined),

  payOrder: (id: number): Promise<void> =>
    api.patch(`/api/orders/${id}/pay`).then(() => undefined),
};
