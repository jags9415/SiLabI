(function() {
    'use strict';

    angular
        .module('silabi')
        .service('MessageService', MessageService);

    MessageService.$inject = ['toastr', '$q'];

    function MessageService(toastr, $q) {

        this.success = success;
        this.info = info;
        this.warning = warning;
        this.error = error;

        /**
        * Show a message.
        * @param type The message type (success, info, warning, error).
        * @param content The message content.
        * @param title The message title. Optional.
        */
        function message(type, content, title) {
          toastr[type](content, title);
        }

        /**
        * Show a success message.
        * @param content The message content.
        * @param title The message title. Optional.
        */
        function success(content, title) {
          message("success", content, title);
        }

        /**
        * Show a info message.
        * @param content The message content.
        * @param title The message title. Optional.
        */
        function info(content, title) {
          message("info", content, title);
        }

        /**
        * Show a warning message.
        * @param content The message content.
        * @param title The message title. Optional.
        */
        function warning(content, title) {
          message("warning", content, title);
        }

        /**
        * Show a error message.
        * @param content The message content.
        * @param title The message title. Optional.
        */
        function error(content, title) {
          message("error", content, title);
        }
    }
})();
