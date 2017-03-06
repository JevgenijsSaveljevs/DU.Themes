/// <binding ProjectOpened='watch, fonts, css-bundle' />
'use strict';

var gulp = require('gulp'),
    cssmin = require('gulp-cssmin'),
    autoprefixer = require('gulp-autoprefixer'),
    concatCss = require('gulp-concat-css'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify');

var config = {
    src: {
        js: {
            jQueryMin: "./bower_components/jquery/dist/jquery.min.js",
            plugins: [

                // select2 with latvian Internationalisation
                "./bower_components/select2/dist/js/select2.full.min.js",
                "./bower_components/select2/dist/js/i18n/lv.js",

                // datatables 
                "./bower_components/datatables/media/js/jquery.dataTables.min.js",
                "./bower_components/datatables/media/js/dataTables.bootstrap.min.js",
            ],
            libs: [
                "./bower_components/vue/dist/vue.min.js",
                "./bower_components/moment/min/moment.min.js",

                // bootstrap
                "./bower_components/bootstrap/dist/js/bootstrap.min.js",

                 // AdminLTE
                "./bower_components/adminlte/dist/js/app.min.js",
                "./bower_components/adminlte/plugins/datepicker/bootstrap-datepicker.js",
                "./bower_components/adminlte/plugins/datepicker/locales/bootstrap-datepicker.lv.js",

                // toastr
                "./bower_components/toastr/toastr.min.js",

                // async
                "./bower_components/async/dist/async.min.js"
            ],
            ownScripts: [
                "./assets/src/**/*.js"
            ]
        },
        css: [
            "./bower_components/fontawesome/css/font-awesome.css",
            "./bower_components/bootstrap/dist/css/bootstrap.css",
            "./bower_components/toastr/toastr.css",
            "./bower_components/AdminLTE/plugins/datepicker/datepicker3.css",
            "./bower_components/AdminLTE/dist/css/AdminLTE.css",
            "./bower_components/AdminLTE/dist/css/skins/_all-skins.css",
            "./bower_components/select2/dist/css/select2.css",
            "./bower_components/select2-bootstrap-theme/dist/select2-bootstrap.css",
            "./bower_components/datatables/media/css/dataTables.bootstrap.css",
        ],
        fonts: [
            "./bower_components/font-awesome/fonts/fontawesome-webfont.*",
            "./bower_components/bootstrap/dist/fonts/glyphicons-halflings-regular.*"
        ]
    },
    dist: {
        js: "./assets/dist/js/",
        css: "./assets/dist/css/",
        fonts: "./assets/dist/fonts"
    }
}

var paths = [
    config.src.js.jQueryMin,
];

var order = paths
     .concat(config.src.js.plugins)
     .concat(config.src.js.libs)
     .concat(config.src.js.ownScripts);


gulp.task('js-bundle', function () {
    return gulp.src(order)
            .on('error', console.error.bind(console))
            .pipe(concat("bundle.js"))
            .pipe(uglify())
            .pipe(gulp.dest(config.dist.js));
});

gulp.task('css-bundle', function () {
    return gulp.src(config.src.css)
            .on('error', console.error.bind(console))
            .pipe(concat("bundle.css"))
            .pipe(autoprefixer())
            .pipe(cssmin())
            .pipe(gulp.dest(config.dist.css));
});

gulp.task('default', ['js-bundle']);

gulp.task('fonts', function () {
    return gulp.src(config.src.fonts)
            .pipe(gulp.dest(config.dist.fonts));
});


gulp.task('watch', function () {
    gulp.watch(config.src.js.ownScripts, ['js-bundle']);
});
