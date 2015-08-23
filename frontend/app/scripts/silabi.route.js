(function() {
    'use strict';

    angular
        .module('silabi')
        .config(routeConfig);

    routeConfig.$inject = ['$routeProvider'];

    function routeConfig($routeProvider) {
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
        .when('/administrators', {
          templateUrl: 'scripts/administrators/adminsHome.html',
          controller: 'AdminController',
          controllerAs: 'AdminHome'
        })
        .otherwise({
          templateUrl: '404.html'
        });
    }
})();
