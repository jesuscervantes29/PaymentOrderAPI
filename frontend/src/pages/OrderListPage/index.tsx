import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { Order, OrderStatus } from '../../types/order';
import type { Fee } from '../../types/fee';
import { orderService } from '../../services/orderService';
import { OrderTable } from '../../components/domain/OrderTable';
import { Modal } from '../../components/ui/Modal';
import { Badge } from '../../components/ui/Badge';
import { Button } from '../../components/ui/Button';
import { LoadingSpinner } from '../../components/ui/LoadingSpinner';

export function OrderListPage() {
  const navigate = useNavigate();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [loadingId, setLoadingId] = useState<number | null>(null);
  const [detailOrder, setDetailOrder] = useState<Order | null>(null);

  function fetchOrders() {
    setLoading(true);
    orderService.getOrders()
      .then(setOrders)
      .catch(e => setError(e.message))
      .finally(() => setLoading(false));
  }

  useEffect(() => { fetchOrders(); }, []);

  async function handlePay(id: number) {
    setLoadingId(id);
    try {
      await orderService.payOrder(id);
      fetchOrders();
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setLoadingId(null);
    }
  }

  async function handleCancel(id: number) {
    setLoadingId(id);
    try {
      await orderService.cancelOrder(id);
      fetchOrders();
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setLoadingId(null);
    }
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-5xl mx-auto px-4 py-10">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-2xl font-bold text-gray-900">Órdenes</h1>
            <p className="text-gray-500 text-sm mt-1">{orders.length} orden(es) registrada(s)</p>
          </div>
          <Button onClick={() => navigate('/')}>+ Nueva orden</Button>
        </div>

        {error && (
          <p className="text-red-500 text-sm mb-4 bg-red-50 border border-red-200 rounded-lg px-4 py-3">
            {error}
          </p>
        )}

        <div className="bg-white rounded-xl shadow-sm border border-gray-200">
          {loading ? (
            <LoadingSpinner />
          ) : (
            <OrderTable
              orders={orders}
              onPay={handlePay}
              onCancel={handleCancel}
              onDetails={setDetailOrder}
              loadingId={loadingId}
            />
          )}
        </div>
      </div>

      {detailOrder && (
        <Modal title={`Orden #${detailOrder.id}`} onClose={() => setDetailOrder(null)}>
          <div className="space-y-4 text-sm">
            <div className="flex justify-between">
              <span className="text-gray-500">Estado</span>
              <Badge status={detailOrder.status as OrderStatus} />
            </div>
            <div className="flex justify-between">
              <span className="text-gray-500">Proveedor</span>
              <span className="font-medium">{detailOrder.providerName}</span>
            </div>
            <div className="flex justify-between">
              <span className="text-gray-500">Modo de pago</span>
              <span>{detailOrder.paymentMode}</span>
            </div>
            <div className="flex justify-between">
              <span className="text-gray-500">ID externo</span>
              <span className="text-gray-600 text-xs">{detailOrder.externalOrderId}</span>
            </div>
            <div>
              <p className="text-gray-500 mb-2">Productos</p>
              <ul className="divide-y divide-gray-100">
                {detailOrder.products.map(p => (
                  <li key={p.name} className="flex justify-between py-1">
                    <span>{p.name}</span>
                    <span className="text-gray-600">${p.unitPrice.toFixed(2)}</span>
                  </li>
                ))}
              </ul>
            </div>
            {detailOrder.fees.length > 0 && (
              <div>
                <p className="text-gray-500 mb-2">Comisiones</p>
                <ul className="divide-y divide-gray-100">
                  {detailOrder.fees.map((f: Fee) => (
                    <li key={f.name} className="flex justify-between py-1">
                      <span>{f.name}</span>
                      <span className="text-gray-600">${f.amount.toFixed(2)}</span>
                    </li>
                  ))}
                </ul>
              </div>
            )}
            <div className="flex justify-between border-t pt-3 font-semibold">
              <span>Total</span>
              <span>${detailOrder.amount.toFixed(2)}</span>
            </div>
          </div>
        </Modal>
      )}
    </div>
  );
}
