// For more information on how to configure a task runner, please visit:
// https://github.com/gulpjs/gulp

var gulp = require('gulp'),
    install = require("gulp-install"),
    inject = require("gulp-inject"),
    uglify = require('gulp-uglify'),
    mainBowerFiles = require('main-bower-files'),
    templateCache = require('gulp-angular-templatecache');
    concat = require('gulp-concat');
    order = require('gulp-order');
    filter = require('gulp-filter');
    del = require('del');

var env = process.env.NODE_ENV;
var skipCleanup = false;

// cleaning-up target and temp directories

gulp.task('cleanup', function () {
    if (!skipCleanup) {
        del.sync(['js/**/*', 'css/**/*', 'fonts/**/*']);
    }
});

// installing bower dependencies

gulp.task('install-bower-packages', function () {
    return gulp.src(['bower.json'])
      .pipe(install());
});

// extracting main bower files and concatenating them to vendor.js

gulp.task("build-vendor-js", ['cleanup', 'install-bower-packages'], function () {
    return gulp.src(mainBowerFiles(), { base: 'vendor' })
        .pipe(filter(['**/*.js', '!vendor/jquery/**/*']))
        .pipe(order([
            'vendor/angular/angular.js',
            'vendor/angular-resource/angular-resource.js',
            'vendor/**/*.js'], { base: '.' }))
        .pipe(concat('vendor.js', { newLine: ';\r\n\r\n' }))
        .pipe(gulp.dest('js/'));
});

// copying fonts

gulp.task('copy-bootstrap-resources', ['cleanup', 'install-bower-packages'], function () {
    gulp.src('vendor/bootstrap/dist/fonts/*')
        .pipe(gulp.dest('fonts/'));
    
    gulp.src('vendor/bootstrap/dist/css/bootstrap.css')
        .pipe(gulp.dest('css/'));

});

// concatinating the contents of angular html templates and joining them to template cache

gulp.task('build-template-cache', ['cleanup'], function () {
    return gulp.src(['client/**/*.html'])
        .pipe(order([], { base: '.' }))
        .pipe(templateCache({ root: '/client' }))
        .pipe(gulp.dest('js/'));
});

// concatenating client app scripts

gulp.task('build-client-js', ['cleanup'], function () {

    var clientStream = gulp.src(['client/**/*.js'])
        .pipe(order(['client/init/app.js', 'client/**/*.js'], { base: '.' }))
        .pipe(concat('client.js', { newLine: ';\r\n\r\n' }))
        //.pipe(uglify())
        .pipe(gulp.dest('js/'));
});

// copying css resources

gulp.task('copy-client-css-resources', ['cleanup'], function () {

    var cssStream = gulp.src('client/css/**/*.css')
        .pipe(gulp.dest('css/'));
});

// injecting resource references to starting index.html template and copying it to target location
function buildStartUpHtml() {
    var vendorStream = gulp.src('js/vendor.js');		
    var clientStream = gulp.src('js/client.js');
    var templatesStream = gulp.src('js/templates.js');
    var cssStream = gulp.src('css/*.css');

    gulp.src('client/index.html')
        .pipe(inject(cssStream, { name: 'styles', addRootSlash: false }))
        .pipe(inject(vendorStream, { name: 'vendor', addRootSlash: false }))
        .pipe(inject(clientStream, { name: 'client', addRootSlash: false }))
        .pipe(inject(templatesStream, { name: 'templates', addRootSlash: false }))
        .pipe(gulp.dest('./'));
}

gulp.task('build-startup-html', buildStartUpHtml);

gulp.task('build-startup-html-with-dependencies', [
    'build-vendor-js',
    'build-client-js',
    'build-template-cache',
    'copy-client-css-resources',
    'copy-bootstrap-resources'], buildStartUpHtml);

// defining the default 'build all' task

gulp.task('default', [
    'cleanup',
    'build-client-js',
    'build-vendor-js',
    'build-template-cache',
    'copy-client-css-resources',
    'copy-bootstrap-resources',
    'build-startup-html-with-dependencies']);

// defining monitoring task for ad-hoc building

gulp.task('monitor', ['default'], function() {
    skipCleanup = true;
    gulp.watch('client/**/*.js', ['build-client-js']);
    gulp.watch('client/**/*.css', ['copy-client-css-resources']);
    gulp.watch(['client/**/*.html', '!client/index.html'], ['build-template-cache']);
    gulp.watch('client/index.html', ['build-startup-html']);
});