import { injectable } from 'inversify-props';
import { settings, PaginatedResponse } from '~/core';
import { IProductsService } from './iproducts.service';
import { Product } from './product.model';

@injectable()
export class ProductsService implements IProductsService {
  public async get(): Promise<PaginatedResponse<Product[]>> {
    const response = await fetch(`${settings.ApiUrl}/v1/products`);

    if (!response.ok) {
      throw new Error('Cannot fetch products');
    }

    return response.json();
  }

  public async detail(id: number): Promise<Product> {
    const response = await fetch(`${settings.ApiUrl}/v1/products/${id}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch product ${id}`);
    }

    return response.json();
  }

  public async add(product: Product): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/products`, {
      method: 'POST',
      body: JSON.stringify(product),
      headers: {
        // 'Authorization': `bearer ${token}`,
        'Content-Type': 'application/json', // TODO: interceptor
      }
    });

    if (!response.ok) {
      throw new Error(`Cannot create product ${product.name}`);
    }
  }

  public async update(product: Product): Promise<void> {
    // TODO: Remove this temporal behaviour
    const _product = product as any;
    _product.productId = product.id;

    const response = await fetch(`${settings.ApiUrl}/v1/products`, {
      method: 'PUT',
      body: JSON.stringify(_product),
      headers: {
        // 'Authorization': `bearer ${token}`,
        'Content-Type': 'application/json', // TODO: interceptor
      }
    });

    if (!response.ok) {
      throw new Error(`Cannot update product ${product.id}`);
    }
  }

  public async remove(product: Product): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/products/${product.id}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete product ${product.id}`);
    }
  }
}

