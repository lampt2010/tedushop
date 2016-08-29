(function (app) {
    app.controller('productAddController', productAddController);// tham số đầu tiên là tên controller dùng ở view còn thứ 2 là tên hàm nội bộ viết trong file này ! thông thường lấy tên trùng tên file
    productAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];// khoi tao cac doi tuong

    function productAddController(apiService, $scope, notificationService, $state, commonService) {// viết hàm tại đây
        $scope.product = {
            CreatedDate: new Date(),
            Status: true,
            Price: 0,
            HotFlag: true,
            HomeFlag: false

        }     

        $scope.ckeditorOptions = {
            language: 'en',
            height: '200px',
         
            uiColor: '#14B8C4',
           
        }


        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            //  notificationService.displaySuccess('vao ham');
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        $scope.AddProduct = AddProduct;
        function AddProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);


            apiService.post('api/product/create', $scope.product, function (result) {
                notificationService.displaySuccess('Thêm mới thành công ' + result.data.Name);
                $state.go('products');// tư động chuyển về trang list 

            }, function (eror) {
                notificationService.displayError('Có lỗi xảy ra khi thêm');

            })
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
                $scope.$apply(function () {
                    $scope.product.Image = fileUrl;

                });
                $('<img style="width:200px" src=' + $scope.product.Image + ' alt=' + $scope.product.Name + '>').appendTo("#anhdaidien");
            }
            finder.popup();
            
        }

        $scope.moreImages = [];
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



        loadProductCategory();// goi ham load này ngay tại đây vì nó nằm trong productAddController nên tự động chạy
      
    }

})(angular.module('tedushop.products'));// giong khai bao namespace chi ra controller này thuộc module productcategory 