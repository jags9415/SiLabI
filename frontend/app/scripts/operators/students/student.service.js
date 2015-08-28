(function() {
    'use strict';

    angular
        .module('silabi')
        .factory('StudentService', students);

    students.$inject = ['$http', 'API_URL'];

    function students($http, apiUrl) {
        var service = {
            getByPage: getByPage,
            getByUsername: getByUsername,
            update: update
        };

        return service;

        function getByPage(pageNumber) {
          return $http.get(apiUrl + '/students?page=' + pageNumber + '&access_token=123')
          .then(function(response) {
            return response.data;
          });
        }

        function getByUsername(username) {
          return $http.get(apiUrl + '/students/' + username)
          .then(function(response) {
            return response.data;
          });
        }

        function update(userID, newInfo) {
          return $http.put(apiUrl + '/students/' + userID , {
            'student': newInfo,
            'access_token': '1'
          })
          .then(function(response) {
            return response.data;
          });
        }

        /**
        CREATE

        {
    "student": {
        "email": "emmanueslq@hotmail.com",
        "id": "201240052",
        "last_name_1": "sanchez",
        "gender":"Masculino",
        "name": "murillo",
        "username":"201240052",
        "password": "0728374821",
        "phone": "89623157",
        "last_name_2": "morales"
    },
    "access_token": "ok"
}

**/
    }
})();
