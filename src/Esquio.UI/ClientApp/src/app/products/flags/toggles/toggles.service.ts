import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { ITogglesService } from './itoggles.service';
import { ToggleParameterDetail } from './toggle-parameter-detail.model';
import { Toggle } from './toggle.model';

@injectable()
export class TogglesService implements ITogglesService {
  public async detail(productName: string, flagName: string, type: string): Promise<Toggle> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${flagName}/toggles/${type}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch toggle ${type} in feature ${flagName} and product ${productName}`);
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

  public async add(productName: string, featureName: string, toggle: Toggle): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/toggles`, {
      method: 'POST',
      body: JSON.stringify({productName, featureName, type: toggle.type, parameters: toggle.parameters})
    });

    if (!response.ok) {
      throw new Error(`Cannot create toggle ${toggle.type} in feature ${featureName} and product ${productName}`);
    }
  }

  public async addParameter(productName: string, featureName: string, toggle: Toggle, parameterName: string, value: any): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/toggles/parameters`, {
      method: 'POST',
      body: JSON.stringify({
        productName,
        featureName,
        type: toggle.type,
        name: parameterName,
        value
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot add parameters to toggle ${toggle.type} in feature ${featureName} and product ${productName}`);
    }
  }

  public async remove(productName: string, flagName: string, toggle: Toggle): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${flagName}/toggles/${toggle.type}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete toggle ${toggle.type} in feature ${flagName} and product ${productName}`);
    }
  }
}

