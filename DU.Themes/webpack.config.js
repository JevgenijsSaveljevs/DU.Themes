/// <reference path="F:\Soft\DU.Themes\DU.Themes\Scripts/main.js" />
var webpack = require('webpack'),
    expose = require('expose-loader');

var jqueryPath = "jquery/dist/jquery.js";

module.exports = {
  entry: {
    bundle: [
    "jquery/dist/jquery.js", 
    "bootstrap/dist/js/bootstrap.js",
    "adminlte/dist/js/app.js",
    "./Scripts/main.js"
    ],
    request: "./Views/Request/index.vue.js"
  },
  output: {
    filename: "./Scripts/[name].js",
  },

  //// make jquery visible in each file
  //plugins: [
  //    new webpack.ProvidePlugin({
  //      $: jqueryPath,
  //      jQuery: jqueryPath,
  //      "window.jQuery": jqueryPath,
  //      "window.$": jqueryPath
  //    })
  //],

  // //export jquery to global namespace
  // module: {
  //   loaders: [
  //     { test: "jquery/dist/jquery.js", loader: "expose?$!expose?jQuery!jquery/dist/jquery.js" },
  //   ]
  // },

  resolve: {
    root: ['./bower_components'],
    alias: { vue: 'vue/dist/vue.js' }
  }
};