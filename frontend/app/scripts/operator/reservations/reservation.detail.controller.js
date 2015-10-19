(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ReservationDetailController', ReservationDetailController);

  ReservationDetailController.$inject = ['$routeParams', 'ReservationService', 'MessageService', 'SoftwareService', 'PeriodService', 'GroupService', 'LabService', 'DateService', '$location'];

  function ReservationDetailController($routeParams, ReservationService, MessageService, SoftwareService, PeriodService, GroupService, LabService, DateService, $location) {
    var vm = this;

    vm.reservation = {};
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

    vm.groups_request = {
      fields: 'id,number,course.name'
    };

    vm.software_request = {
      fields: 'id,code,name',
      limit: 10
    };

    vm.delete = deleteReservation;
    vm.update = updateReservation;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.openDatePicker = openDatePicker;
    vm.loadEndHours = loadEndHours;

    activate();


    function activate() {
      vm.reservation_id = $routeParams.id;
      getLaboratories();
      getHours();
      getReservation();
    }

    function openDatePicker($event){
      if ($event)
      {
        $event.preventDefault();
        $event.stopPropagation();
      }
      vm.datepicker_open = true;
    }

    function loadEndHours () {
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

    function getReservation() {
      ReservationService.GetOne(vm.reservation_id)
      .then(setReservation)
      .catch(handleError);
    }

    function getLaboratories () {
      LabService.GetAll()
      .then(setLaboratories)
      .catch(handleError);
    }

    function getGroups () {
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

      GroupService.GetAll(vm.groups_request)
      .then(setGroups)
      .catch(handleError);
    }

    function getHours() {
      vm.start_hours = DateService.GetReservationStartHours(vm.selected_date);
      vm.end_hours = DateService.GetReservationEndHours(vm.selected_date);
    }

    function searchSoftware (input) {
      vm.software_request.query = {};

      vm.software_request.query.code = {
        operation: 'like',
        value: '*' + input + '*'
      }

      return SoftwareService.GetAll(vm.software_request)
        .then(function(data) {
          return data.results;
        });
    }

    function setReservation (data) {
      vm.reservation = data;

      vm.selected_date = new Date(data.start_time);
      vm.username = vm.reservation.professor.username;
      vm.selected_software = data.software;

      getGroups();
      setStartHour();
      setEndHour();
    }

    function setLaboratories (data) {
      vm.laboratories = data.results;
    }

    function setSoftware (data) {
      vm.selected_software = data;
      vm.reservation.software = data;
    }

    function setGroups (data) {
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
        'software': !_.isEmpty(vm.selected_software) ? vm.selected_software.code : ''
      };

      ReservationService.Update(vm.reservation_id, res)
      .then(handleSuccess)
      .catch(handleError);
    }

    function deleteReservation() {
      MessageService.confirm('¿Desea realmente eliminar esta reservación?')
      .then(function() {
        ReservationService.Delete(vm.reservation_id)
        .then(redirectToReservations)
        .catch(handleError);
      });
    }

    function redirectToReservations() {
      $location.path('/Operador/Reservaciones');
    }


    function handleSuccess (data) {
      MessageService.success('Reservación actualizada.');
      setReservation(data);
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
