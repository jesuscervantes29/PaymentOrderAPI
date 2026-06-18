import { Routes, Route, Navigate } from 'react-router-dom';
import { ProductListPage } from '../pages/ProductListPage';
import { ConfirmOrderPage } from '../pages/ConfirmOrderPage';
import { OrderListPage } from '../pages/OrderListPage';

export function AppRouter() {
  return (
    <Routes>
      <Route path="/"        element={<ProductListPage />} />
      <Route path="/confirm" element={<ConfirmOrderPage />} />
      <Route path="/orders"  element={<OrderListPage />} />
      <Route path="*"        element={<Navigate to="/" replace />} />
    </Routes>
  );
}
