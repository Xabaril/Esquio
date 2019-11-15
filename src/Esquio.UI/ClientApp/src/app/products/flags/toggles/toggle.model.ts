import { ToggleParameter } from './toggle-parameter.model';

export interface Toggle {
  id: number; // TODO: remove
  type: string;
  friendlyName: string;
  parameters: ToggleParameter[];
}
