import { ToggleParameter } from './toggle-parameter.model';

export interface Toggle {
  id: number;
  type?: string;
  typeName?: string;
  parameters: ToggleParameter[];
}
