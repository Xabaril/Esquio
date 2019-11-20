import { ToggleParameterDetail } from './toggle-parameter-detail.model';
import { Toggle } from './toggle.model';

export interface ITogglesService {
  detail(productName: string, flagName: string, type: string): Promise<Toggle>;
  params(toggle: Toggle): Promise<ToggleParameterDetail[]>;
  types(): Promise<any>;
  add(productName: string, flagName: string, toggle: Toggle): Promise<void>;
  addParameter(productName: string, flagName: string, toggle: Toggle, parameterName: string, value: any): Promise<void>;
  remove(productName: string, flagName: string, toggle: Toggle): Promise<void>;
}
