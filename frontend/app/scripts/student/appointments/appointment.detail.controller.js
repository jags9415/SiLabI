(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('StudentAppDetailController', AppointmentDetailController);

  AppointmentDetailController.$inject = ['StudentAppService', 'AppointmentDateService', 'MessageService', 'SoftwareService', '$localStorage', '$location', '$routeParams'];

  function AppointmentDetailController(StudentAppService, AppointmentDateService, MessageService, SoftwareService, $localStorage, $location, $routeParams) {
    var vm = this;
    vm.selected_software = {};
    vm.software_list = [];
    vm.available_dates = [];
    vm.available_hours = [];
    vm.selected_state = {};
    vm.selected_laboratory = {};
    vm.$storage = $localStorage;

    vm.request = {
      fields: "id,date,state,group.number,group.course.name,software.code,software.name,laboratory.name"
    }

    vm.date_request = {
      fields: "date,laboratory.name"
    };

    vm.software_request = {
      fields: "code,name",
      limit: 10
    };

    vm.setAvailableHours = setAvailableHours;
    vm.changeLaboratory = changeLaboratory;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.update = updateAppointment;
    vm.delete = deleteAppointment;

    activate();

    function activate() {
      vm.student_id = vm.$storage['username'];
      vm.app_id = $routeParams.app_id;
      getAppointment();
    }

    function getAppointment () {
      StudentAppService.GetOne(vm.student_id, vm.app_id, vm.request)
      .then(setAppointment)
      .then(setAvailableDates)
      .catch(handleError);
    }

    function searchSoftware (input) {
      vm.software_request.query = {};

      vm.software_request.query.code = {
        operation: "like",
        value: '*' + input + '*'
      }

      return SoftwareService.GetAll(vm.software_request)
        .then(function(data) {
          return data.results;
        });
    }

    function getAvailableDates() {
        return AppointmentDateService.GetAvailable(vm.date_request, vm.student_id);
    }

    function setAppointment (data) {
      vm.appointment = data;
      return getAvailableDates();
    }

    function setSoftware (data) {
      vm.appointment.software = data;
    }

    function setAvailableDates (dates) {
      vm.available_dates = AppointmentDateService.ParseAvailableDates(dates);
      vm.selected_date = null;

      for (var i = 0; i < vm.available_dates.length; i++) {
        if (moment(vm.available_dates[i].day).isSame(vm.appointment.date, 'day')) {
          vm.selected_date = vm.available_dates[i];
          setAvailableHours();
          break;
        }
      }

      if (!vm.selected_date) {
        var current = {
          day: moment(vm.appointment.date).format("YYYY-MM-DD"),
          hoursByLab: [{
            full_date: vm.appointment.date,
            hour: moment(vm.appointment.date).format("HH:mm"),
            laboratory: vm.appointment.laboratory
          }]
        };

        vm.available_dates.unshift(current);
        vm.selected_date = vm.available_dates[0];
        setAvailableHours();
      }
    }

    function setAvailableHours() {
      if (vm.selected_date) {
        vm.available_hours = vm.selected_date.hoursByLab;
        vm.selected_hour = null;

        for (var i = 0; i < vm.available_hours.length; i++) {
          if (moment(vm.available_hours[i].full_date).isSame(vm.appointment.date, 'hour')) {
            vm.selected_hour = vm.available_hours[i];
            break;
          }
        }

        if (!vm.selected_hour) {
          vm.selected_hour = vm.available_hours[0];
        }
      }
    }

    function changeLaboratory () {
      if (vm.selected_hour) {
        vm.selected_laboratory = vm.selected_hour.laboratory;
      }
    }

    function updateAppointment () {
      if (!_.isEmpty(vm.appointment)) {
        if (vm.selected_date && vm.selected_hour) {
          vm.date = vm.selected_date.day + "T" + vm.selected_hour.hour + ":00.000";
        }

        var app = {
          "date": vm.date,
          "software": vm.appointment.software.code
        }

        StudentAppService.Update(vm.student_id, vm.app_id, app)
        .then(handleSuccess)
        .catch(handleError);
      }
    }

    function deleteAppointment() {
      if (!_.isEmpty(vm.appointment)) {
        MessageService.confirm("¿Desea realmente eliminar esta cita?")
        .then(function() {
          StudentAppService.Delete(vm.student_id, vm.app_id)
          .then(redirectPage)
          .catch(handleError);
        });
      }
    }

    function redirectPage () {
      $location.path("/Estudiante/Citas");
    }

    function handleSuccess (data) {
      MessageService.success("Cita actualizada.");
      setAppointment(data);
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
