const path = require('path');
const VueLoaderPlugin = require("vue-loader/lib/plugin");

module.exports = {
    entry: './js/app.js',
    output: {
        filename: './publish/js/app.bundle.js'
    },
    devtool: "source-map",
    mode:"production",
    module: {
        rules: [{
                test: /\.txt$/,
                use: 'raw-loader',
                exclude: /(node_modules|bower_components)/,
            },
            {
                test: /\.vue$/,
                loader: "vue-loader"
            },
            {
                test: /\.css$/,
                use: ["style-loader", "css-loader"]
            }
        ]
    },
    resolve: {
        alias: {
          vue$: "vue/dist/vue.esm.js"
        },
        extensions: ["*", ".js", ".vue", ".json"]
      },
      plugins: [
        new VueLoaderPlugin()
      ]
};