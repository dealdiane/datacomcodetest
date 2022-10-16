module.exports = {
    plugins: [
        // A tool for packing same CSS media query rules into one with PostCSS
        require('css-mqpacker')({
            sort: true
          }),
        // Parse CSS and add vendor prefixes to rules by Can I Use https://twitter.com/autoprefixer
        require('autoprefixer')
    ]
}