import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { ITogglesService } from './itoggles.service';
import { Toggle } from './toggle.model';
import { ToggleParameter } from './toggle-parameter.model';
import { ToggleParameterDetail } from './toggle-parameter-detail.model';

@injectable()
export class TogglesService implements ITogglesService {
  public async detail(id: number): Promise<Toggle> {
    const response = await fetch(`${settings.ApiUrl}/v1/toggles/${id}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch toggle ${id}`);
    }

    return response.json();
  }

  public async params(toggle: Toggle): Promise<ToggleParameterDetail[]> {
    const response = await fetch(`${settings.ApiUrl}/v1/toggles/parameters/${toggle.typeName}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch toggle ${toggle.typeName}`);
    }

    const results = await response.json();

    return results.parameters;
  }

  public async types(): Promise<any> {
    const response = await fetch(`${settings.ApiUrl}/v1/toggles/types`);

    if (!response.ok) {
      throw new Error(`Cannot fetch known toggle types`);
    }

    return response.json();
  }

  public async add(featureId: number, toggle: Toggle): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/toggles`, {
      method: 'POST',
      body: JSON.stringify({featureId, toggleType: toggle.typeName, parameters: toggle.parameters})
    });

    if (!response.ok) {
      throw new Error(`Cannot create toggle ${toggle.id}`);
    }
  }

  public async addParameter(toggle: Toggle, parameterName: string, value: any): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/toggles/${toggle.id}/parameters`, {
      method: 'POST',
      body: JSON.stringify({
        toggleId: toggle.id,
        name: parameterName,
        value
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot add parameters to toggle ${toggle.id}`);
    }
  }

  public async remove(toggle: Toggle): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/toggles/${toggle.id}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete toggle ${toggle.id}`);
    }
  }
}

