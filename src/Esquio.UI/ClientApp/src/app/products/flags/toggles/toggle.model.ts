import { ToggleParameter } from './toggle-parameter.model';

export interface Toggle {
  id: number;
  type: string;
  friendlyName: string;
  parameters: ToggleParameter[];
}
