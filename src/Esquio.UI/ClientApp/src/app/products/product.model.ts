import { Flag } from './flags';

export interface Product {
  id?: string;
  name: string;
  description: string;
  flags?: Flag[];
}
