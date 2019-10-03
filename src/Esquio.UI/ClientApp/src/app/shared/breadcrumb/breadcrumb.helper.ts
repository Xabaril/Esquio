import { BreadCrumbItem } from './breadcrumb.model';
import { Route } from 'vue-router';

const SEPARATOR = '/';
export const ADD_ROUTE = 'add';
export const EDIT_ROUTE = 'edit';

/**
 * Example of url
 * /products/123/456/789
 * /products/add -> add product
 * /products/123/add -> add flag
 * /products/123/456/add -> add toggle
 * /products/:product/:flag/:toggle
 */

 // TODO: Refactor magic strings
const breadcrumbTemplate: string[] = [
  '___',
  'products-',
  'flags-',
  'toggles-'
];

export function generateBreadcrumb(route: Route): BreadCrumbItem[] {
  if (!route || !route.path) {
    return [];
  }

  const parts = route.path.split(SEPARATOR).filter(x => x);
  const breadcrumb = [];

  parts.forEach((part, key) => {
    const sufix = part === ADD_ROUTE ? ADD_ROUTE : EDIT_ROUTE;
    const name = breadcrumbTemplate[key] + sufix;
    const id = isNaN(Number(part)) ? '' : part;
    let productId = '';

    if (id && key > 0) {
      productId = breadcrumb[key - 1].productId || breadcrumb[key - 1].id;
    }

    breadcrumb.push({
      name,
      id,
      productId
    });
  });

  return breadcrumb.slice(1);
}
