/**
 * Created by teomantuncer on 28-Aug-17.
 */
var gulp            = require('gulp'),
	stylus          = require('gulp-stylus'),
	concat          = require('gulp-concat'),
	cssmin          = require('gulp-cssmin'),
	plumber         = require('gulp-plumber'),
	uglify          = require('gulp-uglify'),
	rename          = require('gulp-rename'),
	bootstrap       = require('bootstrap-styl'),
	sourcemaps      = require('gulp-sourcemaps'),
	autoprefixer    = require('autoprefixer-stylus');
var dir    = {};
dir.main   = '../';
dir.build  = dir.main + '/';
dir.source = dir.main + 'gulp/src/';
dir.css    = dir.source + 'css/';
dir.js     = dir.source + 'js/';
var config = {
	css: {
		watch: dir.css + '**/*.styl',
		source: dir.css + '*.styl',
		target: dir.build + 'assets/css',
		file: 'sihirdarvadisi'
	},
	js: {
		watch: dir.js + '*.js',
		source: dir.js + '*.js',
		target: dir.build + 'assets/js',
		file: 'sihirdarvadisi'
	}
};
gulp.task('css', function () {
	return gulp.src(config.css.source)
	.pipe(plumber())
	.pipe(sourcemaps.init())
	.pipe(stylus({use: [bootstrap(), autoprefixer('> 1%', '> 1% in BG', 'last 2 versions', 'ie 7', 'ie 8', 'ie 9')]}))
	.pipe(sourcemaps.write())
	.pipe(concat(config.css.file + '.css'))
	.pipe(gulp.dest(config.css.target))
	.pipe(cssmin())
	.pipe(rename({suffix: '.min'}))
	.pipe(gulp.dest(config.css.target));
});
gulp.task('css:watch', function () {
	gulp.watch(config.css.watch, ['css']);
});
gulp.task('js', function () {
	return gulp.src(config.js.source)
	.pipe(plumber())
	//.pipe(stylus())
	.pipe(concat(config.js.file + '.js'))
	.pipe(gulp.dest(config.js.target))
	.pipe(uglify())
	.pipe(rename({suffix: '.min'}))
	.pipe(gulp.dest(config.js.target));
});
gulp.task('js:watch', function () {
	gulp.watch(config.js.watch, ['js']);
});
gulp.task('build', ['css', 'js']);
gulp.task('watch', ['build', 'css:watch', 'js:watch']);
gulp.task('default', ['watch']);