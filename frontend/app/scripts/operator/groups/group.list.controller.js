(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('GroupListController', GroupListController);

    GroupListController.$inject = ['$scope', 'GroupService', 'MessageService', '$location'];

    function GroupListController($scope, GroupService, MessageService, $location) {
      var vm = this;

      vm.loaded = false;
      vm.groups = [];
      vm.searched = {};
      vm.limit = 20;
      vm.request = {
        fields : "id,number,course,state"
      };

      vm.open = openGroup;
      vm.delete = deleteGroup;
      vm.search = searchGroup;
      vm.isEmpty = isEmpty;
      vm.isLoaded = isLoaded;
      vm.loadPage = loadPage;

      activate();

      function activate() {
        var page = parseInt($location.search()['page']);

        if (isNaN(page)) {
          page = 1;
        }

        vm.totalPages = page;
        vm.page = page;
        loadPage();
      }

      function loadPage() {
        $location.search('page', vm.page);

        vm.request.page = vm.page;
        vm.request.limit = vm.limit;

        GroupService.GetAll(vm.request)
        .then(setGroups)
        .catch(handleError);
      }

      function openGroup(id) {
        $location.url('/Operador/Grupos/' + id);
      }

      function searchGroup() {
        vm.request.query = {};

        if (vm.searched.number) {
          vm.request.query.number = {
            operation: "like",
            value: '*' + vm.searched.number.replace(' ', '*') + '*'
          }
        }

        if (vm.searched.name) {
          vm.request.query['course.name'] = {
            operation: "like",
            value: '*' + vm.searched.name.replace(' ', '*') + '*'
          }
        }

        loadPage();
      }

      function isEmpty() {
        return vm.groups.length == 0;
      }

      function isLoaded() {
        return vm.loaded;
      }

      function setGroups(data) {
        vm.groups = data.results;
        vm.page = data.current_page;
        vm.totalPages = data.total_pages;
        vm.totalItems = vm.limit * vm.totalPages;
        vm.loaded = true;
      }

      function deleteGroup(id) {
        MessageService.confirm("¿Desea realmente eliminar este grupo?")
        .then(function() {
          GroupService.Delete(id)
          .then(loadPage)
          .catch(handleError);
        });
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();