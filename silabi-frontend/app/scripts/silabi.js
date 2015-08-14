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
        .config(function ($routeProvider) {
          $routeProvider
            .when('/', {
              templateUrl: 'scripts/login/login.html',
              controller: 'LoginController',
              controllerAs: 'Login'
            })
            .when('/about', {
              templateUrl: 'scripts/about/about.html',
              controller: 'AboutController',
              controllerAs: 'About'
            })
            .otherwise({
              templateUrl: '404.html'
            });
        });
})();
