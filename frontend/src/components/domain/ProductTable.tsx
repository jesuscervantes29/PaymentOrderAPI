import type { ProductCatalog } from '../../types/product';

interface ProductTableProps {
  products: ProductCatalog[];
  selectedNames: Set<string>;
  onToggle: (name: string) => void;
}

export function ProductTable({ products, selectedNames, onToggle }: ProductTableProps) {
  return (
    <table className="w-full text-sm">
      <thead>
        <tr className="bg-gray-50 text-gray-500 uppercase text-xs">
          <th className="px-4 py-3 text-left w-10">
            <input
              type="checkbox"
              className="accent-blue-600"
              checked={products.length > 0 && products.filter(p => p.isAvailable).every(p => selectedNames.has(p.name))}
              onChange={e => {
                const available = products.filter(p => p.isAvailable).map(p => p.name);
                if (e.target.checked) available.forEach(n => onToggle(selectedNames.has(n) ? '' : n));
                else available.forEach(n => { if (selectedNames.has(n)) onToggle(n); });
              }}
            />
          </th>
          <th className="px-4 py-3 text-left">Producto</th>
          <th className="px-4 py-3 text-left">Detalles</th>
          <th className="px-4 py-3 text-center">Status</th>
          <th className="px-4 py-3 text-right">Precio unitario</th>
        </tr>
      </thead>
      <tbody className="divide-y divide-gray-100">
        {products.map(p => (
          <tr
            key={p.name}
            onClick={() => p.isAvailable && onToggle(p.name)}
            className={`transition-colors
              ${p.isAvailable ? 'cursor-pointer hover:bg-blue-50' : 'opacity-50 cursor-not-allowed'}
              ${selectedNames.has(p.name) ? 'bg-blue-50' : ''}
            `}
          >
            <td className="px-4 py-3">
              <input
                type="checkbox"
                readOnly
                disabled={!p.isAvailable}
                checked={selectedNames.has(p.name)}
                className="accent-blue-600"
              />
            </td>
            <td className="px-4 py-3 font-medium text-gray-800">{p.name}</td>
            <td className="px-4 py-3 text-gray-500">{p.details}</td>
            <td className="px-4 py-3 text-center">
              <span className={`text-xs font-semibold px-2 py-1 rounded-full ${p.isAvailable ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-500'}`}>
                {p.isAvailable ? 'Available' : 'Not Available'}
              </span>
            </td>
            <td className="px-4 py-3 text-right text-gray-600">${p.unitPrice.toFixed(2)}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
