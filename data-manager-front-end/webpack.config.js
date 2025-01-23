const HTMLWebpackPlugin = require("html-webpack-plugin");

module.exports = {
    mode: "development",
    entry: "./src/index.js",
    devServer: {
        port: 4001,
        allowedHosts: "all",
        historyApiFallback: true,
        proxy: [
            {
                context: ["/api"],
                target:
                    process.env.services__weatherapi__https__0 ||
                    process.env.services__weatherapi__http__0,
                pathRewrite: {"^/api": ""},
                secure: false,
            },
        ],
    },
    output: {
        path: `${__dirname}/dist`,
        filename: "bundle.js",
        publicPath: "/",
    },
    plugins: [
        new HTMLWebpackPlugin({
            template: "./src/index.html",
            favicon: "./src/favicon.ico",
        }),
    ],
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: [
                            "@babel/preset-env",
                            ["@babel/preset-react", {runtime: "automatic"}],
                        ],
                    },
                },
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader',
                    'postcss-loader'
                ]
            },
            {
                test: /\.(jpe?g|png|gif|svg)(\?.*)?$/i,
                loader: 'file-loader',
                options: {
                    name: '[path][name].[hash].[ext]'
                },
            }
        ],
    },
    performance: {
        hints: false,
        maxEntrypointSize: 512000,
        maxAssetSize: 512000
    }
};