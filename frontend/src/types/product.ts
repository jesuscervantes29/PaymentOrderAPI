export interface Product {
  name: string;
  unitPrice: number;
}

export interface ProductCatalog extends Product {
  details: string;
  isAvailable: boolean;
}
