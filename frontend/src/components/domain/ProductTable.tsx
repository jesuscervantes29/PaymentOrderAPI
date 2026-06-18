import type { Product } from '../../types/product';

interface ProductTableProps {
  products: Product[];
  selectedNames: Set<string>;
  onToggle: (name: string) => void;
}

export function ProductTable({ products, selectedNames, onToggle }: ProductTableProps) {
  return (
    <table className="w-full text-sm">
      <thead>
        <tr className="bg-gray-50 text-gray-500 uppercase text-xs">
          <th className="px-4 py-3 text-left w-10" />
          <th className="px-4 py-3 text-left">Producto</th>
          <th className="px-4 py-3 text-right">Precio unitario</th>
        </tr>
      </thead>
      <tbody className="divide-y divide-gray-100">
        {products.map(p => (
          <tr
            key={p.name}
            onClick={() => onToggle(p.name)}
            className={`cursor-pointer hover:bg-blue-50 transition-colors ${selectedNames.has(p.name) ? 'bg-blue-50' : ''}`}
          >
            <td className="px-4 py-3">
              <input
                type="checkbox"
                readOnly
                checked={selectedNames.has(p.name)}
                className="accent-blue-600"
              />
            </td>
            <td className="px-4 py-3 font-medium text-gray-800">{p.name}</td>
            <td className="px-4 py-3 text-right text-gray-600">${p.unitPrice.toFixed(2)}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
