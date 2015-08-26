(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorsProfessorsController', OperatorsProfessorsController);
        OperatorsProfessorsController.$inject = ['$scope', '$routeParams', 'OperatorsService', 'ProfessorsService', '$route', '$location'];

    function OperatorsProfessorsController($scope, $routeParams, OperatorsService, ProfessorsService, $route, $location) {
      
      	init();

        $scope.modifyProfessor = modifyProfessor;
        $scope.deleteProfessor = deleteProfessor;
      	
        function  checkProfessorModifyInput() 
        {
            if(!isNaN($scope.professorModifyName))
            {
                alert("Nombre incorrecto.");
                return false;
            }
            else if(!isNaN($scope.professorModifyLastName1))
            {
                alert("Primer Apellido incorrecto.");
                return false;
            }
            else if(!isNaN($scope.professorModifyLastName2))
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
            $scope.professorUserName =  $routeParams.userName;
            ProfessorsService.getProfessorByUserName($scope.professorUserName, "a"/*$scope.access_token*/ ).
            then(function(response)
            {
                $scope.professor = response;
                $scope.professorid = $scope.professor.id;
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
                var jsonObject =
                {
                    "professor":
                    {
                        "email": $scope.professor.email,
                        "last_name_1": $scope.professor.last_name_1,
                        "last_name_2": $scope.professor.last_name_2,
                        "phone": $scope.professor.phone
                    },
                    "access_token":$scope.access_token
                };
                ProfessorsService.updateProfessor($scope.professorid, jsonObject).
                then(function(response)
                {
                    $scope.professor = response;
                    console.log(response);
                },
                function(error)
                {
                    alert("Error al modificar datos de docente." + error.description);
                });
            }
        }

        function deleteProfessor()
        {
            console.log("Deleting professor: "+$scope.professorid+" access_token: "+$scope.access_token);
            ProfessorsService.deleteProfessor($scope.professorid, $scope.access_token).
                then(function(response)
                {
                    $location.path("/Operador/Docentes");
                },
                function(error)
                {
                    alert("Error al modificar datos de docente." + error.description);
                });
        }

    	function init()
    	{
    		$scope.genders = [{"name":"Masculino"}, {"name":"Femenino"}];
            var accessToken = -1;
            if(typeof(sessionStorage) != 'undefined' && sessionStorage.getItem('access_token') != null)
            {
                accessToken = sessionStorage.getItem('access_token');
            }
            $scope.access_token = accessToken;
            $scope.professorid = null;
            $scope.professor = null;
            retrieveProfessor();
    	}
    }
})();