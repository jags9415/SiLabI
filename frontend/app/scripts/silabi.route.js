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
          redirectTo: '/Operador/Asistencia'
        })
        .when('/Operador', {
          redirectTo: '/Operador/Asistencia'
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
        .when('/Operador/Laboratorios/Agregar', {
          templateUrl: 'views/operator/labs/lab.create.html',
          controller: 'LabAddController',
          controllerAs: 'LabAdd'
        })
        .when('/Operador/Laboratorios/:id', {
          templateUrl: 'views/operator/labs/lab.detail.html',
          controller: 'LabDetailController',
          controllerAs: 'LabDetail'
        })
        .when('/Operador/Software', {
          templateUrl: 'views/operator/software/software.list.html',
          controller: 'SoftwareListController',
          controllerAs: 'SoftwareList',
          reloadOnSearch: false
        })
        .when('/Operador/Software/Agregar', {
          templateUrl: 'views/operator/software/software.create.html',
          controller: 'SoftwareCreateController',
          controllerAs: 'SoftwareCreate',
          reloadOnSearch: false
        })
        .when('/Operador/Software/:code', {
          templateUrl: 'views/operator/software/software.detail.html',
          controller: 'SoftwareDetailController',
          controllerAs: 'SoftwareDetail'
        })
        .when('/Operador/Citas', {
          templateUrl: 'views/operator/appointments/appointment.list.html',
          controller: 'AppointmentListController',
          controllerAs: 'AppointmentList',
          reloadOnSearch: false
        })
        .when('/Operador/Citas/Agregar', {
          templateUrl: 'views/operator/appointments/appointment.create.html',
          controller: 'AppointmentCreateController',
          controllerAs: 'AppointmentCreate',
          reloadOnSearch: false
        })
        .when('/Operador/Citas/:id', {
          templateUrl: 'views/operator/appointments/appointment.detail.html',
          controller: 'AppointmentDetailController',
          controllerAs: 'AppointmentDetail',
          reloadOnSearch: false
        })
        .when('/Operador/Asistencia', {
          templateUrl: 'views/operator/appointments/appointment.current.html',
          controller: 'AppointmentCurrentController',
          controllerAs: 'AppointmentCurrent'
        })
        .when('/Estudiante/:student_id/Citas', {
          templateUrl: 'views/student/appointments/appointment.list.html',
          controller: 'StudentAppListController',
          controllerAs: 'AppointmentList',
          reloadOnSearch: false
        })
        .when('/404', {
          templateUrl: 'views/public/404.html'
        })
        .otherwise('/404');
    }

    function redirectToLogin($location, $localStorage) {
      delete $localStorage['access_token'];
      delete $localStorage['username'];
      delete $localStorage['user_id'];
      delete $localStorage['user_name'];
      delete $localStorage['user_type'];
      $location.path('/Login');
    }

    handleHomeRedirect.$inject = ['$location', '$localStorage', 'AuthenticationService'];

    function handleHomeRedirect($location, $localStorage, AuthenticationService) {
        if (AuthenticationService.isAuthenticated()) {
          var data = AuthenticationService.getUserData();
          $location.path('/' + data.type);
        }
        else {
          redirectToLogin($location, $localStorage);
        }
      }

    routeChangeListener.$inject = ['$rootScope', '$location', '$localStorage', 'AuthenticationService'];

    function routeChangeListener($rootScope, $location, $localStorage, AuthenticationService) {
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
          redirectToLogin($location, $localStorage);
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
