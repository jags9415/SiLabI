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
      * @param queryString The optional queryString.
      * @return The joined url.
      * @example join("http://localhost/api/v1/", "/students/201242273") => "http://localhost/api/v1/students/201242273"
      * @example join("http://localhost/api/v1/", "/students/201242273", "?id=1") => "http://localhost/api/v1/students/201242273?id=1"
      */
      function join(baseUrl, endpoint, queryString) {
        if (baseUrl.endsWith("/")) {
          baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        if (!endpoint.startsWith("/")) {
          endpoint = "/" + endpoint;
        }
        var url = baseUrl + endpoint;
        if (queryString) {
          url += queryString;
        }
        return url;
      }

      /**
      * Perform a GET request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param queryString The query string.
      * @return A promise.
      * @example getRequest("/students", "?access_token=abcd1234&page=1")
      */
      function getRequest(endpoint, queryString) {
        var url = join(API_URL, endpoint, queryString);
        var defer = $q.defer();

        $http.get(url)
        .then(function(response) { defer.resolve(response.data) }, function(response) { defer.reject(response.data) });

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
        .then(function(response) { defer.resolve(response.data) }, function(response) { defer.reject(response.data) });

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
        .then(function(response) { defer.resolve(response.data) }, function(response) { defer.reject(response.data) });
        
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
        .then(function(response) { defer.resolve(response.data) }, function(response) { defer.reject(response.data) });
        
        return defer.promise;
      }
    }
})();
