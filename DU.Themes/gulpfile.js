/// <binding ProjectOpened='watch' />
'use strict';

var gulp = require('gulp'),
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
        }
    },
    dist: {
        js: "./assets/dist/js/"
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

gulp.task('default', ['js-bundle']);

gulp.task('watch', function () {
    gulp.watch(config.src.js.ownScripts, ['js-bundle']);
});
