(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AdminListController', AdminListController);
        AdminListController.$inject = ['AdminService', 'MessageService', '$location'];

    function AdminListController(AdminService, MessageService, $location) {

      var vm = this;
      vm.loaded = false;
      vm.administrators = [];
      vm.searched = {};
      vm.limit = 20;
      vm.advanceSearch = false;
      vm.request = {
        fields: "id,full_name,email,phone,username,state",
        sort: [{field: "created_at", type: "DESC"}]
      };

  	  vm.loadPage = loadPage;
      vm.open = openAdministrator;
      vm.delete = deleteAdministrator;
      vm.search = searchAdministrators;
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

        vm.states = [
          {
            name: 'Cualquiera',
            value: '*'
          },
          {
            name: 'Activo',
            value: 'Activo'
          },
          {
            name: 'Inactivo',
            value: 'Inactivo'
          }
        ];

        loadPage();
      }

  	  function loadPage() {
        $location.search('page', vm.page);

        vm.request.page = vm.page;
        vm.request.limit = vm.limit;

        AdminService.GetAll(vm.request)
        .then(handleGetSuccess)
        .catch(handleError);
    	};

      function createAdministrator() {
        $location.path('/Administrador/Administradores/Agregar');
      }

      function openAdministrator(id) {
        $location.path('/Administrador/Administradores/' + id);
      }

      function deleteAdministrator(id) {
        MessageService.confirm("¿Desea realmente eliminar este administrador?")
        .then(function(){
          AdminService.Delete(id)
          .then(handleDeleteSuccess)
          .catch(handleError)
        });
      }

      function searchAdministrators() {
        vm.request.query = {};

          if (vm.searched.full_name) {
            vm.request.query.full_name = {
              operation: "like",
              value: '*' + vm.searched.full_name.replace(' ', '*') + '*'
            }
          }

          if (vm.searched.username) {
            vm.request.query.username = {
              operation: "like",
              value: '*' + vm.searched.username + '*'
            }
          }

          if (vm.searched.state) {
            vm.request.query.state = {
              operation: "like",
              value: vm.searched.state.value
            }
          }

          if (vm.searched.email) {
            vm.request.query.email = {
              operation: "eq",
              value: vm.searched.email
            }
          }

          loadPage();
        }

        function toggleAdvanceSearch() {
          vm.advanceSearch = !vm.advanceSearch;
          delete vm.searched.state;
          delete vm.searched.email;
          delete vm.searched.username;
        }

        function handleDeleteSuccess() {
          loadPage();
        }

        function handleGetSuccess(data) {
          vm.administrators = data.results;
          vm.page = data.current_page;
          vm.totalPages = data.total_pages;
          vm.totalItems = vm.limit * vm.totalPages;
          vm.loaded = true;
        }

        function handleError(data) {
          MessageService.error(data.description);
          vm.loaded = true;
        }

        function isEmpty() {
          return vm.administrators.length == 0;
        }

        function isLoaded() {
          return vm.loaded;
        }
    }
})();
