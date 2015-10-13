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

      function parseSortField(obj) {
        var result = "";
        if (obj.type === "DESC") {
          result += "-";
        }
        result += obj.field;
        return result;
      }

      function parseSort(sort) {
        var result = "";

        if (_.isArray(sort)) {
          for (var i = 0; i < sort.length; i++) {
            result += parseSortField(sort[i]);
            if (i < sort.length - 1) result += ","
          }
        }
        else if (_.isString(sort)) {
          result = sort;
        }
        else if (_.isObject(sort)) {
          result = parseSortField(sort);
        }

        return result;
      }

      function parseQuery(query) {
        var result = "";
        var current;

        for (var property in query) {
          if (query.hasOwnProperty(property)) {
            current = query[property];
            if (_.isArray(current)) {
              for (var i = 0; i < current.length; i++) {
                result += property + '+' + current[i].operation + '+' + current[i].value + ',';
              }
            }
            else {
              result += property + '+' + current.operation + '+' + current.value + ',';
            }
          }
        }

        return result.substring(0, result.length - 1);
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

        if (request.sort && !_.isEmpty(request.sort)) {
          if (addAmpersand) query += "&";
          query += "sort=" + parseSort(request.sort);
          addAmpersand = true;
        }

        if (request.query && !_.isEmpty(request.query)) {
          if (addAmpersand) query += "&";
          query += "q=" + parseQuery(request.query);
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
