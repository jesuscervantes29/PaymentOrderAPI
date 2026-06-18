import type { OrderStatus } from '../../types/order';

const statusClass: Record<OrderStatus, string> = {
  Created:   'bg-blue-100 text-blue-700',
  Paid:      'bg-green-100 text-green-700',
  Cancelled: 'bg-red-100 text-red-700',
};

export function Badge({ status }: { status: OrderStatus }) {
  return (
    <span className={`px-2 py-1 rounded-full text-xs font-semibold ${statusClass[status]}`}>
      {status}
    </span>
  );
}
