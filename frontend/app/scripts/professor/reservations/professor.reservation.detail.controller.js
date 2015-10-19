(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ProfessorReservationDetailController', ProfessorReservationDetailController);

  ProfessorReservationDetailController.$inject = ['$scope', '$routeParams', 'ProfessorReservationService', 'MessageService', 'SoftwareService', 'PeriodService', 'GroupService', 'LabService', 'DateService', '$location', '$localStorage'];

  function ProfessorReservationDetailController($scope, $routeParams, ProfessorReservationService, MessageService, SoftwareService, PeriodService, GroupService, LabService, DateService, $location, $localStorage) {
    var vm = this;

    vm.disabled = true;
    vm.software_list = [];
    vm.groups = [];
    vm.laboratories = [];
    vm.start_hours = [];
    vm.end_hours = [];
    vm.end_hours_sliced = [];
    vm.selected_software = null;
    vm.selected_group = null;
    vm.min_date = new Date();
    vm.datepicker_open = false;
    vm.$storage = $localStorage;

    vm.request = {
      fields: 'id,state,start_time,end_time,laboratory.name,group.course.name,software.code,software.name'
    }

    vm.laboratory_request = {
      fields: 'name'
    }

    vm.groups_request = {
      fields: 'id,course.name'
    };

    vm.software_request = {
      fields: 'id,code,name',
      limit: 10
    };

    vm.delete = deleteReservation;
    vm.update = updateReservation;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.formatSoftware = formatSoftware;
    vm.openDatePicker = openDatePicker;
    vm.loadEndHours = loadEndHours;

    activate();

    function activate() {
      vm.reservation_id = $routeParams.id;
      vm.username = vm.$storage['username'];

      getLaboratories();
      getGroups();
      getReservation();
    }

    function getReservation() {
      ProfessorReservationService.GetOne(vm.username, vm.reservation_id, vm.request)
      .then(setReservation)
      .catch(handleError);
    }

    function getLaboratories() {
      LabService.GetAll(vm.laboratory_request, true)
      .then(setLaboratories)
      .catch(handleError);
    }

    function getGroups() {
      var period = PeriodService.GetCurrentPeriod('Semestre');
      vm.groups_request.query = {};

      vm.groups_request.query['period.type'] = {
        operation: 'eq',
        value: 'Semestre'
      };

      vm.groups_request.query['period.value'] = {
        operation: 'eq',
        value: period.value
      };

      vm.groups_request.query['period.year'] = {
        operation: 'eq',
        value: period.year
      };

      vm.groups_request.query['professor.username'] = {
        operation: 'eq',
        value: vm.username
      };

      GroupService.GetAll(vm.groups_request, true)
      .then(setGroups)
      .catch(handleError);
    }

    function getHours() {
      vm.start_hours = DateService.GetHourRange(8, 17);
      vm.end_hours = DateService.GetHourRange(9, 18);
    }

    function loadEndHours() {
      var index = _.indexOf(vm.end_hours, vm.selected_start_time);

      if (index >= 0) {
        vm.end_hours_sliced = vm.end_hours.slice(index + 1, vm.end_hours.length);
      }
      else {
        vm.end_hours_sliced = vm.end_hours;
      }

      if (!_.contains(vm.end_hours_sliced, vm.selected_end_time)) {
        vm.selected_end_time = vm.end_hours_sliced[0];
      }
    }

    function searchSoftware(input) {
      vm.software_request.query = {};

      vm.software_request.query.code = {
        operation: 'like',
        value: '*' + input + '*'
      }

      return SoftwareService.GetAll(vm.software_request, true)
        .then(function(data) {
          return data.results;
        });
    }

    function setReservation(data) {
      vm.reservation = data;
      vm.disabled = data.state != 'Por iniciar';
      vm.selected_date = new Date(data.start_time);

      setStartHour();
      setEndHour();
      loadEndHours();
    }

    function setLaboratories(data) {
      vm.laboratories = data.results;
    }

    function setSoftware(data) {
      vm.reservation.software = data;
    }

    function formatSoftware(model) {
      if (model) {
        return model.code;
      }
    }

    function setGroups(data) {
      vm.groups = data.results;
    }

    function setStartHour() {
      var start_time = moment(vm.reservation.start_time);

      for (var i = 0; i < vm.start_hours.length; i++) {
        var current_hour = moment(vm.start_hours[i]);

        if (start_time.hour() === current_hour.hour()) {
          vm.selected_start_time = current_hour.format();
          return;
        }
      }
    }

    function setEndHour() {
      var end_time = moment(vm.reservation.end_time);

      for (var i = 0; i < vm.end_hours.length; i++) {
        var current_hour = moment(vm.end_hours[i]);

        if (end_time.hour() === current_hour.hour()) {
          vm.selected_end_time = current_hour.format();
          return;
        }
      }
    }

    function updateReservation () {
      var start_time = moment(vm.selected_date).hour(moment(vm.selected_start_time).hour());
      var end_time = moment(vm.selected_date).hour(moment(vm.selected_end_time).hour());

      var res = {
        'laboratory': vm.reservation.laboratory.name,
        'start_time': start_time.format(),
        'end_time': end_time.format(),
        'group': !_.isEmpty(vm.reservation.group) ? vm.reservation.group.id : 0,
        'software': !_.isEmpty(vm.reservation.software) ? vm.reservation.software.code : ''
      };

      ProfessorReservationService.Update(vm.username, vm.reservation_id, res)
      .then(handleSuccess)
      .catch(handleError);
    }

    function deleteReservation() {
      MessageService.confirm('¿Desea realmente eliminar esta reservación?')
      .then(function() {
        ProfessorReservationService.Delete(vm.username, vm.reservation_id)
        .then(redirectToHome)
        .catch(handleError);
      });
    }

    function redirectToHome () {
      $location.path('Docente/Reservaciones');
    }

    function handleSuccess (data) {
      MessageService.success('Reservación actualizada.');
      setReservation(data);
    }

    function handleError(data) {
      MessageService.error(data.description);
    }

    function openDatePicker($event){
      if ($event) {
        $event.preventDefault();
        $event.stopPropagation();
      }
      vm.datepicker_open = true;
    }
  }
})();
