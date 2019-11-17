import { ToggleParameter } from './toggle-parameter.model';

export interface Toggle {
  type: string;
  friendlyName: string;
  parameters: ToggleParameter[];
}
