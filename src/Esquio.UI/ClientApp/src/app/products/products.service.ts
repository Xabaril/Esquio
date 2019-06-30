import { injectable } from 'inversify-props';
import { settings, ApiResponse } from '~/core';
import { IProductsService } from './iproducts.service';
import { Product } from './product.model';

@injectable()
export class ProductsService implements IProductsService {
  public async get(): Promise<ApiResponse<Product[]>> {
    const response = await fetch(`${settings.apiUrl}/v1/products`);

    if (!response.ok) {
      throw new Error('Cannot fetch products');
    }

    return response.json();
  }

  public async add(product: Product): Promise<void> {
    const response = await fetch(`${settings.apiUrl}/v1/products`, {
      method: 'POST',
      body: JSON.stringify(product),
      headers: {
        // 'Authorization': `bearer ${token}`,
        'Content-Type': 'application/json', // TODO: interceptor
      }
    });

    if (!response.ok) {
      throw new Error('Cannot fetch products');
    }
  }
}

