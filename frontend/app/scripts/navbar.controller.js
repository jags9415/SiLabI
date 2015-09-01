(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('NavBarController', NavBarController);

    NavBarController.$inject = ['$rootScope'];

    function NavBarController($rootScope) {
        var vm = this;
        vm.collapsed = true;
    }
})();
