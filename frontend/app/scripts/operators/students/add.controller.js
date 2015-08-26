(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsAddController', StudentsAddController);

    function StudentsAddController() {
        var vm = this;

        vm.student = {};
        vm.genders = [
          { name: 'Masculino' },
          { name: 'Femenenino' }
        ];

        vm.mostrar = mostrar();

        function mostrar() {
          console.log(vm.student);
        }
    }
})();
