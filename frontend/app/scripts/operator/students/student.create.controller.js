(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsAddController', StudentsAddController);

    StudentsAddController.$inject = ['$location', 'RequestService', 'MessageService'];

    function StudentsAddController($location, RequestService, MessageService) {
        var vm = this;

        vm.student = {};
        vm.genders = [
          { name: 'Masculino' },
          { name: 'Femenino' }
        ];

        vm.create = create;
        vm.cancel = cancel;

        function create() {
          RequestService.post('/students', {
            'student': vm.student,
            'access_token': '123'   // add a real access_token
          })
          .then(handleSuccess)
          .catch(handleError);
        }

        function handleSuccess(result) {
          vm.student = {};
        }

        function handleError(error) {
          MessageService.error(error.description);
        }

        function cancel() {
          $location.path('/');
        }
    }
})();
