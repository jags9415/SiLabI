(function() {
    'use strict';

    angular
        .module('silabi')
        .config(routeConfig)
        .run(routeChangeListener);

    routeConfig.$inject = ['$routeProvider'];

    function routeConfig($routeProvider) {
      $routeProvider
        .when('/', {
          templateUrl: 'scripts/login/login.html',
          controller: 'LoginController',
          controllerAs: 'Auth'
        })
        .when('/About', {
          templateUrl: 'scripts/about/about.html',
          controller: 'AboutController',
          controllerAs: 'About'
        })
        .when('/Administrador', {
          templateUrl: 'scripts/administrators/adminsHome.html',
          controller: 'AdminController',
          controllerAs: 'Admin'
        })
        .when('/Operador', {
          templateUrl: 'scripts/operators/operatorsHome.html',
          controller: 'OperatorsController',
          controllerAs: 'OperatorsHome'
        })
        .when('/Estudiante', {
          templateUrl: 'scripts/students/studentsHome.html',
          controller: 'StudentsController',
          controllerAs: 'Students'
        })
        .when('/Docente', {
          templateUrl: 'scripts/professors/professorsHome.html',
          controller: 'ProfessorsController',
          controllerAs: 'Professors'
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
        .otherwise('/');
    }

    routeChangeListener.$inject = ['$rootScope', '$location', '$sessionStorage', 'jwtHelper'];

    function routeChangeListener($rootScope, $location, $sessionStorage, jwtHelper) {
      $rootScope.$on("$routeChangeStart", function (event, next, current) {
        var token = $sessionStorage['access_token'];

        if (token && !jwtHelper.isTokenExpired(token)) {
          var payload = jwtHelper.decodeToken(token);

          if ((next.templateUrl === "scripts/public/login/login.html") ||
              (next.templateUrl.startsWith("scripts/administrator") && payload.type !== "Administrador") ||
              (next.templateUrl.startsWith("scripts/operator") && (payload.type !== "Operador" || payload.type !== "Administrador")) ||
              (next.templateUrl.startsWith("scripts/professor") && payload.type !== "Docente") ||
              (next.templateUrl.startsWith("scripts/student") && payload.type !== "Estudiante")) {
                event.preventDefault();
          }
        }
        else if (!next.templateUrl.startsWith("scripts/public")) {
          $location.path('/');
        }
      });
    }

})();
