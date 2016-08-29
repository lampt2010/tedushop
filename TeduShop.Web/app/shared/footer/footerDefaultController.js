(function (app) {
    app.controller('footerDefaultController', footerDefaultController);// tham số đầu tiên là tên controller dùng ở view còn thứ 2 là tên hàm nội bộ viết trong file này ! thông thường lấy tên trùng tên file
    footerDefaultController.$inject = ['apiService', '$scope', 'notificationService', 'commonService', '$stateParams'];// khoi tao cac doi tuong

    function footerDefaultController(apiService, $scope, notificationService , commonService, $stateParams) {// viết hàm tại đây
        $scope.footer = {


        }

        $scope.ckeditorOptions = {
            language: 'vi',
            height: '500px',

        }

        $scope.UpdateFooter = UpdateFooter;
        function UpdateFooter() {
           // $scope.footer.Content = JSON.stringify($scope.Content);
            apiService.put('api/common/update', $scope.footer, function (result) {
                notificationService.displaySuccess('Sửa thành công ' + result.data.ID);
               // $state.go('products');// tư động chuyển về trang list 

            }, function (eror) {
                notificationService.displayError('Có lỗi xảy ra khi sửa');

            })
        }
        function loadFooterDetail() {

            apiService.get('api/common/getfooter', null, function (result) {
                $scope.footer = result.data;           
                //$scope.footer.Content = JSON.parse(result.data.Content);


            }, function (error) {
                notificationService.displayError(error);
            });
        }


        
        loadFooterDetail();
    }

})(angular.module('tedushop.common'));// giong khai bao namespace chi ra controller này thuộc module common 