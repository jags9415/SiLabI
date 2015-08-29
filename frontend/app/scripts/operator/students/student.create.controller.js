(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsAddController', StudentsAddController);

    StudentsAddController.$inject = [''];

    function StudentsAddController() {
        var vm = this;

        vm.student = {};
        vm.genders = [
          { name: 'Masculino' },
          { name: 'Femenino' }
        ];

        vm.create = create;

        function create() {
          // body...
        }
    }
})();
