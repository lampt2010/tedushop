
/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function () {

    angular.module('tedushop.pages', ['tedushop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider']; //cái inject thực tế là khởi tạo các biến trung gian dùng trong ứng dụng
    //cái này công dụng giống router config trong mvc
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('pages', {
            url: "/pages",
            parent: "base",
            templateUrl: "/app/components/pages/pagesListView.html",
            controller: "pagesListController"

        }).state('add_page', {
            url: "/add_page",
            parent: "base",
            templateUrl: "/app/components/pages/pageAddView.html",
            controller: "pageAddController"
        }).state('edit_page', {
            url: "/edit_page/:id",
            parent: "base",
            templateUrl: "/app/components/pages/pageUpdateView.html",
            controller: "pageUpdateController"
        });
    }
})();