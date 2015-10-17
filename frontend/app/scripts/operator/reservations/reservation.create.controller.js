(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ReservationCreateController', ReservationCreateController);

  ReservationCreateController.$inject = ['$scope', 'ReservationService', 'MessageService', 'DateService', 'ProfessorService', 'SoftwareService', 'PeriodService', 'LabService', 'GroupService', '$location'];

  function ReservationCreateController($scope, ReservationService, MessageService, DateService, ProfessorService, SoftwareService, PeriodService, LabService, GroupService, $location) {
    var vm = this;
    vm.professor = {};
    vm.start_hours = [];
    vm.end_hours = [];
    vm.end_hours_sliced = [];
    vm.groups = [];
    vm.selected_group = null;
    vm.laboratories = [];
    vm.software_list = [];
    vm.selected_software = null;

    vm.professor_request = {
      fields: "id,username,full_name"
    };

    vm.groups_request = {
      fields: "id,number,course.name"
    };

    vm.software_request = {
      fields: "id,code,name",
      limit: 10
    };

    vm.selected_date;
    vm.datepicker_open = false;
    vm.min_date = new Date();

    vm.openDatePicker = openDatePicker;
    vm.loadEndHours = loadEndHours;
    vm.searchProfessor = searchProfessor;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.create = createReservation;

    activate();

    function activate() {
      getLaboratories();
      getHours();
    }

    function searchProfessor() {
      ProfessorService.GetOne(vm.professor_username, vm.professor_request)
      .then(setProfessor)
      .catch(handleError);
      clearFields();
    }

    function getLaboratories () {
      LabService.GetAll()
      .then(setLaboratories)
      .catch(handleError);
    }

    function setProfessor (professor) {
      vm.professor = professor;
      getGroups();
    }

    function loadEndHours () {
      var index = -1;

      for (var i = 0; i < vm.end_hours.length; i++)
      {
        if(vm.selected_start_time.value == vm.end_hours[i].value)
        {
          index = i;
        }
      }
      if(index >= 0)
      {
        vm.end_hours_sliced = vm.end_hours.slice(index+1, vm.end_hours.length);
      }
      else
      {
        vm.end_hours_sliced = vm.end_hours;
      }
    }

    function getHours() {
      vm.start_hours = DateService.GetReservationStartHours();
      vm.end_hours = DateService.GetReservationEndHours();
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
        value: vm.professor.username
      };

      GroupService.GetAll(vm.groups_request)
      .then(setGroups)
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

    function setGroups (data) {
      vm.groups = data.results;
    }

    function setLaboratories (data) {
      vm.laboratories = data.results;
    }

    function setSoftware (data) {
      vm.selected_software = data;
    }

    function openDatePicker($event){
      if ($event)
      {
        $event.preventDefault();
        $event.stopPropagation();
      }
      vm.datepicker_open = true;
    }

    function createReservation () {
      var group_id = null;
      var software_code = null;
      if(vm.selected_group)
      {
        group_id = vm.selected_group.id;
      }
      if(vm.selected_software)
      {
        software_code = vm.selected_software.code;
      }
      var start_time = vm.selected_date.getFullYear()+"-"+ (vm.selected_date.getMonth() + 1)+ "-" + vm.selected_date.getUTCDate() + "T" + vm.selected_start_time.value;
      var end_time = vm.selected_date.getFullYear()+"-"+ (vm.selected_date.getMonth() + 1)+ "-" + vm.selected_date.getUTCDate() + "T" + vm.selected_end_time.value;
      var res = {
        "professor": vm.professor.username,
        "laboratory": vm.selected_laboratory.name,
        "software": software_code,
        "start_time": start_time,
        "end_time": end_time,
        "group": group_id
      };

      ReservationService.Create(res)
      .then(handleSuccess)
      .catch(handleError);
    }


    function clearFields() {
      $scope.$broadcast('show-errors-reset');

      delete vm.professor;
      delete vm.professor_username;
      delete vm.selected_date;
      delete vm.groups;
      delete vm.selected_start_time;
      delete vm.selected_end_time;
      delete vm.software_list;
      delete vm.selected_date;
      delete vm.selected_software;
      delete vm.selected_laboratory;
      delete vm.selected_group;
    }

    function handleSuccess (data) {
      MessageService.success("Reservación creada.");
      clearFields();
      delete vm.student_username;
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
