import type { Order, OrderStatus } from '../../types/order';
import { Badge } from '../ui/Badge';
import { Button } from '../ui/Button';

interface OrderTableProps {
  orders: Order[];
  onPay: (id: number) => void;
  onCancel: (id: number) => void;
  onDetails: (order: Order) => void;
  loadingId: number | null;
}

export function OrderTable({ orders, onPay, onCancel, onDetails, loadingId }: OrderTableProps) {
  if (orders.length === 0) {
    return <p className="text-center text-gray-400 py-12">No hay órdenes registradas.</p>;
  }

  return (
    <div className="overflow-x-auto">
      <table className="w-full text-sm">
        <thead>
          <tr className="bg-gray-50 text-gray-500 uppercase text-xs">
            <th className="px-4 py-3 text-left">#</th>
            <th className="px-4 py-3 text-left">Proveedor</th>
            <th className="px-4 py-3 text-left">Modo de pago</th>
            <th className="px-4 py-3 text-right">Total</th>
            <th className="px-4 py-3 text-center">Estado</th>
            <th className="px-4 py-3 text-center">Acciones</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-100">
          {orders.map(order => (
            <tr key={order.id} className="hover:bg-gray-50 transition-colors">
              <td className="px-4 py-3 text-gray-500">{order.id}</td>
              <td className="px-4 py-3 font-medium text-gray-800">{order.providerName}</td>
              <td className="px-4 py-3 text-gray-600">{order.paymentMode}</td>
              <td className="px-4 py-3 text-right font-semibold">${order.amount.toFixed(2)}</td>
              <td className="px-4 py-3 text-center">
                <Badge status={order.status as OrderStatus} />
              </td>
              <td className="px-4 py-3">
                <div className="flex justify-center gap-2">
                  <Button
                    variant="secondary"
                    className="text-xs px-3 py-1"
                    onClick={() => onDetails(order)}
                  >
                    Detalle
                  </Button>
                  {order.status === 'Created' && (
                    <>
                      <Button
                        variant="primary"
                        className="text-xs px-3 py-1"
                        loading={loadingId === order.id}
                        onClick={() => onPay(order.id)}
                      >
                        Pagar
                      </Button>
                      <Button
                        variant="danger"
                        className="text-xs px-3 py-1"
                        loading={loadingId === order.id}
                        onClick={() => onCancel(order.id)}
                      >
                        Cancelar
                      </Button>
                    </>
                  )}
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
