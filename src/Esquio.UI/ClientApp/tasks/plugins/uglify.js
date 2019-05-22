const UglifyJSPlugin = require('uglifyjs-webpack-plugin');

module.exports = (env) => {
  const defaultConfig = new UglifyJSPlugin({
    uglifyOptions: {
      keep_classnames: true,
      keep_fnames: true,
    }
  });

  const plugin = {
    production: defaultConfig
  };

  return plugin[env];
}