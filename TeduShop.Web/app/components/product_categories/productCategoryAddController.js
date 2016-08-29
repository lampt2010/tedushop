(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);// tham số đầu tiên là tên controller dùng ở view còn thứ 2 là tên hàm nội bộ viết trong file này ! thông thường lấy tên trùng tên file
    productCategoryAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];// khoi tao cac doi tuong

    function productCategoryAddController(apiService, $scope, notificationService, $state, commonService) {// viết hàm tại đây như 1 hàm tạo
        $scope.productCategory = {// tạo ra 1 đối tượng có các giá trị mặc định khi vào form
            CreatedDate: new Date(),
            Status:true
        }

        $scope.AddProductCategory = AddProductCategory;
        function AddProductCategory() {
            apiService.post('api/productcategory/create', $scope.productCategory, function (result) {
                notificationService.displaySuccess('Thêm mới thành công ' + result.data.Name);
                $state.go('product_categories');// tư động chuyển về trang list category

            }, function (eror) {
                notificationService.displayError('Có lỗi xảy ra');

            })
        }
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            // notificationService.displaySuccess('vao ham');
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }


        $scope.parentCategories = [];
        function  loadParentCategory(){
            apiService.get('api/productcategory/getallparents',null,function(result){
                $scope.parentCategories = result.data;
            
            }, function () {
                console.log('Có lỗi xảy ra');

            })//goi ajax  ! null la tham so truyen vao
        }

        loadParentCategory();// goi ham load này ngay tại đây vì nó nằm trong productCategoryAddController nên tự động chạy

    }

})(angular.module('tedushop.product_categories'));// giong khai bao namespace chi ra controller này thuộc module productcategory 