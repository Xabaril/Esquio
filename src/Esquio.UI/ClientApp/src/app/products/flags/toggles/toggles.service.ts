import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { ITogglesService } from './itoggles.service';
import { ToggleParameterDetail } from './toggle-parameter-detail.model';
import { Toggle } from './toggle.model';

@injectable()
export class TogglesService implements ITogglesService {
  public async detail(id: number): Promise<Toggle> {
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features/{name}/toggles/type-->${id}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch toggle ${id}`);
    }

    return response.json();
  }

  public async params(toggle: Toggle): Promise<ToggleParameterDetail[]> {
    const response = await fetch(`${settings.ApiUrl}/toggles/parameters/${toggle.type}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch toggle ${toggle.type}`);
    }

    const results = await response.json();

    return results.parameters;
  }

  public async types(): Promise<any> {
    const response = await fetch(`${settings.ApiUrl}/toggles/types`);

    if (!response.ok) {
      throw new Error(`Cannot fetch known toggle types`);
    }

    return response.json();
  }

  public async add(featureId: number, toggle: Toggle): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/toggles`, {
      method: 'POST',
      body: JSON.stringify({featureId, type: toggle.type, parameters: toggle.parameters})
      // productname, feature name, X id
    });

    if (!response.ok) {
      throw new Error(`Cannot create toggle ${toggle.id}`);
    }
  }

  public async addParameter(toggle: Toggle, parameterName: string, value: any): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/toggles/parameters`, {
      method: 'POST',
      body: JSON.stringify({
        toggleId: toggle.id, // X
        // productname
        // featurename
        //togle type
        name: parameterName,
        value
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot add parameters to toggle ${toggle.id}`);
    }
  }

  public async remove(toggle: Toggle): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features/{name}/toggles/type-->${toggle.id}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete toggle ${toggle.id}`);
    }
  }
}

