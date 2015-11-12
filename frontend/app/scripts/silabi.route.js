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
        .when('/Administrador', {
          redirectTo: '/Operador/Asistencia'
        })
        .when('/Operador', {
          redirectTo: '/Operador/Asistencia'
        })
        .when('/Estudiante', {
          redirectTo: '/Estudiante/Citas'
        })
        .when('/Docente', {
          redirectTo: '/Docente/Reservaciones'
        })
        .when('/Login', {
          templateUrl: 'views/public/login.html',
          controller: 'LoginController',
          controllerAs: 'Auth'
        })
        .when('/Perfil', {
          templateUrl: 'views/shared/profile/profile.html',
          controller: 'ProfileController',
          controllerAs: 'Profile'
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
          controllerAs: 'SoftwareCreate'
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
          controllerAs: 'AppointmentCreate'
        })
        .when('/Operador/Citas/:id', {
          templateUrl: 'views/operator/appointments/appointment.detail.html',
          controller: 'AppointmentDetailController',
          controllerAs: 'AppointmentDetail'
        })
        .when('/Operador/Asistencia', {
          templateUrl: 'views/operator/appointments/appointment.current.html',
          controller: 'AppointmentCurrentController',
          controllerAs: 'AppointmentCurrent'
        })
        .when('/Estudiante/Citas', {
          templateUrl: 'views/student/appointments/appointment.list.html',
          controller: 'StudentAppListController',
          controllerAs: 'AppointmentList',
          reloadOnSearch: false
        })
        .when('/Estudiante/Citas/Agregar', {
          templateUrl: 'views/student/appointments/appointment.create.html',
          controller: 'StudentAppCreateController',
          controllerAs: 'AppointmentCreate'
        })
        .when('/Estudiante/Citas/:appointmentId', {
          templateUrl: 'views/student/appointments/appointment.detail.html',
          controller: 'StudentAppDetailController',
          controllerAs: 'AppointmentDetail'
        })
        .when('/Docente/Reservaciones', {
          templateUrl: 'views/professor/reservations/reservation.list.html',
          controller: 'ProfessorReservationListController',
          controllerAs: 'ProfessorReservationList',
          reloadOnSearch: false
        })
        .when('/Docente/Reservaciones/Agregar', {
          templateUrl: 'views/professor/reservations/reservation.create.html',
          controller: 'ProfessorReservationCreateController',
          controllerAs: 'ProfessorReservationCreate'
        })
        .when('/Docente/Reservaciones/:id', {
          templateUrl: 'views/professor/reservations/reservation.detail.html',
          controller: 'ProfessorReservationDetailController',
          controllerAs: 'ProfessorReservationDetail',
        })
        .when('/Operador/Reservaciones', {
          templateUrl: 'views/operator/reservations/reservation.list.html',
          controller: 'ReservationListController',
          controllerAs: 'ReservationList',
          reloadOnSearch: false
        })
        .when('/Operador/Reservaciones/Agregar', {
          templateUrl: 'views/operator/reservations/reservation.create.html',
          controller: 'ReservationCreateController',
          controllerAs: 'ReservationCreate'
        })
        .when('/Operador/Reservaciones/:id', {
          templateUrl: 'views/operator/reservations/reservation.detail.html',
          controller: 'ReservationDetailController',
          controllerAs: 'ReservationDetail'
        })
        .when('/404', {
          templateUrl: 'views/public/404.html'
        })
        .otherwise('/404');
    }

    handleHomeRedirect.$inject = ['$location', '$localStorage', 'AuthenticationService'];

    function handleHomeRedirect($location, $localStorage, AuthenticationService) {
      if (AuthenticationService.isAuthenticated()) {
        var data = AuthenticationService.getUserData();
        $location.path('/' + data.type);
      }
      else {
        $localStorage.$reset();
        $location.path('/Login');
      }
    }

    routeChangeListener.$inject = ['$rootScope', '$location', '$localStorage', 'AuthenticationService', 'lodash'];

    function routeChangeListener($rootScope, $location, $localStorage, AuthenticationService, _) {
      $rootScope.$on('$routeChangeStart', function (event, next, current) {
        var path = next.originalPath;
        var url = next.templateUrl;

        // User is authenticated.
        if (AuthenticationService.isAuthenticated()) {
          var data = AuthenticationService.getUserData();
          // User is trying to access the login view or a restricted view.
          if ((_.startsWith(url, 'views/public') && path !== '/404') || !hasAccesssToView(_, next, data.type)) {
            if (current) {
              event.preventDefault();
            }
            else {
              $location.path('/' + data.type);
            }
          }
        }
        // User is not authenticated. Only have access to public views.
        else if (url && !_.startsWith(url, 'views/public')) {
          $localStorage.$reset();
          $location.path('/Login');
        }
      });
    }

    function hasAccesssToView(_, next, type) {
      var path = next.originalPath;
      var url = next.templateUrl;

      if (path === '/') {
        return true;
      }
      if (_.startsWith(url, 'views/public')) {
        return true;
      }
      else if (_.startsWith(url, 'views/shared')) {
        return type === 'Administrador' || type === 'Operador' || type === 'Docente' || type === 'Estudiante';
      }
      else if (_.startsWith(path, '/Administrador')) {
        return type === 'Administrador';
      }
      else if (_.startsWith(path, '/Operador')) {
        return type === 'Operador' || type === 'Administrador';
      }
      else if (_.startsWith(path, '/Docente')) {
        return type === 'Docente';
      }
      else if (_.startsWith(path, '/Estudiante')) {
        return type === 'Estudiante';
      }
      else {
        return false;
      }
    }
})();
