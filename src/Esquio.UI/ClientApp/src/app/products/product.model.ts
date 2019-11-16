import { Flag } from './flags';

export interface Product {
  id?: string; // TODO: Remove id
  name: string;
  description: string;
  flags?: Flag[];
}
