(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('StudentAppDetailController', AppointmentDetailController);

  AppointmentDetailController.$inject = ['$localStorage', '$location', '$routeParams', 'StudentAppService', 'AppointmentDateService', 'MessageService', 'SoftwareService', 'moment', 'lodash'];

  function AppointmentDetailController($localStorage, $location, $routeParams, StudentAppService, AppointmentDateService, MessageService, SoftwareService, moment, _) {
    var vm = this;

    vm.disabled = true;
    vm.selectedSoftware = {};
    vm.softwareList = [];
    vm.availableDates = [];
    vm.availableHours = [];
    vm.selectedState = {};
    vm.selectedLaboratory = {};
    vm.$storage = $localStorage;

    vm.request = {
      fields: 'id,date,state,group.number,group.course.name,software.code,software.name,laboratory.name'
    };

    vm.dateRequest = {
      fields: 'date,laboratory.name'
    };

    vm.softwareRequest = {
      fields: 'code,name',
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
      vm.username = vm.$storage['username'];
      vm.appointmentId = $routeParams.appointmentId;
      getAppointment();
    }

    function getAppointment () {
      StudentAppService.GetOne(vm.username, vm.appointmentId, vm.request)
      .then(setAppointment)
      .then(setAvailableDates)
      .catch(handleError);
    }

    function searchSoftware (input) {
      vm.softwareRequest.query = {};

      vm.softwareRequest.query.code = {
        operation: 'like',
        value: '*' + input + '*'
      };

      return SoftwareService.GetAll(vm.softwareRequest)
        .then(function(data) {
          return data.results;
        });
    }

    function getAvailableDates() {
        return AppointmentDateService.GetAvailableForUpdate(vm.dateRequest, vm.username, vm.appointment.id);
    }

    function setAppointment (data) {
      vm.appointment = data;
      vm.disabled = data.state != 'Por iniciar';
      return getAvailableDates();
    }

    function setSoftware (data) {
      vm.appointment.software = data;
    }

    function setAvailableDates (dates) {
      vm.availableDates = AppointmentDateService.ParseAvailableDates(dates);
      vm.selectedDate = null;

      for (var i = 0; i < vm.availableDates.length; i++) {
        if (moment(vm.availableDates[i].day).isSame(vm.appointment.date, 'day')) {
          vm.selectedDate = vm.availableDates[i];
          setAvailableHours();
          break;
        }
      }
    }

    function setAvailableHours() {
      if (vm.selectedDate) {
        vm.availableHours = vm.selectedDate.hoursByLab;
        vm.selectedHour = null;

        for (var i = 0; i < vm.availableHours.length; i++) {
          if (moment(vm.availableHours[i].fullDate).isSame(vm.appointment.date, 'hour')) {
            vm.selectedHour = vm.availableHours[i];
            break;
          }
        }

        if (_.isNull(vm.selectedHour)) {
          vm.selectedHour = vm.availableHours[0];
        }
      }
    }

    function changeLaboratory () {
      if (vm.selectedHour) {
        vm.selectedLaboratory = vm.selectedHour.laboratory;
      }
    }

    function updateAppointment () {
      if (!_.isEmpty(vm.appointment)) {

        var app = {
          'date': vm.selectedHour.fullDate,
          'software': vm.appointment.software.code
        };

        StudentAppService.Update(vm.username, vm.appointmentId, app)
        .then(handleSuccess)
        .catch(handleError);
      }
    }

    function deleteAppointment() {
      if (!_.isEmpty(vm.appointment)) {
        MessageService.confirm('¿Desea realmente eliminar esta cita?')
        .then(function() {
          StudentAppService.Delete(vm.username, vm.appointmentId)
          .then(redirectPage)
          .catch(handleError);
        });
      }
    }

    function redirectPage () {
      $location.path('/Estudiante/Citas');
    }

    function handleSuccess (data) {
      MessageService.success('Cita actualizada.');
      setAppointment(data);
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
