(function () {

    angular.module('tedushop.product_categories', ['tedushop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider']; //cái inject thực tế là khởi tạo các biến trung gian dùng trong ứng dụng
    //cái này công dụng giống router config trong mvc
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('product_categories', {
            url: "/product_categories",
            parent:"base",
            templateUrl: "/app/components/product_categories/productCategoryListView.html",
            controller: "productCategoryListController"

        }).state('add_product_category', {
            url: "/add_product_category",
            parent:"base",
            templateUrl: "/app/components/product_categories/productCategoryAddView.html",
            controller: "productCategoryAddController"
        }).state('edit_product_category', {
            url: "/edit_product_category:id",
            parent:"base",
            templateUrl: "/app/components/product_categories/productCategoryEditView.html",
            controller: "productCategoryEditController"
        });
      



    }
})();