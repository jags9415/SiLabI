(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfileController', ProfileController);

    ProfileController.$inject = ['$scope', 'ProfileService', 'MessageService', 'GenderService', 'CryptoJS'];

    function ProfileController($scope, ProfileService, MessageService, GenderService, CryptoJS) {
        var vm = this;
        vm.profile = {};
        vm.genders = [];
        vm.update = update;

        activate();

        function activate() {
          GenderService.GetAll()
          .then(setGenders)
          .catch(handleError);

          ProfileService.Get()
          .then(setProfile)
          .catch(handleError);
        }

        function update() {
          if (!_.isEmpty(vm.profile)) {
            if (vm.password) {
              var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
              vm.profile.password = hash;
            }

            ProfileService.Update(vm.profile)
            .then(updateProfile)
            .catch(handleError);
          }
        }

        function setGenders(genders) {
          vm.genders = genders;
        }

        function setProfile(profile) {
          vm.profile = profile;
        }

        function updateProfile(profile) {
          setProfile(profile);
          $scope.$broadcast('show-errors-reset');
          MessageService.success("Perfil actualizado.");
        }

        function handleError(error) {
          MessageService.error(error.description);
        }
    }
})();
