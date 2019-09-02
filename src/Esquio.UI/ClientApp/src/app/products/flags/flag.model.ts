import { Toggle } from './toggles';

export interface Flag {
  id: number;
  productId: number;
  name: string;
  description: string;
  enabled: boolean;
  toggles?: Toggle[];
}
