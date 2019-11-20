import { Flag } from './flags';

export interface Product {
  name: string;
  description: string;
  flags?: Flag[];
}
