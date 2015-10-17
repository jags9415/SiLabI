(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('SoftwareListController', SoftwareListController);

    SoftwareListController.$inject = ['$scope', 'SoftwareService', 'MessageService', 'StateService', '$location'];

    function SoftwareListController($scope, SoftwareService, MessageService, StateService, $location) {
      var vm = this;

      vm.loaded = false;
      vm.softwareList = [];
      vm.searched = {};
      vm.limit = 20;

      vm.request = {
        fields : 'id,code,name,state',
        sort: {field: 'code', type: 'ASC'}
      };

      vm.states = [];

      vm.open = openSoftware;
      vm.delete = deleteSoftware;
      vm.search = searchSoftware;
      vm.isEmpty = isEmpty;
      vm.isLoaded = isLoaded;
      vm.loadPage = loadPage;
      vm.toggleAdvanceSearch = toggleAdvanceSearch;

      activate();

      function activate() {
        var page = parseInt($location.search()['page']);

        if (isNaN(page)) {
          page = 1;
        }

        vm.totalPages = page;
        vm.page = page;
        loadPage(true);

        StateService.GetLabStates()
        .then(setStates)
        .catch(handleError);
      }

      function loadPage(cached) {
        $location.search('page', vm.page);

        vm.request.page = vm.page;
        vm.request.limit = vm.limit;

        vm.promise = SoftwareService.GetAll(vm.request, cached)
        .then(setSoftware)
        .catch(handleError);
      }

      function openSoftware(id) {
        $location.url('/Operador/Software/' + id);
      }

      function searchSoftware() {
        vm.request.query = {};

        if (vm.searched.name) {
          vm.request.query.name = {
            operation: 'like',
            value: '*' + vm.searched.name.replace(' ', '*') + '*'
          };
        }

        if (vm.searched.code) {
          vm.request.query.code = {
            operation: 'eq',
            value: vm.searched.code
          };
        }

        if (vm.searched.state) {
        vm.request.query.state = {
          operation: 'like',
          value: vm.searched.state.value
        };
      }


        loadPage();
      }

      function toggleAdvanceSearch() {
      vm.advanceSearch = !vm.advanceSearch;
      delete vm.searched.code;
      delete vm.searched.name;
      delete vm.searched.state;
    }


      function isEmpty() {
        return vm.softwareList.length === 0;
      }

      function isLoaded() {
        return vm.loaded;
      }

      function setSoftware(data) {
        vm.softwareList = data.results;
        vm.page = data['current_page'];
        vm.totalPages = data['total_pages'];
        vm.totalItems = vm.limit * vm.totalPages;
        vm.loaded = true;
      }

      function setStates(states) {
      vm.states = states;
    }

      function deleteSoftware(id) {
        MessageService.confirm('¿Desea realmente eliminar este software?')
        .then(function() {
          SoftwareService.Delete(id)
          .then(loadPage)
          .catch(handleError);
        });
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
