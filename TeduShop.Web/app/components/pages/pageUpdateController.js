(function (app) {
    app.controller('pageUpdateController', pageUpdateController);// tham số đầu tiên là tên controller dùng ở view còn thứ 2 là tên hàm nội bộ viết trong file này ! thông thường lấy tên trùng tên file
    pageUpdateController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];// khoi tao cac doi tuong

    function pageUpdateController(apiService, $scope, notificationService, $state, $stateParams, commonService) {// viết hàm tại đây
        $scope.page = {
           
        }

        $scope.UpdatePage = UpdatePage;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            //  notificationService.displaySuccess('vao ham');
            $scope.page.Alias = commonService.getSeoTitle($scope.page.Name);
        }

        function UpdatePage() {
            apiService.put('api/page/update', $scope.page, function (result) {
                notificationService.displaySuccess('Sửa thành công ' + result.data.Name);
                $state.go('pages');// tư động chuyển về trang list 

            }, function (eror) {
                notificationService.displayError('Có lỗi xảy ra');

            })
        }
        function loadPageDetail() {


            apiService.get('api/page/getbyid/' + $stateParams.id, null, function (result) {
                $scope.page = result.data;
            }, function (error) {
                notificationService.displayError(error);
            });
        }

        loadPageDetail();

      // goi ham load này ngay tại đây vì nó nằm trong productCategoryAddController nên tự động chạy
        
    }

})(angular.module('tedushop.pages'));// giong khai bao namespace chi ra controller này thuộc module productcategory 