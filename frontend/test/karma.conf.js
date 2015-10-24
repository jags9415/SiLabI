// Karma configuration
// http://karma-runner.github.io/0.12/config/configuration-file.html
// Generated on 2015-08-10 using
// generator-karma 1.0.0

module.exports = function(config) {
  'use strict';

  config.set({
    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,

    // base path, that will be used to resolve files and exclude
    basePath: '../',

    // testing framework to use (jasmine/mocha/qunit/...)
    // as well as any additional frameworks (requirejs/chai/sinon/...)
    frameworks: [
      "jasmine"
    ],

    // list of files / patterns to load in the browser
    files: [
      // bower:js
      'bower_components/jquery/dist/jquery.js',
      'bower_components/angular/angular.js',
      'bower_components/angular-animate/angular-animate.js',
      'bower_components/angular-bootstrap/ui-bootstrap-tpls.js',
      'bower_components/angular-bootstrap-show-errors/src/showErrors.js',
      'bower_components/angular-busy/dist/angular-busy.js',
      'bower_components/angular-cache/dist/angular-cache.js',
      'bower_components/angular-i18n/angular-locale_es-cr.js',
      'bower_components/angular-jwt/dist/angular-jwt.js',
      'bower_components/angular-route/angular-route.js',
      'bower_components/angular-sanitize/angular-sanitize.js',
      'bower_components/angular-ui-mask/dist/mask.js',
      'bower_components/angular-ui-select/dist/select.js',
      'bower_components/bootstrap/dist/js/bootstrap.js',
      'bower_components/bootbox/bootbox.js',
      'bower_components/crypto-js/rollups/sha256.js',
      'bower_components/lodash/lodash.js',
      'bower_components/moment/moment.js',
      'bower_components/ng-password-strength/dist/scripts/ng-password-strength.js',
      'bower_components/ngstorage/ngStorage.js',
      'bower_components/toastr/toastr.js',
      'bower_components/angular-mocks/angular-mocks.js',
      // endbower
      "app/scripts/**/silabi.js",
      "app/scripts/**/*.js",
      "test/mock/**/*.js",
      "test/spec/**/*.js"
    ],

    // list of files / patterns to exclude
    exclude: [
    ],

    // web server port
    port: 8080,

    // Start these browsers, currently available:
    // - Chrome
    // - ChromeCanary
    // - Firefox
    // - Opera
    // - Safari (only Mac)
    // - PhantomJS
    // - IE (only Windows)
    browsers: [
      "PhantomJS"
    ],

    // Which plugins to enable
    plugins: [
      "karma-phantomjs-launcher",
      "karma-jasmine"
    ],

    // Continuous Integration mode
    // if true, it capture browsers, run tests and exit
    singleRun: false,

    colors: true,

    // level of logging
    // possible values: LOG_DISABLE || LOG_ERROR || LOG_WARN || LOG_INFO || LOG_DEBUG
    logLevel: config.LOG_INFO,

    // Uncomment the following lines if you are using grunt's server to run the tests
    // proxies: {
    //   '/': 'http://localhost:9000/'
    // },
    // URL root prevent conflicts with the site root
    // urlRoot: '_karma_'
  });
};
