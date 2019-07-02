import { Feature } from './features';

export interface Product {
  id: string;
  name: string;
  description: string;
  features?: Feature[];
}
