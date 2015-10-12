(function() {
    'use strict';

    angular
        .module('silabi')
        .service('RequestService', RequestService);

    RequestService.$inject = ['$http', '$q', 'API_URL'];

    function RequestService($http, $q, API_URL) {

      this.get = getRequest;
      this.put = putRequest;
      this.post = postRequest;
      this.delete = deleteRequest;

      /**
      * Join a base URL and a endpoint.
      * @param baseUlr The base URL.
      * @param endpoint The endpoint.
      * @return The joined url.
      * @example join("http://localhost/api/v1/", "/students/201242273") => "http://localhost/api/v1/students/201242273"
      */
      function join(baseUrl, endpoint) {
        if (baseUrl.endsWith("/")) {
          baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        if (!endpoint.startsWith("/")) {
          endpoint = "/" + endpoint;
        }
        return baseUrl + endpoint;
      }

      /**
      * Create a query string based on a request object.
      * @param request The request.
      * @return The query string representation of the request.
      */
      function createQueryString(request) {
        var query = "?";
        var addAmpersand = false;

        if (request.access_token) {
          if (addAmpersand) query += "&";
          query += "access_token=" + request.access_token;
          addAmpersand = true;
        }

        if (request.fields) {
          if (addAmpersand) query += "&";
          query += "fields=" + request.fields;
          addAmpersand = true;
        }

        if (request.page) {
          if (addAmpersand) query += "&";
          query += "page=" + request.page;
          addAmpersand = true;
        }

        if (request.limit) {
          if (addAmpersand) query += "&";
          query += "limit=" + request.limit;
          addAmpersand = true;
        }

        if (request.sort && request.sort.length > 0) {
          if (addAmpersand) query += "&";
          query += "sort=";

          for (var i = 0; i < request.sort.length; i++) {
            if (request.sort[i].type === "DESC") {
              query += "-";
            }
            query += request.sort[i].field;
            if (i < request.sort.length - 1) query += ","
          }

          addAmpersand = true;
        }

        if (request.query && !_.isEmpty(request.query)) {
          var addComma = false;
          if (addAmpersand) query += "&";
          query += "q=";
          for (var property in request.query) {
            if (request.query.hasOwnProperty(property)) {
              if (addComma) query += ',';
              query += property + '+' + request.query[property].operation + '+' + request.query[property].value;
              addComma = true;
            }
          }
          addAmpersand = true;
        }

        return query;
      }

      function serviceUnavailableResponse() {
        return {
          code: 503 ,
          error: "ServiceUnavailable",
          description: "El servidor no se encuentra disponible. Inténtelo luego."
        }
      }

      /**
      * Perform a GET request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param request The request.
      * @return A promise.
      * @example getRequest("/students", object)
      */
      function getRequest(endpoint, request) {
        var defer = $q.defer();
        var url = join(API_URL, endpoint);

        if (request) {
          var queryString = createQueryString(request);
          url += queryString;
        }

        $http.get(url)
        .then(
          function(response) {
            defer.resolve(response.data)
          },
          function(response) {
            if (response.status == 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data)
          }
        );

        return defer.promise;
      }

      /**
      * Perform a POST request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example postRequest("/students", {"name": "...", "username": "..."})
      */
      function postRequest(endpoint, data) {
        var url = join(API_URL, endpoint);
        var defer = $q.defer();

        $http.post(url, data)
        .then(
          function(response) {
            defer.resolve(response.data)
          },
          function(response) {
            if (response.status == 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data)
          }
        );

        return defer.promise;
      }

      /**
      * Perform a PUT request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example putRequest("/students/201242273", {"name": "...", "username": "..."})
      */
      function putRequest(endpoint, data) {
        var url = join(API_URL, endpoint);
        var defer = $q.defer();

        $http.put(url, data)
        .then(
          function(response) {
            defer.resolve(response.data)
          },
          function(response) {
            if (response.status == 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data)
          }
        );

        return defer.promise;
      }

      /**
      * Perform a DELETE request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example deleteRequest("/students/201242273", {"access_token": "..."})
      */
      function deleteRequest(endpoint, data) {
        var defer = $q.defer();
        var config = {
          method: "DELETE",
          headers: {"Content-Type": "application/json"},
          url : join(API_URL, endpoint),
          data : data
        }

        $http(config)
        .then(
          function(response) {
            defer.resolve(response.data)
          },
          function(response) {
            if (response.status == 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data)
          }
        );
        
        return defer.promise;
      }
    }
})();
