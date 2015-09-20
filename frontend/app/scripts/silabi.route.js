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
        .when('/About', {
          templateUrl: 'views/public/about.html'
        })
        .when('/Administrador', {
          templateUrl: 'views/administrator/home.html'
        })
        .when('/Operador', {
          templateUrl: 'views/operator/home.html'
        })
        .when('/Estudiante', {
          templateUrl: 'views/student/home.html'
        })
        .when('/Docente', {
          templateUrl: 'views/professor/home.html'
        })
        .when('/Login', {
          templateUrl: 'views/public/login.html',
          controller: 'LoginController',
          controllerAs: 'Auth'
        })
        .when('/Administrador/Administradores', {
          templateUrl: 'views/administrator/administrators/administrator.list.html',
          controller: 'AdminListController',
          controllerAs: 'AdminList',
        })
        .when('/Administrador/Administradores/Agregar', {
          templateUrl: 'views/administrator/administrators/administrator.create.html',
          controller: 'AdministratorCreateController',
          controllerAs: 'AdministratorCreate',
        })
        .when('/Administrador/Administradores/:id', {
          templateUrl: 'views/administrator/administrators/administrator.detail.html',
          controller: 'AdministratorDetailController',
          controllerAs: 'AdministratorDetail',
        })
        .when('/Administrador/Operadores', {
          templateUrl: 'views/administrator/operators/operator.list.html',
          controller: 'OperatorListController',
          controllerAs: 'OperatorList',
          reloadOnSearch: false
        })
        .when('/Administrador/Operadores/Agregar', {
          templateUrl: 'views/administrator/operators/operator.create.html',
          controller: 'OperatorCreateController',
          controllerAs: 'Operator'
        })
        .when('/Administrador/Operadores/:id', {
          templateUrl: 'views/administrator/operators/operator.detail.html',
          controller: 'OperatorDetailController',
          controllerAs: 'Operator'
        })
        .when('/Operador/Docentes', {
          templateUrl: 'views/operator/professors/professor.list.html',
          controller: 'ProfessorListController',
          controllerAs: 'ProfessorList',
          reloadOnSearch: false
        })
        .when('/Operador/Docentes/Agregar', {
          templateUrl: 'views/operator/professors/professor.create.html',
          controller: 'ProfessorsCreateController',
          controllerAs: 'ProfessorCreate'
        })
        .when('/Operador/Docentes/:username', {
          templateUrl: 'views/operator/professors/professor.detail.html',
          controller: 'ProfessorsDetailController',
          controllerAs: 'ProfessorDetail'
        })
        .when('/Operador/Estudiantes', {
          templateUrl: 'views/operator/students/student.list.html',
          controller: 'StudentListController',
          controllerAs: 'StudentList',
          reloadOnSearch: false
        })
        .when('/Operador/Estudiantes/Agregar', {
          templateUrl: 'views/operator/students/student.create.html',
          controller: 'StudentsAddController',
          controllerAs: 'StudentsAdd'
        })
        .when('/Operador/Estudiantes/:username', {
          templateUrl: 'views/operator/students/student.detail.html',
          controller: 'StudentsDetailController',
          controllerAs: 'StudentDetails'
        })
        .when('/Operador/Cursos', {
          templateUrl: 'views/operator/courses/course.list.html',
          controller: 'CourseListController',
          controllerAs: 'CourseList',
          reloadOnSearch: false
        })
        .when('/Operador/Cursos/Agregar', {
          templateUrl: 'views/operator/courses/course.create.html',
          controller: 'CourseAddController',
          controllerAs: 'CourseAdd'
        })
        .when('/Operador/Cursos/:id', {
          templateUrl: 'views/operator/courses/course.detail.html',
          controller: 'CourseDetailController',
          controllerAs: 'CourseDetail'
        })
        .when('/Operador/Grupos', {
          templateUrl: 'views/operator/groups/group.list.html',
          controller: 'GroupListController',
          controllerAs: 'GroupList',
          reloadOnSearch: false
        })
        .when('/Operador/Grupos/Agregar', {
          templateUrl: 'views/operator/groups/group.create.html',
          controller: 'GroupCreateController',
          controllerAs: 'GroupCreate'
        })
        .when('/Operador/Grupos/:id', {
          templateUrl: 'views/operator/groups/group.detail.html',
          controller: 'GroupDetailController',
          controllerAs: 'GroupDetail'
        })
        .when('/Operador/Laboratorios', {
          templateUrl: 'views/operator/labs/lab.list.html',
          controller: 'LabListController',
          controllerAs: 'LabList',
          reloadOnSearch: false
        })
        .when('/404', {
          templateUrl: 'views/public/404.html'
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
        if (!url) return;

        // User is authenticated.
        if (AuthenticationService.isAuthenticated()) {
          var data = AuthenticationService.getUserData();
          // User is trying to access the login view or a restricted view.
          if (url === "views/public/login.html" || !hasAccesssToView(url, data.type)) {
            if (current) {
              event.preventDefault();
            }
            else {
              $location.path('/' + data.type);
            }
          }
        }
        // User is not authenticated. Only have access to public views.
        else if (!url.startsWith("views/public")) {
          $location.path('/Login');
        }
      });
    }

    function hasAccesssToView(view, type) {
      if (view.startsWith("views/public")) {
        return true;
      }
      else if (view.startsWith("views/administrator")) {
        return type === "Administrador";
      }
      else if (view.startsWith("views/operator")) {
        return type === "Operador" || type === "Administrador";
      }
      else if (view.startsWith("views/professor")) {
        return type === "Docente";
      }
      else if (view.startsWith("views/student")) {
        return type === "Estudiante";
      }
      else {
        return false;
      }
    }
})();
