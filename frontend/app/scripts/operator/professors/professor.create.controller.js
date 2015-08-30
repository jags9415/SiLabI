(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorsCreateController', ProfessorsCreateController);
        ProfessorsCreateController.$inject = ['$routeParams', 'ProfessorsService', '$location', 'MessageService'];

    function ProfessorsCreateController($routeParams, ProfessorsService, $location, MessageService) {
      
        var vm = this;
      	activate();
    	
    	vm.createProfessor = createProfessor;
        vm.generateUserName = generateUserName;

    	
    	function  checkProfessorCreateInput() 
    	{
    		if(!isNaN(vm.professor.name))
    		{
    			alert("Nombre incorrecto.");
    			return false;
    		}
    		else if(!isNaN(vm.professor.last_name_1))
    		{
    			alert("Primer Apellido incorrecto.");
    			return false;
    		}
    		else if(!isNaN(vm.professor.last_name_2))
    		{
    			alert("Segundo Apellido incorrecto.");
    			return false;
    		}
    		else if(isNaN(vm.professor.phone))
    		{
    			alert("Número telefónico incorrecto.");
    			return false;
    		}
    		else
    		{
    			return true;
    		}
    	}


        function generateUserName()
        {
            if(vm.professor.name && vm.professor.last_name_1)
            {
                vm.professor.username = (vm.professor.name.substring(0, 1) + vm.professor.last_name_1).toLowerCase(); 
            }
            else
            {
                vm.professor.username = null;
            } 
        }


    	function createProfessor()
    	{
    		if(checkProfessorCreateInput() && vm.professor != null)
    		{
                var hash = CryptoJS.SHA256(vm.professor.password).toString(CryptoJS.enc.Hex);
    			var jsonObject = 
				{
	              "professor": {
	                "email": vm.professor.email,
	                "gender": vm.selected_gender,
	                "last_name_1": vm.professor.last_name_1,
	                "last_name_2": vm.professor.last_name_2,
	                "name": vm.professor.name,
	                "phone": vm.professor.phone,
	                "username": vm.professor.username,
	                "password": hash
	              },
	              "access_token":""
	            }
	            console.log(jsonObject);
    			ProfessorsService.Create(jsonObject)
	    		.then(handleCreateSuccess)
                .catch(handleError);
	    	}
    	}

        function handleCreateSuccess() {
          MessageService.success("Docente creado con éxito.");
        }

        function handleError(data) {
          MessageService.error(data.description);
        }

    	function activate()
    	{
    		vm.genders = [{"name":"Masculino"}, {"name":"Femenino"}];
    		var name = "undefined";
            if(typeof(sessionStorage) != 'undefined')
            {
                if(sessionStorage.getItem('user_name') != null)
                {
                    name = sessionStorage.getItem('user_name');
                }
            }
            vm.user_name = name;
            vm.professor = 
            {
                "email": "",
                "gender": "",
                "last_name_1": "",
                "last_name_2": "",
                "name": "",
                "phone": "",
                "username": "",
                "password": ""
          };
    	}
    }
})();