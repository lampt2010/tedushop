(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);// tham số đầu tiên là tên controller dùng ở view còn thứ 2 là tên hàm nội bộ viết trong file này ! thông thường lấy tên trùng tên file
    productCategoryEditController.$inject = ['apiService', '$scope', 'notificationService', '$state','$stateParams','commonService'];// khoi tao cac doi tuong

    function productCategoryEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {// viết hàm tại đây
        $scope.productCategory = {
            CreatedDate: new Date(),
            Status: true
        }

        $scope.UpdateProductCategory = UpdateProductCategory;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
          //  notificationService.displaySuccess('vao ham');
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }


        function loadProductCategoryDetail() {
          

            apiService.get('api/productcategory/getbyid/' +$stateParams.id,null, function (result) {
                $scope.productCategory = result.data;
            }, function (error) {
                notificationService.displayError(error);
            });
        }

        $scope.ChooseMoreImage = ChooseMoreImage;
        function ChooseMoreImage() {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                });

            }
            finder.popup();
        }

        function UpdateProductCategory() {
            apiService.put('api/productcategory/update', $scope.productCategory, function (result) {
                notificationService.displaySuccess('Sửa thành công ' + result.data.Name);
                $state.go('product_categories');// tư động chuyển về trang list 

            }, function (eror) {
                notificationService.displayError('Có lỗi xảy ra');

            })
        }



        $scope.parentCategories = [];
        function loadParentCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.parentCategories = result.data;

            }, function () {
                console.log('Có lỗi xảy ra');

            })//goi ajax  ! null la tham so truyen vao
        }

        loadParentCategory();// goi ham load này ngay tại đây vì nó nằm trong productCategoryAddController nên tự động chạy
        loadProductCategoryDetail();
    }

})(angular.module('tedushop.product_categories'));// giong khai bao namespace chi ra controller này thuộc module productcategory 