const path = require('path');
const webpack = require('webpack');
const copyPlugin = require('copy-webpack-plugin');
const extractTextPlugin = require("extract-text-webpack-plugin");

//const { merge } = require("webpack-merge");

const assets = require('./assets');

function getConfigs(env) {
    env = env || {};
    var isProduction = env === 'production' || env.NODE_ENV === 'production';



    const transpileConfig = {
        module: {
            rules: [
                {
                    test: /\.jsx?$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "babel-loader",
                    },
                },
                {
                    test: /\.scss?$/,
                    use: [
                        {
                            loader: 'style-loader'
                        },
                        {
                            loader: "css-loader",
                            options: {
                                sourceMap: true
                            }
                        },
                        {
                            // Use postcss.config.js to customise options (e.g. autoprefixer options)
                            loader: 'postcss-loader'
                        },
                        {
                            loader: "sass-loader",
                            options: {
                                sourceMap: true
                            }
                        }
                    ]
                },
            ]
        },
        resolve: {
            extensions: ['', '.js', '.jsx', '.scss']
        },
        output: {
            path: path.resolve(__dirname, './../wwwroot/app'),
            filename: 'main.js'
        },
        plugins: [
            new webpack.ProvidePlugin({
                "React": "react",
            })
        ]
    };

    const extractCss = new extractTextPlugin({
        filename: '../css/[name].css' // relative to transpileConfig.output
    });


    const transpileSassConfig = {
        entry: path.resolve(__dirname, './src/styles/main.scss'),
        module: {
            rules: [
                {
                    test: /\.scss?$/,
                    use: extractCss.extract({
                        use: [
                            {
                                loader: "css-loader",
                                options: {
                                    sourceMap: true
                                }
                            },
                            {
                                // Use postcss.config.js to customise options (e.g. autoprefixer options)
                                loader: 'postcss-loader'
                            },
                            {
                                loader: "sass-loader",
                                options: {
                                    sourceMap: true
                                }
                            }
                        ]
                    })
                },
            ]
        },
        resolve: {
            extensions: ['.scss']
        },
        output: {
            path: path.resolve(__dirname, './../wwwroot/app'),
            filename: 'main.css'
        },
        plugins: [
            extractCss
        ]
    };

    const copyLibConfig = {
        entry: {},
        output: {
            //path: path.join(__dirname, 'wwwroot'),
            //filename: "[name].bundle.js"
        },
        plugins: []
    };

    if (assets.length) {
        copyLibConfig.plugins.push(
            new copyPlugin({
                //patterns: [
                //    //{ from: path.resolve(__dirname, './dist/main.js'), to: path.resolve(__dirname, './../wwwroot/app/main.js') }
                //]
                patterns: assets.map(asset => {
                    return {
                        from: path.resolve(__dirname, `./node_modules/${asset}`),
                        to: path.resolve(__dirname, `./../wwwroot/lib/${asset}`)
                    };
                })
            })
        );
    }

    return [transpileConfig, copyLibConfig];
}

module.exports = getConfigs;

//module.exports = {
//    module: {
//        rules: [
//            {
//                test: /\.js$/,
//                exclude: /node_modules/,
//                use: {
//                    loader: "babel-loader",
//                },
//            },
//        ],
//    },
//};