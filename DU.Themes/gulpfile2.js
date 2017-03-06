'use strict';

var gulp = require('gulp'),
    cssmin = require('gulp-cssmin'),
    autoprefixer = require('gulp-autoprefixer'),
    sass = require('gulp-sass'),
    concat = require('gulp-concat'),
    concatCss = require('gulp-concat-css');

var paths = {
    src: {
        css: [
            "bower_components/bootstrap/dist/css/bootstrap.css",
            "bower_components/AdminLTE/dist/css/AdminLTE.min.css",
            "bower_components/AdminLTE/dist/css/skins/skin-red-light.min.css",
            "Content/fonts/font-awesome.css",
            "Content/Site.css"
        ],
        fonts_fontAwesome: [
            "Content/fonts/font-awesome.scss"
        ],
        fonts: [
            "bower_components/fontawesome/scss/",
            "Content/fonts/variables.scss",
            "Content/fonts/font-awesome.scss"

        ],
        fontsPath: [
            "bower_components/fontawesome/fonts/**.*",
            "Content/fonts/variables.scss",
            "Content/fonts/font-awesome.scss"
        ]
    },
    dist: {
        css: "Content/css",
        fonts: "Content/fonts",
        fontsPath: "Content/fonts"
    }

};

gulp.task('css', ['fonts'], function () {
    return gulp.src(paths.src.css)
            .on('error', console.error.bind(console))
            .pipe(concat("bundle.css"))
            .pipe(autoprefixer())
            .pipe(cssmin())
            .pipe(gulp.dest(paths.dist.css));
});

gulp.task('fonts', ["move_fonts"], function () {
    return gulp.src(paths.src.fonts_fontAwesome)
            .on('error', console.error.bind(console))
            .pipe(sass({
                sourceComments: false,
                outputStyle: 'expanded',
                errLogToConsole: true,
                includePaths: paths.src.fonts
            }))
            .pipe(gulp.dest(paths.dist.fonts));
});

gulp.task('move_fonts', function () {
    return gulp.src(paths.src.fontsPath)
            .on('error', console.error.bind(console))
            .pipe(gulp.dest(paths.dist.fontsPath));
});


gulp.task('default', ['sass']);