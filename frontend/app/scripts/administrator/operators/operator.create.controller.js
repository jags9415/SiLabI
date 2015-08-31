(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorCreateController', OperatorCreateController);

    OperatorCreateController.$inject = ['OperatorService', 'StudentService', 'MessageService'];

    function OperatorCreateController(OperatorService, StudentService, MessageService) {
        var vm = this;
        vm.periods = [];
        vm.currentYear = new Date().getFullYear();
        vm.year = vm.currentYear;
        vm.search = search;
        vm.create = create;

        activate();

        // FIX. Periods need to be fetched from web service.
        function activate() {
          vm.periods = [
            {
              value: 1,
              type: 'Semestre'
            },
            {
              value: 2,
              type: 'Semestre'
            },
            {
              value: 1,
              type: 'Cuatrimestre'
            },
            {
              value: 2,
              type: 'Cuatrimestre'
            },
            {
              value: 3,
              type: 'Cuatrimestre'
            },
            {
              value: 1,
              type: 'Trimestre'
            },
            {
              value: 2,
              type: 'Trimestre'
            },
            {
              value: 3,
              type: 'Trimestre'
            },
            {
              value: 4,
              type: 'Trimestre'
            },
            {
              value: 1,
              type: 'Bimestre'
            },
            {
              value: 2,
              type: 'Bimestre'
            },
            {
              value: 3,
              type: 'Bimestre'
            },
            {
              value: 4,
              type: 'Bimestre'
            },
            {
              value: 5,
              type: 'Bimestre'
            },
            {
              value: 6,
              type: 'Bimestre'
            }
          ];
          if (vm.periods.length > 0) {
            vm.period = vm.periods[0];
          }
        }

        function search() {
          if (vm.username) {
            StudentService.GetOne(vm.username)
            .then(handleSearchSuccess)
            .catch(handleError);
          }
          else {
            vm.user = null;
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

        function handleSearchSuccess(student) {
          vm.user = student;
        }

        function handleCreateSuccess() {
          MessageService.success("Operador creado con éxito.");
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
