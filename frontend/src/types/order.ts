import type { Product } from './product';
import type { Fee } from './fee';

export type OrderStatus = 'Created' | 'Paid' | 'Cancelled';
export type PaymentMode = 'Cash' | 'TDC' | 'Transfer';

export interface Order {
  id: number;
  externalOrderId: string;
  providerName: string;
  status: OrderStatus;
  paymentMode: PaymentMode;
  products: Product[];
  fees: Fee[];
  amount: number;
  createdAt: string;
}
