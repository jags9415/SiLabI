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
        // FIXME: 'Acerca' instead of 'about'
        .when('/about', {
          templateUrl: 'scripts/about/about.html',
          controller: 'AboutController',
          controllerAs: 'About'
        })
        .when('/Inicio/Administrador', {
          templateUrl: 'scripts/administrators/adminsHome.html',
          controller: 'AdminController',
          controllerAs: 'AdminHome'
        })
        .when('/Inicio/Operador', {
          templateUrl: 'scripts/operators/operatorsHome.html',
          controller: 'OperatorsController',
          controllerAs: 'OperatorsHome'
        })
        .when('/Inicio/Estudiante', {
          templateUrl: 'scripts/students/studentsHome.html',
          controller: 'StudentsController',
          controllerAs: 'StudentsHome'
        })
        .when('/Inicio/Docente', {
          templateUrl: 'scripts/professors/professorsHome.html',
          controller: 'ProfessorsController',
          controllerAs: 'ProfessorsHome'
        })
        .otherwise('/');
    }
})();
