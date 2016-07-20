// For more information on how to configure a task runner, please visit:
// https://github.com/gulpjs/gulp

var gulp = require('gulp'),
    install = require("gulp-install"),
    inject = require("gulp-inject"),
    uglify = require('gulp-uglify'),
    mainBowerFiles = require('main-bower-files'),
    concat = require('gulp-concat');
    order = require('gulp-order');
    del = require('del');

var env = process.env.NODE_ENV;

// running 'bower install' command

gulp.task('packages-install', function () {
    return gulp.src(['./bower.json'])
      .pipe(install());
});

// cleaning-up target and temp directories

gulp.task('cleanup-build-results', function () {
    return del.sync(['./build/**/*', './js/**/*', './css/**/*', './fonts/**/*']);
});

// copy main bower files to 'build' directory

gulp.task("copy-bower-files", ['packages-install', 'cleanup-build-results'], function () {
    return gulp.src(mainBowerFiles(), { base: './vendor' })
        .pipe(gulp.dest('./build/vendor'));
});

// Concatinate the contents of all .html-files in the templates
// directories and save to www/templates.js.

var templateCache = require('gulp-angular-templatecache');

gulp.task('templateCache', function () {
    return gulp.src(['client/**/*.html'])
        .pipe(order([], { base: '.' }))
        .pipe(templateCache({ root: '/client' }))
        .pipe(gulp.dest('./build/'));
});

// Copy fonts

gulp.task('bootstrap-fonts', ['copy-bower-files'], function () {
    return gulp.src(['./build/vendor/bootstrap/dist/fonts/*'])
        .pipe(gulp.dest('./fonts/'));
});

// inject scripts from ['client', 'vendor']

gulp.task('inject', ['templateCache', 'copy-bower-files', 'bootstrap-fonts'], function () {

    var vendorStream = gulp.src([
            'build/vendor/**/*.js',
            '!build/vendor/jquery/**/*'
         ])
        .pipe(order([
            'build/vendor/angular/angular.js',
            'build/vendor/angular-resource/angular-resource.js',
            'build/vendor/**/*.js'], { base: '.' }))
        .pipe(concat('vendor.js', { newLine: ';\r\n\r\n' }))
        .pipe(gulp.dest('./js/'));
		
    var clientStream = gulp.src(['client/**/*.js'])
        .pipe(order(['client/init/app.js', 'client/**/*.js'], { base: '.' }))
        .pipe(concat('client.js', { newLine: ';\r\n\r\n' }))
//        .pipe(uglify())
        .pipe(gulp.dest('./js/'));

    var templatesStream = gulp.src(['./build/templates.js'])
        .pipe(gulp.dest('./js/'));

    var cssStream = gulp.src(['./build/vendor/bootstrap/dist/css/bootstrap.css', './client/css/**/*.css'])
        .pipe(gulp.dest('./css/'));

    gulp.src('./client/index.html')
        .pipe(inject(cssStream, { name: 'styles', addRootSlash: false }))
        .pipe(inject(vendorStream, { name: 'vendor', addRootSlash: false }))
        .pipe(inject(clientStream, { name: 'client', addRootSlash: false }))
        .pipe(inject(templatesStream, { name: 'templates', addRootSlash: false }))
        .pipe(gulp.dest('./'));
});

// configuring default tasks sequence

gulp.task('default', ['inject']);
