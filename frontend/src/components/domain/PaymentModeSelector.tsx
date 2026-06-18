import type { PaymentMode } from '../../types/order';

const MODES: { value: PaymentMode; label: string; description: string }[] = [
  { value: 'Cash',     label: 'Efectivo',       description: 'Comisión fija $15' },
  { value: 'TDC',      label: 'Tarjeta',         description: 'Comisión variable 1–2%' },
  { value: 'Transfer', label: 'Transferencia',   description: 'Comisión variable 0.5–2.5%' },
];

interface PaymentModeSelectorProps {
  value: PaymentMode | null;
  onChange: (mode: PaymentMode) => void;
}

export function PaymentModeSelector({ value, onChange }: PaymentModeSelectorProps) {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
      {MODES.map(m => (
        <label
          key={m.value}
          className={`flex flex-col p-4 rounded-lg border-2 cursor-pointer transition-colors
            ${value === m.value ? 'border-blue-600 bg-blue-50' : 'border-gray-200 hover:border-blue-300'}`}
        >
          <input
            type="radio"
            name="paymentMode"
            value={m.value}
            checked={value === m.value}
            onChange={() => onChange(m.value)}
            className="sr-only"
          />
          <span className="font-semibold text-gray-800">{m.label}</span>
          <span className="text-xs text-gray-500 mt-1">{m.description}</span>
        </label>
      ))}
    </div>
  );
}
