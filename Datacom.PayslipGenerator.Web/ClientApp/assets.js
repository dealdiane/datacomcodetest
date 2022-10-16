// Add lib files here that are required to be copied to output directory

const CSS = [
    //'bootstrap/dist/css/bootstrap.css',
    //'bootstrap/dist/css/bootstrap.min.css'
];
const JS = [
    // Support new ECMAScript language features by using polyfills for older browsers
    //'babel-loader/lib/cache.js'
    //'babel-polyfill/dist/polyfill.js',
    //'babel-polyfill/dist/polyfill.min.js'

    //'jquery/dist/jquery.js',
    //'jquery/dist/jquery.min.js',
];

module.exports = [...JS, ...CSS];