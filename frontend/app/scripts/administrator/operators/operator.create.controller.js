(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorCreateController', OperatorCreateController);

    OperatorCreateController.$inject = ['OperatorService', 'StudentService', 'PeriodService', 'MessageService'];

    function OperatorCreateController(OperatorService, StudentService, PeriodService, MessageService) {
        var vm = this;
        vm.periods = [];
        vm.currentYear = new Date().getFullYear();
        vm.year = vm.currentYear;
        vm.search = search;
        vm.create = create;

        activate();

        function activate() {
          PeriodService.GetAll()
          .then(setPeriods)
          .catch(handleError);
        }

        function search() {
          vm.user = {};
          if (vm.username) {
            StudentService.GetOne(vm.username)
            .then(setUser)
            .catch(handleError);
          }
        }

        function create() {
          if (vm.user && vm.period && vm.year) {
            vm.period.year = vm.year;

            OperatorService.Create(vm.user.id, vm.period)
            .then(handleCreateSuccess)
            .catch(handleError);
          }
        }

        function setPeriods(periods) {
          vm.periods = periods;
          vm.period = periods[0];
        }

        function setUser(user) {
          vm.user = user;
        }

        function handleCreateSuccess() {
          MessageService.success("Operador creado con éxito.");
          vm.user = {};
          delete vm.username;
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
