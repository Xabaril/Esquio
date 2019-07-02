const TerserPlugin = require('terser-webpack-plugin');

module.exports = (env) => {
  const defaultConfig = new TerserPlugin({
    terserOptions: {
      keep_classnames: true,
      keep_fnames: true
    },
  });

  const plugin = {
    production: defaultConfig
  };

  return plugin[env];
}
