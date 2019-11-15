import { Toggle } from './toggles';

export interface Flag {
  id: number;  // TODO: remove
  productId: number; // TODO: remove
  name: string;
  description: string;
  enabled: boolean;
  toggles?: Toggle[];
}
