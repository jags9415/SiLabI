(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorsDetailController', ProfessorsDetailController);
        ProfessorsDetailController.$inject = ['$routeParams', 'ProfessorsService', '$route', '$location', 'MessageService'];

    function ProfessorsDetailController($routeParams, ProfessorsService, $route, $location, MessageService) {
        var vm = this;
      	activate();

        vm.modifyProfessor = modifyProfessor;
        vm.deleteProfessor = deleteProfessor;
      	
        function  checkProfessorModifyInput() 
        {
            if(!isNaN(vm.professorModifyName))
            {
                alert("Nombre incorrecto.");
                return false;
            }
            else if(!isNaN(vm.professorModifyLastName1))
            {
                alert("Primer Apellido incorrecto.");
                return false;
            }
            else if(!isNaN(vm.professorModifyLastName2))
            {
                alert("Segundo Apellido incorrecto.");
                return false;
            }
            else
            {
                return true;
            }
        }

        function retrieveProfessor()
        {
            vm.professorUserName =  $routeParams.username;
            ProfessorsService.GetOne(vm.professorUserName).
            then(function(response)
            {
                vm.professor = response;
                vm.professorid = vm.professor.id;
                console.log(response);
            },
            function(error)
            {
                alert("Error al obtener datos de docente. "+error.description);
            });
        }

        function modifyProfessor()
        {
            if(checkProfessorModifyInput())
            {
                var hash = CryptoJS.SHA256(vm.professor.password).toString(CryptoJS.enc.Hex);
                var jsonObject =
                {
                    "professor":
                    {
                        "email": vm.professor.email,
                        "last_name_1": vm.professor.last_name_1,
                        "last_name_2": vm.professor.last_name_2,
                        "password": hash,
                        "phone": vm.professor.phone
                    },
                    "access_token":""
                };
                ProfessorsService.Update(vm.professorid, jsonObject)
                .then(handleGetSuccess)
                .catch(handleError);
            }
        }

        function deleteProfessor()
        {
            ProfessorsService.Delete(vm.professorid)
            .then(handleDeleteSuccess)
            .catch(handleError);
        }

        function handleGetSuccess(data) {
          vm.professor = data;
          MessageService.success("Docente actualizado.");
        }

        function handleDeleteSuccess() {
          MessageService.success("Docente eliminado.");
          $location.path("/Operador/Docentes");
        }

        function handleError(data) {
          MessageService.error(data.description);
        }

    	function activate()
    	{
    		vm.genders = [{"name":"Masculino"}, {"name":"Femenino"}];
            var accessToken = -1;
            if(typeof(sessionStorage) != 'undefined' && sessionStorage.getItem('access_token') != null)
            {
                accessToken = sessionStorage.getItem('access_token');
            }
            vm.professorid = null;
            vm.professor = null;
            retrieveProfessor();
    	}
    }
})();