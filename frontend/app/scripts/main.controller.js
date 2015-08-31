(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('MainController', MainController);

    MainController.$inject = ['$rootScope', '$localStorage'];

    function MainController($rootScope, $localStorage) {
        var vm = this;
        vm.user = {};
        vm.$storage = $localStorage;
        vm.access_token = vm.$storage['access_token'];
        vm.user.id = vm.$storage['user_id'];
        vm.user.name = vm.$storage['user_name'];
        vm.user.type = vm.$storage['user_type'];
    }
})();
