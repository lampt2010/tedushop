/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.factory('apiService', apiService);// khai bao sevice
    apiService.$inject = ['$http','notificationService','authenticationService'];//notificationservice chi la thu vien giup hien thi thong bao
    function apiService($http, notificationService, authenticationService) {
        return {
            get: get,
            post: post,
            put: put,
            del:del


        }


        function del(url, data, success, failure) {
            authenticationService.setHeader();
            $http.delete(url, data).then(function (result) {
                // notificationService.displaySuccess('Thêm mới thành công');
                success(result);
            }, function (error) {
                if (error.status == 401) {
                    notificationService.displayError('Authenticate is required');
                } else if (failure != null) {
                    failure(error);
                }



            });

        }
        function put(url, data, success, failure) {
            authenticationService.setHeader();
            $http.put(url, data).then(function (result) {
                // notificationService.displaySuccess('Thêm mới thành công');
                success(result);
            }, function (error) {
                if (error.status == 401) {
                    notificationService.displayError('Authenticate is required');
                } else if (failure != null) {
                    failure(error);
                }



            });

        }
        function post(url, data, success, failure) {
            authenticationService.setHeader();
            $http.post(url, data).then(function (result) {
               // notificationService.displaySuccess('Thêm mới thành công');
                success(result);
            }, function (error) {
                if (error.status == 401) {
                    notificationService.displayError('Authenticate is required');
                } else if(failure!=null) {
                    failure(error);
                }

              
                
            });

        }
        function get(url, params, success, failed) {
            authenticationService.setHeader();
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                failed(error);
            });
        }

    }



})(angular.module('tedushop.common'));