import fetchIntercept from 'fetch-intercept';
import nprogress from 'nprogress/nprogress.js';


export function registerInterceptor() {
  nprogress.configure({ showSpinner: false });

  fetchIntercept.register({
    request: function (url, config) {
      nprogress.start();
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
