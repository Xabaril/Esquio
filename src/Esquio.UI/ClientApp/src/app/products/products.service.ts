import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { addQueryParams, PaginatedResponse, PaginationInfo } from '~/shared';
import { IProductsService } from './iproducts.service';
import { Product } from './product.model';

@injectable()
export class ProductsService implements IProductsService {
  public async get(pagination: PaginationInfo): Promise<PaginatedResponse<Product[]>> {
    const params = {
      pageIndex: pagination.pageIndex - 1,
      pageCount: pagination.pageCount
    };

    const response = await fetch(addQueryParams(`${settings.ApiUrl}/products`, params));

    if (!response.ok) {
      throw new Error('Cannot fetch products');
    }

    return response.json();
  }

  public async detail(name: string): Promise<Product> {
    const response = await fetch(`${settings.ApiUrl}/products/${name}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch product ${name}`);
    }

    return response.json();
  }

  public async add(product: Product): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products`, {
      method: 'POST',
      body: JSON.stringify(product)
    });

    if (!response.ok) {
      throw new Error(`Cannot create product ${product.name}`);
    }
  }

  public async update(product: Product, oldProduct: Product): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${oldProduct.name}`, {
      method: 'PUT',
      body: JSON.stringify(product)
    });

    if (!response.ok) {
      throw new Error(`Cannot update product ${oldProduct.name}`);
    }
  }

  public async remove(product: Product): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${product.name}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete product ${product.name}`);
    }
  }
}

