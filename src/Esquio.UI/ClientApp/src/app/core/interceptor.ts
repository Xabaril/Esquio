import fetchIntercept from 'fetch-intercept';
import nprogress from 'nprogress/nprogress.js';
import { container, cid } from 'inversify-props';
import { IAuthService } from '@/shared';


export function registerInterceptor() {
  nprogress.configure({ showSpinner: false });
  const authService = container.get<IAuthService>(cid.IAuthService);

  fetchIntercept.register({
    request: function (url, config) {
      nprogress.start();

      config = config || {};
      const headers = config.headers || {};

      config.headers = {
        ...headers,
        Authorization: `Bearer ${authService.user.access_token}`
      };
      return [url, config];
    },

    requestError: function (error) {
      return Promise.reject(error);
    },

    response: function (response) {
      nprogress.done();
      return response;
    },

    responseError: function (error) {
      nprogress.done();
      return Promise.reject(error);
    }
  });
}
