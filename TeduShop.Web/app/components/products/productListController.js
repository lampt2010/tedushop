(function (app) {

    app.controller('productListController', productListController);
    productListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];//ngBootbox la thu vien su dung de tao box thong bao

    function productListController ($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.products = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getProducts = getProducts;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteProduct = deleteProduct;
        $scope.isAll = false;
        $scope.deleteMulti = deleteMulti;
        function deleteMulti() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            })

            var config = {

                params: {
                    listProducts: JSON.stringify(listId)
                }
            }
            apiService.del('api/product/deletemulti', config, function (result) {
                notificationService.displaySuccess('Đã xóa thành công ' + $scope.selected.length + ' mục');
                search();

            }, function () {
                notificationService.displayError('Xóa thất bại');
            })
        }




        $scope.CheckAll = CheckAll;
        function CheckAll() {
            if ($scope.isAll == false) {
                angular.forEach($scope.products, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.products, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }



        $scope.$watch("products", function (n, o) {// enable nut xoa nhieu neu co checked

            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);



        function deleteProduct(id) {

            $ngBootbox.confirm('Bạn có chắc muốn xóa ko ?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/product/delete', config, function (result) {

                    notificationService.displaySuccess('Xóa thành công ' + result.data.Name);
                    search();
                }, function () {

                    notificationService.displayError('Xoá thất bại ');
                })
            });
        }




        function search() {
            getProducts();
        }

        function getProducts(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 4//thay doi so item lay ra
                }
            }



            apiService.get('api/product/getall',
                config,
                function (result) {
                    if (result.data.TotalCount == 0) {
                        notificationService.displayWarning('Không có bản ghi nào được tìm thấy !')
                    } else {
                        notificationService.displaySuccess('Có ' + result.data.TotalCount + ' bản ghi được tìm thấy !')

                    }


                    $scope.products = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPage;
                    $scope.totalCount = result.data.TotalCount;

                }, function () {
                    notificationService.displayError('Có lỗi xảy ra')

                    console.log('Load products failed');
                });
        }

        $scope.getProducts();
    }
})(angular.module('tedushop.products'));