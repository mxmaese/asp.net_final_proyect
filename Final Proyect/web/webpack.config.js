/// <binding BeforeBuild='Run - Development' />
"use strict";

const webpack = require("webpack");
const path = require("path");
const CopyPlugin = require("copy-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = {
  mode: "development",

  devtool: "source-map",

  stats: {
    errorDetails: true
  },

  entry: {
    maqueta: "./src/maquetas/app.js"
  },

  devServer: {
    port: 9000,
    static: {
      directory: path.resolve(__dirname, "./wwwroot")
    },
    proxy: {
      "/api": {
        target: "http://localhost:6060",
        secure: false,
        changeOrigin: true
      }
    },
    client: {
      logging: "verbose"
    },
    devMiddleware: {
      index: "MainApp.html",
      writeToDisk: true
    }
  },

  plugins: [
    new CopyPlugin({
      patterns: [
        {
          from: "src/maquetas/layout_portal.html",
          to: path.join(__dirname, "wwwroot/index.html")
        },
        {
          from: "src/Temporal.js",
          to: path.join(__dirname, "wwwroot/dist/js/Temporal.js")
        }
      ]
    }),

    new CleanWebpackPlugin(),
    {
      apply (compiler)
      {
        compiler.hooks.done.tap("MyPlugin",
          (stats) =>
          {
            console.log(path.join(__dirname, "wwwroot/dist"));
            console.log(path.resolve(__dirname, "./wwwroot"));
          });
      }
    }
  ],

  output: {
    publicPath: "dist/",
    path: path.join(__dirname, "wwwroot/dist"),
    filename: "js/[name]/[name].bundle.js"
  },

  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: "babel-loader",
          options: {
            presets: [ "@babel/preset-env" ]
          }
        }
      },
      {
        test: /\.css$/,
        use: [ "vue-style-loader", "css-loader" ]
      },
      {
        test: /\.handlebars$/,
        use: {
          loader: "html-loader",
          options: {
            sources: false
          }
        }
      },
      {
        test: /\.png$/,
        type: "asset/resource",
        generator: {
          outputPath: "images",
          publicPath: "/dist/images/"
        }
      }
    ]
  },

  resolve: {
    alias: {
      vue: "vue/dist/vue.cjs.js"
    },
    extensions: [ ".js", ".vue" ]
  }
};
