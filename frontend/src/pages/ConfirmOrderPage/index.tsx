import { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import type { Product } from '../../types/product';
import type { PaymentMode } from '../../types/order';
import { orderService } from '../../services/orderService';
import { PaymentModeSelector } from '../../components/domain/PaymentModeSelector';
import { Button } from '../../components/ui/Button';

interface LocationState {
  products: Product[];
}

export function ConfirmOrderPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { products } = (location.state as LocationState) ?? { products: [] };

  const [mode, setMode] = useState<PaymentMode | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const total = products.reduce((sum, p) => sum + p.unitPrice, 0);

  async function handleConfirm() {
    if (!mode) return;
    setLoading(true);
    setError(null);
    try {
      await orderService.createOrder({ paymentMode: mode, products });
      navigate('/orders');
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setLoading(false);
    }
  }

  if (products.length === 0) {
    navigate('/');
    return null;
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-2xl mx-auto px-4 py-10">
        <button onClick={() => navigate('/')} className="text-blue-600 text-sm mb-6 hover:underline">
          ← Volver al catálogo
        </button>

        <h1 className="text-2xl font-bold text-gray-900 mb-6">Confirmar orden</h1>

        {/* Resumen de productos */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6 mb-6">
          <h2 className="font-semibold text-gray-700 mb-3">Productos seleccionados</h2>
          <ul className="divide-y divide-gray-100">
            {products.map(p => (
              <li key={p.name} className="flex justify-between py-2 text-sm">
                <span className="text-gray-800">{p.name}</span>
                <span className="text-gray-600">${p.unitPrice.toFixed(2)}</span>
              </li>
            ))}
          </ul>
          <div className="flex justify-between pt-3 mt-2 border-t font-semibold text-gray-900">
            <span>Total</span>
            <span>${total.toFixed(2)}</span>
          </div>
        </div>

        {/* Modo de pago */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6 mb-6">
          <h2 className="font-semibold text-gray-700 mb-3">Modo de pago</h2>
          <PaymentModeSelector value={mode} onChange={setMode} />
        </div>

        {error && (
          <p className="text-red-500 text-sm mb-4 bg-red-50 border border-red-200 rounded-lg px-4 py-3">
            {error}
          </p>
        )}

        <div className="flex justify-end">
          <Button
            disabled={!mode}
            loading={loading}
            onClick={handleConfirm}
            className="px-8"
          >
            Confirmar orden
          </Button>
        </div>
      </div>
    </div>
  );
}
