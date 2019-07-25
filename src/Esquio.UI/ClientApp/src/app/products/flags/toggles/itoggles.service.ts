import { Toggle } from './toggle.model';
import { ToggleParameter } from './toggle-parameter.model';
import { ToggleParameterDetail } from './toggle-parameter-detail.model';

export interface ITogglesService {
  detail(id: number): Promise<Toggle>;
  params(toggle: Toggle): Promise<ToggleParameterDetail[]>;
  types(): Promise<any>;
  add(featureId: number, toggle: Toggle): Promise<void>;
  addParameter(toggle: Toggle, parameter: ToggleParameter): Promise<void>;
  remove(toggle: Toggle): Promise<void>;
}
