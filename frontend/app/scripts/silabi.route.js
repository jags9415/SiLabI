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
          templateUrl: 'scripts/public/login/login.html',
          controller: 'LoginController',
          controllerAs: 'Auth'
        })
        .when('/About', {
          templateUrl: 'scripts/public/about/about.html',
          controller: 'AboutController',
          controllerAs: 'About'
        })
        .when('/Administrador', {
          templateUrl: 'scripts/administrator/home.html',
          controller: 'AdminController',
          controllerAs: 'Admin'
        })
        .when('/Operador', {
          templateUrl: 'scripts/operator/home.html',
          controller: 'OperatorsController',
          controllerAs: 'OperatorsHome'
        })
        .when('/Estudiante', {
          templateUrl: 'scripts/student/home.html',
          controller: 'StudentsController',
          controllerAs: 'Students'
        })
        .when('/Docente', {
          templateUrl: 'scripts/professor/home.html',
          controller: 'ProfessorsController',
          controllerAs: 'Professors'
        })
        .when('/Administrador/Operadores', {
          templateUrl: 'scripts/administrator/operators/operator.list.html',
          controller: 'OperatorListController',
          controllerAs: 'OperatorList',
          reloadOnSearch: false
        })
        .when('/Administrador/Operadores/Agregar', {
          templateUrl: 'scripts/administrator/operators/operator.create.html',
          controller: 'OperatorCreateController',
          controllerAs: 'Operator'
        })
        .when('/Administrador/Operadores/:id', {
          templateUrl: 'scripts/administrator/operators/operator.detail.html',
          controller: 'OperatorDetailController',
          controllerAs: 'Operator'
        })
        .when('/Operador/Docentes', {
          templateUrl: 'scripts/operator/professors/professors.list.html',
          controller: 'OperatorsController',
          controllerAs: 'Operator'
        })
        .when('/Operador/Docentes/Agregar', {
          templateUrl: 'scripts/operator/professors/professors.create.html',
          controller: 'OperatorsController',
          controllerAs: 'Operator'
        })
        .when('/Operador/Docentes/:username', {
          templateUrl: 'scripts/operator/professors/professors.detail.html',
          controller: 'OperatorsProfessorsController',
          controllerAs: 'OperatorProfessor'
        })
        .when('/Operador/Estudiantes', {
          templateUrl: 'scripts/operator/students/student.list.html',
          controller: 'StudentListController',
          controllerAs: 'StudentList'
        })
        .when('/Operador/Estudiantes/Agregar', {
          templateUrl: 'scripts/operator/students/student.create.html',
          controller: 'StudentsAddController',
          controllerAs: 'StudentsAdd'
        })
        .when('/Operador/Estudiantes/:username', {
          templateUrl: 'scripts/operator/students/student.detail.html',
          controller: 'StudentsDetailController',
          controllerAs: 'StudentDetails'
        })
        .otherwise('/');
    }

    routeChangeListener.$inject = ['$rootScope', '$location', '$localStorage', 'jwtHelper'];

    function routeChangeListener($rootScope, $location, $localStorage, jwtHelper) {
      $rootScope.$on("$routeChangeStart", function (event, next, current) {
        var token = $localStorage['access_token'];

        if (token && !jwtHelper.isTokenExpired(token)) {
          var payload = jwtHelper.decodeToken(token);

          if ((next.templateUrl === "scripts/public/login/login.html") ||
              (next.templateUrl.startsWith("scripts/administrator") && payload.type !== "Administrador") ||
              (next.templateUrl.startsWith("scripts/operator") && payload.type !== "Operador" && payload.type !== "Administrador") ||
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
