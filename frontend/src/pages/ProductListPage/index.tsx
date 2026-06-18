import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { Product } from '../../types/product';
import { orderService } from '../../services/orderService';
import { ProductTable } from '../../components/domain/ProductTable';
import { Button } from '../../components/ui/Button';
import { LoadingSpinner } from '../../components/ui/LoadingSpinner';

export function ProductListPage() {
  const navigate = useNavigate();
  const [products, setProducts] = useState<Product[]>([]);
  const [selected, setSelected] = useState<Set<string>>(new Set());
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    orderService.getProducts()
      .then(setProducts)
      .catch(e => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  function toggleProduct(name: string) {
    setSelected(prev => {
      const next = new Set(prev);
      next.has(name) ? next.delete(name) : next.add(name);
      return next;
    });
  }

  function handleContinue() {
    const selectedProducts = products.filter(p => selected.has(p.name));
    navigate('/confirm', { state: { products: selectedProducts } });
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-3xl mx-auto px-4 py-10">
        <div className="mb-6">
          <h1 className="text-2xl font-bold text-gray-900">Catálogo de productos</h1>
          <p className="text-gray-500 text-sm mt-1">Selecciona los productos para tu orden</p>
        </div>

        <div className="bg-white rounded-xl shadow-sm border border-gray-200">
          {loading && <LoadingSpinner />}
          {error && <p className="text-red-500 text-center py-8">{error}</p>}
          {!loading && !error && (
            <ProductTable
              products={products}
              selectedNames={selected}
              onToggle={toggleProduct}
            />
          )}
        </div>

        <div className="mt-6 flex justify-between items-center">
          <span className="text-sm text-gray-500">
            {selected.size} producto(s) seleccionado(s)
          </span>
          <div className="flex gap-3">
            <Button variant="secondary" onClick={() => navigate('/orders')}>
              Ver órdenes
            </Button>
            <Button
              disabled={selected.size === 0}
              onClick={handleContinue}
            >
              Crear orden →
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}
