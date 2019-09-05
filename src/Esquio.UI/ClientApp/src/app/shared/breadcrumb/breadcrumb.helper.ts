import { BreadCrumbItem } from './breadcrumb.model';
import { Route } from 'vue-router';

const SEPARATOR = '/';
const ADD_ROUTE = 'add';
const EDIT_ROUTE = 'edit';

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

  return parts.map((part, key) => {
    const sufix = part === ADD_ROUTE ? ADD_ROUTE : EDIT_ROUTE;
    const name = breadcrumbTemplate[key] + sufix;
    const id = isNaN(Number(part)) ? null : Number(part);

    return {
      name,
      id
    };
  }).slice(1);
}
