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
          'silabi.navbar',
          'silabi.sidebar',
        ])
        .constant('API_URL','http://localhost/api/v1/');
})();
