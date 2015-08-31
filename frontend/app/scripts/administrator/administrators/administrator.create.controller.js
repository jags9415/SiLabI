(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AdministratorCreateController', AdministratorCreateController);

    AdministratorCreateController.$inject = ['AdminService', 'StudentService', 'ProfessorService','MessageService'];

    function AdministratorCreateController(AdminService, StudentService, ProfessorService, MessageService) {
        var vm = this;
        vm.search = search;
        vm.create = create;

        activate();


        function activate() 
        {
          vm.user = {};
        }

        function searchProfessor()
        {
          if (vm.username) 
          {
            ProfessorService.GetOne(vm.username)
            .then(handleSearchSuccess)
            .catch(handleError);
          }
        }

        function search() 
        {
          if (vm.username) 
          {
            StudentService.GetOne(vm.username)
            .then(function(response)
            {
              if(response!= null)
              {
                handleSearchSucces(response);
              }
            },
            function error(response)
            {
                searchProfessor();
            });
          }
        }


        function create() 
        {
          AdminService.Create(vm.user.id)
          .then(handleCreateSuccess)
          .catch(handleError);
        }

        function handleSearchSuccess(User) 
        {
          vm.user = User;
        }

        function handleCreateSuccess() 
        {
          MessageService.success("Administrador creado con éxito.");
        }

        function handleError(data) 
        {
          MessageService.error(data.description);
        }
    }
})();
