(function () {

    angular.module('tedushop.footer', ['tedushop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider']; //cái inject thực tế là khởi tạo các biến trung gian dùng trong ứng dụng
    //cái này công dụng giống router config trong mvc
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('footer', {
            url: "/footer_default",
            parent: "base",
            templateUrl: "/app/shared/footer/footerDefaultView.html",
            controller: "footerDefaultController"
        });
    }
})();