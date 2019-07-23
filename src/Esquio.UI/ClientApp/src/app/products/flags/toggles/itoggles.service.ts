import { Toggle } from './toggle.model';
import { ToggleParameter } from './toggle-parameter.model';

export interface ITogglesService {
  detail(id: number): Promise<Toggle>;
  types(): Promise<any>;
  add(featureId: number, toggle: Toggle): Promise<void>;
  addParameter(toggle: Toggle, parameter: ToggleParameter): Promise<void>;
  remove(toggle: Toggle): Promise<void>;
}
