(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('CourseAddController', CourseAddController);

    CourseAddController.$inject = ['$scope', 'CourseService', 'MessageService'];

    function CourseAddController($scope, CourseService, MessageService) {
        var vm = this;
        vm.course = {};
        vm.create = create;

        function create() {
          if (!_.isEmpty(vm.course)) {
            CourseService.Create(vm.course)
            .then(handleCreateSuccess)
            .catch(handleError);
          }
        }

        function handleCreateSuccess(result) {
          MessageService.success("Curso creado.");

          // Reset form data.
          vm.course = {};

          // Reset form validations.
          $scope.$broadcast('show-errors-reset');
        }

        function handleError(error) {
          MessageService.error(error.description);
        }
    }
})();
