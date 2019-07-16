import fetchIntercept from 'fetch-intercept';
import nprogress from 'nprogress/nprogress.js';
import { container, cid } from 'inversify-props';
import { IAuthService } from '@/shared';


export function registerInterceptor(next = null) {
  nprogress.configure({ showSpinner: false });
  const authService = container.get<IAuthService>(cid.IAuthService);

  fetchIntercept.register({
    request: function (url, config) {
      nprogress.start();

      config = config || {};
      const headers = config.headers || {};

      config.headers = {
        ...headers,
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authService.user.access_token}`
      };
      return [url, config];
    },

    requestError: function (error) {
      return Promise.reject(error);
    },

    response: function (response) {
      if (response.status === 401 && next) {
        next({ path: '/login' });
        return response;
      }

      if (!response.ok) {
        throw new Error(response.status + ' ' + response.statusText);
      }

      nprogress.done();
      return response;
    },

    responseError: function (error) {
      nprogress.done();
      return Promise.reject(error);
    }
  });
}
