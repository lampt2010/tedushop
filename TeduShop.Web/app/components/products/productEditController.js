(function (app) {
    app.controller('productEditController', productEditController);// tham số đầu tiên là tên controller dùng ở view còn thứ 2 là tên hàm nội bộ viết trong file này ! thông thường lấy tên trùng tên file
    productEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];// khoi tao cac doi tuong

    function productEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {// viết hàm tại đây
        $scope.product = {
           

        }

        $scope.ckeditorOptions = {
            language: 'vi',
            height: '200px',

        }


        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            //  notificationService.displaySuccess('vao ham');
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        $scope.UpdateProduct = UpdateProduct;
        function UpdateProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);
            apiService.put('api/product/update', $scope.product, function (result) {
                notificationService.displaySuccess('Sửa thành công ' + result.data.Name);
                $state.go('products');// tư động chuyển về trang list 

            }, function (eror) {
                notificationService.displayError('Có lỗi xảy ra khi sửa');

            })
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



        function loadProductDetail() {

            apiService.get('api/product/getbyid/' + $stateParams.id, null, function (result) {
                $scope.product = result.data;

                if (result.data.Image != undefined || result.data.Image != '') {
                    $('<img style="width:200px" src=' + $scope.product.Image + ' alt=' + $scope.product.Name + '>').appendTo("#anhdaidien");
                }

                $scope.moreImages = JSON.parse($scope.product.MoreImages);


            }, function (error) {
                notificationService.displayError(error);
            });
        }


        $scope.productCategories = [];
        function loadProductCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = result.data;

            }, function () {
                console.log('Có lỗi xảy ra khi load productCategories');

            })//goi ajax  ! null la tham so truyen vao
        }
        $scope.ChooseImage = ChooseImage;

        function ChooseImage() {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.product.Image = fileUrl;
                $('<img style="width:200px" src=' + $scope.product.Image + ' alt=' + $scope.product.Name + '>').appendTo("#anhdaidien");
            }
            finder.popup();

        }
        loadProductCategory();// goi ham load này ngay tại đây vì nó nằm trong productAddController nên tự động chạy
        loadProductDetail();
    }

})(angular.module('tedushop.products'));// giong khai bao namespace chi ra controller này thuộc module productcategory 