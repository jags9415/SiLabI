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
          resolve: { load: handleHomeRedirect }
        })
        .when('/Login', {
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
          controller: 'ProfessorListController',
          controllerAs: 'ProfessorList',
          reloadOnSearch: false
        })
        .when('/Operador/Docentes/Agregar', {
          templateUrl: 'scripts/operator/professors/professors.create.html',
          controller: 'OperatorsProfessorsCreateController',
          controllerAs: 'OperatorCreate'
        })
        .when('/Operador/Docentes/:username', {
          templateUrl: 'scripts/operator/professors/professors.detail.html',
          controller: 'OperatorsProfessorsController',
          controllerAs: 'OperatorProfessor'
        })
        .when('/Operador/Estudiantes', {
          templateUrl: 'scripts/operator/students/student.list.html',
          controller: 'StudentListController',
          controllerAs: 'StudentList',
          reloadOnSearch: false
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
        .when('/404', {
          templateUrl: '404.html'
        })
        .otherwise('/404');
    }

    handleHomeRedirect.$inject = ['$location', 'AuthenticationService'];

    function handleHomeRedirect($location, AuthenticationService) {
        if (AuthenticationService.isAuthenticated()) {
          var data = AuthenticationService.getUserData();
          $location.path('/' + data.type);
        }
        else {
          $location.path('/Login');
        }
      }

    routeChangeListener.$inject = ['$rootScope', '$location', 'AuthenticationService'];

    function routeChangeListener($rootScope, $location, AuthenticationService) {
      $rootScope.$on("$routeChangeStart", function (event, next, current) {
        var url = next.templateUrl;

        if (!url) {
          return;
        }

        // User is authenticated.
        if (AuthenticationService.isAuthenticated()) {
          var data = AuthenticationService.getUserData();
          // User is trying to access the login view or a restricted view.
          if (url === "scripts/public/login/login.html" || !hasAccesssToView(url, data.type)) {
            if (current) {
              event.preventDefault();
            }
            else {
              $location.path('/' + data.type);
            }
          }
        }
        // User is not authenticated. Only have access to public views.
        else if (!url.startsWith("scripts/public")) {
          $location.path('/Login');
        }
      });
    }

    function hasAccesssToView(view, type) {
      if (view.startsWith("scripts/administrator")) {
        return type === "Administrador";
      }
      else if (view.startsWith("scripts/operator")) {
        return type === "Operador" || type === "Administrador";
      }
      else if (view.startsWith("scripts/professor")) {
        return type === "Docente";
      }
      else if (view.startsWith("scripts/student")) {
        return type === "Estudiante";
      }
      else {
        return false;
      }
    }
})();
