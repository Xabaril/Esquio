import { generateBreadcrumb, EDIT_ROUTE, ADD_ROUTE } from './breadcrumb.helper';
import { Route } from 'vue-router';

describe('Breadcrumb Helper', () => {
  let route: Route = null;

  beforeEach(() => {
    route = {
      path: null,
      hash: null,
      query: null,
      params: null,
      fullPath: null,
      matched: null,
      meta: null,
      name: null,
      redirectedFrom: null
    };
  });
  it('when path is null should return an empty array', () => {
    route.path = null;

    const results = generateBreadcrumb(route);
    expect(results.length).toBe(0);
  });

  it('when path is empty string should return an empty array', () => {
    route.path = '';

    const results = generateBreadcrumb(route);
    expect(results.length).toBe(0);
  });

  it('when the path does not apply a match criteria should return an empty array', () => {
    route.path = 'test';

    const results = generateBreadcrumb(route);
    expect(results.length).toBe(0);
  });

  it('when the path is products/:productId should return an array of one element', () => {
    const name = 'products';
    const productId = 1;
    route.path = `${name}/${productId}`;

    const results = generateBreadcrumb(route);
    expect(results.length).toBe(1);
    expect(results[0].id).toBe(`${productId}`);
    expect(results[0].name).toBe(`${name}-${EDIT_ROUTE}`);
  });

  it('when the path is products/:productId/:flagId should return an array of two elements', () => {
    const productsName = 'products';
    const flagsName = 'flags';
    const productId = 1;
    const flagId = 2;
    route.path = `${productsName}/${productId}/${flagId}`;

    const results = generateBreadcrumb(route);
    expect(results.length).toBe(2);
    expect(results[0].id).toBe(`${productId}`);
    expect(results[0].name).toBe(`${productsName}-${EDIT_ROUTE}`);
    expect(results[1].id).toBe(`${flagId}`);
    expect(results[1].name).toBe(`${flagsName}-${EDIT_ROUTE}`);
  });

  it('when the path is products/:productId/:flagId/:toggleId should return an array of three elements', () => {
    const productsName = 'products';
    const flagsName = 'flags';
    const togglesName = 'toggles';
    const productId = 1;
    const flagId = 2;
    const toggleId = 3;
    route.path = `${productsName}/${productId}/${flagId}/${toggleId}`;

    const results = generateBreadcrumb(route);
    expect(results.length).toBe(3);
    expect(results[0].id).toBe(`${productId}`);
    expect(results[0].name).toBe(`${productsName}-${EDIT_ROUTE}`);
    expect(results[1].id).toBe(`${flagId}`);
    expect(results[1].name).toBe(`${flagsName}-${EDIT_ROUTE}`);
    expect(results[2].id).toBe(`${toggleId}`);
    expect(results[2].name).toBe(`${togglesName}-${EDIT_ROUTE}`);
  });

  it('when the route contains add should return add instead of edit', () => {
    const productsName = 'products';
    const flagsName = 'flags';
    const productId = 1;
    route.path = `${productsName}/${productId}/${ADD_ROUTE}`;

    const results = generateBreadcrumb(route);
    expect(results.length).toBe(2);
    expect(results[0].id).toBe(`${productId}`);
    expect(results[0].name).toBe(`${productsName}-${EDIT_ROUTE}`);
    expect(results[1].name).toBe(`${flagsName}-${ADD_ROUTE}`);
  });

  it('when data are not numbers should still work', () => {
    const productsName = 'products';
    const flagsName = 'flags';
    const togglesName = 'toggles';
    const productId = '1';
    const flagId = '2';
    const toggleId = '3';
    route.path = `${productsName}/${productId}/${flagId}/${toggleId}`;

    const results = generateBreadcrumb(route);
    expect(results.length).toBe(3);
    expect(results[0].id).toBe(`${productId}`);
    expect(results[0].name).toBe(`${productsName}-${EDIT_ROUTE}`);
    expect(results[1].id).toBe(`${flagId}`);
    expect(results[1].name).toBe(`${flagsName}-${EDIT_ROUTE}`);
    expect(results[2].id).toBe(`${toggleId}`);
    expect(results[2].name).toBe(`${togglesName}-${EDIT_ROUTE}`);
  });
});
