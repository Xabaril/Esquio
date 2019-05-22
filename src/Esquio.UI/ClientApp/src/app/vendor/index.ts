// Import vendor
// If you use usage in babelrc dont import polyfilll import '@babel/polyfill';

// Import Vue vendor
import * as bootstrap from './bootstrap';
// import 'jquery'; // For bootstrap material
import 'bootstrap-material-design';

export const vendor = [
    ...(<any>Object).values(bootstrap)
];
