(function() {
    'use strict';

    angular
        .module('silabi')
        .service('RequestService', RequestService);

    Service.$inject = ['$http', 'API_URL'];

    function RequestService($http, API_URL) {

      this.get = get;
      this.put = put;
      this.post = post;
      this.delete = delete;

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
      * @example get("/students", "?access_token=abcd1234&page=1")
      */
      function get(endpoint, queryString) {
        var url = join(API_URL, endpoint, queryString);
        return $http.get(url);
      }

      /**
      * Perform a POST request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example put("/students", {"name": "...", "username": "..."})
      */
      function post(enpoint, data) {
        var url = join(API_URL, endpoint);
        return $http.post(url, data);
      }

      /**
      * Perform a PUT request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example put("/students/201242273", {"name": "...", "username": "..."})
      */
      function put(endpoint, data) {
        var url = join(API_URL, endpoint);
        return $http.put(url, data);
      }

      /**
      * Perform a DELETE request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example put("/students/201242273", {"access_token": "..."})
      */
      function delete(endpoint, data) {
        var config = {
          method: "DELETE",
          headers: {"Content-Type": "application/json"},
          url : join(API_URL, endpoint),
          data : data
        }
        return $http(config);
      }
    }
})();
