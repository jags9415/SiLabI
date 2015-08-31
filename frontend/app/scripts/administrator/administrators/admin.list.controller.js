(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AdminListController', AdminListController);
        AdminListController.$inject = ['AdminService', 'MessageService', '$location'];

    function AdminListController(AdminService, MessageService, $location) {
        
        var vm = this;
        vm.loaded = false;
        vm.searched = {};
        vm.limit = 20;
        vm.advanceSearch = false;
        vm.request = {
          fields : "id,full_name,username,state,period.value,period.year,period.type"
        };

      	activate();
      	    	
    	vm.loadPage = loadPage;
        vm.open = openAdministrator;
        vm.delete = deleteAdministrator;
        vm.search = searchAdministrators;
        vm.isEmpty = isEmpty;
        vm.isLoaded = isLoaded;
        vm.loadPage = loadPage;
        vm.toggleAdvanceSearch = toggleAdvanceSearch;

    	 function loadPage()
    	{
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
          $location.path('/Administrador/Operadores/' + id);
        }

        function deleteAdministrator(id) {
          AdminService.Delete(id)
          .then(handleDeleteSuccess)
          .catch(handleError);
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

          if (vm.searched.period) {
            if (vm.searched.period.value) {
              vm.request.query["period.value"] = {
                operation: "eq",
                value: vm.searched.period.value
              }
            }
            if (vm.searched.period.type) {
              vm.request.query["period.type"] = {
                operation: "eq",
                value: vm.searched.period.type
              }
            }
          }

          if (vm.searched.year) {
            vm.request.query["period.year"] = {
              operation: "eq",
              value: vm.searched.year
            }
          }

          if (vm.searched.email) {
              vm.request.query["email"] = {
                operation: "eq",
                value: vm.searched.email
              }
            }

          loadPage();
        }

        function toggleAdvanceSearch() 
        {
          vm.advanceSearch = !vm.advanceSearch;
          delete vm.searched.state;
          delete vm.searched.year;
          delete vm.searched.period;
          delete vm.searched.username;
        }

        function handleDeleteSuccess() 
        {
          MessageService.success("Administrador eliminado.");
          loadPage();
        }

        function handleGetSuccess(data) 
        {
          vm.administrators = data.results;
          vm.page = data.current_page;
          vm.totalPages = data.total_pages;
          vm.totalItems = vm.limit * vm.totalPages;
          vm.loaded = true;
        }

        function handleError(data) 
        {
          MessageService.error(data.description);
          vm.loaded = true;
        }

        function isEmpty() 
        {
          return vm.administrators.length == 0;
        }

        function isLoaded() 
        {
          return vm.loaded;
        }

    	function activate()
    	{
            var page = parseInt($location.search()['page']);

            if (isNaN(page)) {
                page = 1;
             }
            vm.totalPages = page;
            vm.page = page;
    		vm.administrators = [];
            var name = "undefined";
            var accessToken = -1;
            if(typeof(sessionStorage) != 'undefined')
            {
                if(sessionStorage.getItem('user_name') != null)
                {
                    name = sessionStorage.getItem('user_name');
                }
            }
            vm.states = 
            [
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

            vm.periods = [
            {
              value: 1,
              type: 'Semestre'
            },
            {
              value: 2,
              type: 'Semestre'
            },
            {
              value: 1,
              type: 'Cuatrimestre'
            },
            {
              value: 2,
              type: 'Cuatrimestre'
            },
            {
              value: 3,
              type: 'Cuatrimestre'
            },
            {
              value: 1,
              type: 'Trimestre'
            },
            {
              value: 2,
              type: 'Trimestre'
            },
            {
              value: 3,
              type: 'Trimestre'
            },
            {
              value: 4,
              type: 'Trimestre'
            },
            {
              value: 1,
              type: 'Bimestre'
            },
            {
              value: 2,
              type: 'Bimestre'
            },
            {
              value: 3,
              type: 'Bimestre'
            },
            {
              value: 4,
              type: 'Bimestre'
            },
            {
              value: 5,
              type: 'Bimestre'
            },
            {
              value: 6,
              type: 'Bimestre'
            }
          ];
            vm.user_name = name;
            loadPage();
    	}
    }
})();