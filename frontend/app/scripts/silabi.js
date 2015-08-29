(function() {
    'use strict';

    angular
        .module('silabi', [
          'ngAnimate',
          'ngCookies',
          'ngResource',
          'ngRoute',
          'ngSanitize',
          'ngTouch',
          'ngStorage',
          'angular-jwt',
          'ui.select',
          'ui.mask',
          'ui.bootstrap.showErrors',
          'silabi.sidebar',
        ]);
})();
