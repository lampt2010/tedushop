(function (app) {

    app.controller('rootController', rootController);

    rootController.$inject = ['$state', 'authData', 'loginService', '$scope', '$rootScope', 'authenticationService'];

    function rootController($state, authData, loginService, $scope, $rootScope, authenticationService) {
        $scope.logOut = function () {
            loginService.logOut();
            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;

        authenticationService.validateRequest();

        $scope.stopPropagation = function ($event) { $event.preventDefault(); }
    }
})(angular.module('tedushop'));