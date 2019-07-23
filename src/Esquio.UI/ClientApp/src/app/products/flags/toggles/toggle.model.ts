import { ToggleType } from './toggle-type.enum';
import { ToggleParameter } from './toggle-parameter.model';

export interface Toggle {
  id: number;
  type?: ToggleType;
  typeName?: ToggleType;
  parameters: ToggleParameter[];
}
