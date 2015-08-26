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
          controllerAs: 'Admin'
        })
        .when('/Inicio/Operador', {
          templateUrl: 'scripts/operators/operatorsHome.html',
          controller: 'OperatorsController',
          controllerAs: 'OperatorsHome'
        })
        .when('/Operador/Docentes', {
          templateUrl: 'scripts/operators/professors/professorsList.html',
          controller: 'OperatorsController',
          controllerAs: 'Operator'
        })
        .when('/Operador/Docentes/Agregar', {
          templateUrl: 'scripts/operators/professors/professorsCreate.html',
          controller: 'OperatorsController',
          controllerAs: 'Operator'
        })
        .when('/Operador/Docentes/:userName', {
          templateUrl: 'scripts/operators/professors/professorsDetail.html',
          controller: 'OperatorsProfessorsController',
          controllerAs: 'OperatorProfessor'
        })
        .when('/Operador/Estudiantes', {
          templateUrl: 'scripts/operators/students/list.html',
          controller: 'StudentListController',
          controllerAs: 'StudentList'
        })
        .when('/Operador/Estudiantes/Agregar', {
          templateUrl: 'scripts/operators/students/add.html',
          controller: 'StudentsAddController',
          controllerAs: 'StudentsAdd'
        })
        .when('/Operador/Estudiantes/:username', {
          templateUrl: 'scripts/operators/students/detail.html',
          controller: 'StudentsDetailController'
        })
        .when('/Inicio/Estudiante', {
          templateUrl: 'scripts/students/studentsHome.html',
          controller: 'StudentsController',
          controllerAs: 'Students'
        })
        .when('/Inicio/Docente', {
          templateUrl: 'scripts/professors/professorsHome.html',
          controller: 'ProfessorsController',
          controllerAs: 'Professors'
        })
        .otherwise('/');
    }
})();
