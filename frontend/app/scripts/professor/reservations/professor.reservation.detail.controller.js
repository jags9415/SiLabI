(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ProfessorReservationDetailController', ProfessorReservationDetailController);

  ProfessorReservationDetailController.$inject = ['$scope', '$routeParams', 'ProfessorReservationService', 'MessageService', 'SoftwareService', 'PeriodService', 'GroupService', 'LabService', 'DateService', '$location', '$localStorage'];

  function ProfessorReservationDetailController($scope, $routeParams, ProfessorReservationService, MessageService, SoftwareService, PeriodService, GroupService, LabService, DateService, $location, $localStorage) {
    var vm = this;
    vm.software_list = [];
    vm.groups = [];
    vm.laboratories = [];
    vm.start_hours = [];
    vm.end_hours = [];
    vm.selected_software = null;
    vm.selected_group = null;
    vm.min_date = new Date();
    vm.datepicker_open = false;
    vm.$storage = $localStorage;
 
    vm.groups_request = {
      fields: "id,number,course.name"
    };

    vm.software_request = {
      fields: "id,code,name",
      limit: 10
    };
    
    vm.fieldsReady = fieldsReady;
    vm.delete = deleteReservation;
    vm.update = updateReservation;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.openDatePicker = openDatePicker;

    activate();

  
    function activate() {
      vm.reservation_id = $routeParams.id;
      if(vm.$storage['username'])
      {
        vm.username = vm.$storage['username'];
      }
      getLaboratories();
      getHours();
      getGroups ();
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

    function fieldsReady () {
      return !_.isEmpty(vm.reservation) &&
             !_.isEmpty(vm.selected_date) &&
             !_.isEmpty(vm.selected_start_time) &&
             !_.isEmpty(vm.selected_end_time);
    }

    function getReservation() {
      ProfessorReservationService.GetOne(vm.username, vm.reservation_id)
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

      vm.groups_request.query["period.type"] = {
        operation: "eq",
        value: "Semestre"
      };

      vm.groups_request.query["period.value"] = {
        operation: "eq",
        value: period.value
      };

      vm.groups_request.query["period.year"] = {
        operation: "eq",
        value: period.year
      };

      vm.groups_request.query["professor.username"] = {
        operation: "eq",
        value: vm.username
      };

      GroupService.GetAll(vm.groups_request)
      .then(setGroups)
      .catch(handleError);
    }

    function getHours() {
      vm.start_hours = DateService.GetReservationStartHours();
      vm.end_hours = DateService.GetReservationEndHours();

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

    function setReservation (data) {
      vm.reservation = data; 
      vm.selected_date = data.start_time;
      setStartHour();
      setEndHour(); 
    }

    function setLaboratories (data) {
      vm.laboratories = data.results;
    }

    function setSoftware (data) {
      vm.selected_software = data;
    }

    function setGroups (data) {
      vm.groups = data.results;
    }

    function setStartHour() {
      var start_time = vm.reservation.start_time;
      var start_hour = start_time.substring(start_time.indexOf("T") + 1, start_time.length);
      for (var i = 0; i < vm.start_hours.length; i++) {
        var current_hour = vm.start_hours[i];
        if(start_hour === current_hour.value)
        {
          vm.selected_start_time = current_hour;
          return;
        }
    }
  }

     function setEndHour() {
      var end_time = vm.reservation.end_time;
      var end_hour = end_time.substring(end_time.indexOf("T") + 1, end_time.length);
      for (var i = 0; i < vm.end_hours.length; i++) {
        var current_hour = vm.end_hours[i];
        if(end_hour === current_hour.value)
        {
          vm.selected_end_time = current_hour;
          return;
        }
    }
    }

    function updateReservation () {
      console.log(vm.reservation.software.code);
      vm.selected_date = new Date(vm.selected_date);
      var start_time = vm.selected_date.getFullYear()+"-"+ (vm.selected_date.getMonth() + 1)+ "-" + vm.selected_date.getUTCDate() + "T" + vm.selected_start_time.value;
      var end_time = vm.selected_date.getFullYear()+"-"+ (vm.selected_date.getMonth() + 1)+ "-" + vm.selected_date.getUTCDate() + "T" + vm.selected_end_time.value;
      vm.reservation.start_time = start_time;
      vm.reservation.end_time = end_time;
      var res = 
      {
        "laboratory": vm.reservation.laboratory.name,
        "start_time": start_time,
        "end_time": end_time,
        "group": vm.reservation.group.id,
        "sofware":vm.reservation.software.code
      };

      ProfessorReservationService.Update(vm.username, vm.reservation_id, res)
      .then(handleSuccess)
      .catch(handleError);
    }

    function deleteReservation() {
      MessageService.confirm("¿Desea realmente eliminar esta reservación?")
      .then(function() {
        ProfessorReservationService.Delete(vm.username, vm.reservation_id)
        .then(loadPage)
        .catch(handleError);
      });
    }

    function clearFields() {
      $scope.$broadcast('show-errors-reset');
      delete vm.groups;
      delete vm.aselected_start_time;
      delete vm.aselected_end_time;
      delete vm.software_list;
      delete vm.selected_date;
      delete vm.selected_software;
      delete vm.selected_laboratory;
      delete vm.selected_group;
    }

    function handleSuccess (data) {
      MessageService.success("Reservación creada.");
      clearFields();
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
