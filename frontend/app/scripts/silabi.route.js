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
        .when('/administrators/home', {
          templateUrl: 'scripts/administrators/adminsHome.html',
          controller: 'AdminController',
          controllerAs: 'AdminHome'
        })
        .when('/operators/home', {
          templateUrl: 'scripts/operators/operatorsHome.html',
          controller: 'OperatorsController',
          controllerAs: 'OperatorsHome'
        })
        .when('/students/home', {
          templateUrl: 'scripts/students/studentsHome.html',
          controller: 'StudentsController',
          controllerAs: 'StudentsHome'
        })
        .when('/professors/home', {
          templateUrl: 'scripts/professors/professorsHome.html',
          controller: 'ProfessorsController',
          controllerAs: 'ProfessorsHome'
        })
        .otherwise({
          templateUrl: '404.html'
        });
    }
})();
