import { Toggle } from './toggles';

export interface Flag {
  name: string;
  description: string;
  enabled: boolean;
  toggles?: Toggle[];
}
