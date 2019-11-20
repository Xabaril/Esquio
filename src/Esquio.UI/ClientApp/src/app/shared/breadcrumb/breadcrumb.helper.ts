import { Route } from 'vue-router';
import { BreadCrumbItem } from './breadcrumb.model';

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

  // TODO: Improve in future
  if (route.path.includes('users')) {
    return breadcrumb;
  }

  parts.forEach((part, key) => {
    const sufix = part === ADD_ROUTE ? ADD_ROUTE : EDIT_ROUTE;
    const name = breadcrumbTemplate[key] + sufix;
    const id = part !== ADD_ROUTE && part !== EDIT_ROUTE ? part : '';
    let productName = '';
    let flagName = '';
    let type = '';

    if (key > 1) {
      const prev = breadcrumb[key - 1];
      productName = prev.productName || prev.id;
      flagName = prev.productName && !prev.flagName ? prev.id : prev.flagName;
      type = prev.flagName && !prev.type ? prev.id : prev.type;
    }

    breadcrumb.push({
      name,
      id,
      productName: productName,
      flagName: flagName,
      type: type,
    });
  });

  return breadcrumb.slice(1);
}
