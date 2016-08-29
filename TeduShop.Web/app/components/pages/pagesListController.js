(function (app) {

    app.controller('pagesListController', pagesListController);
    pagesListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];//ngBootbox la thu vien su dung de tao box thong bao

    function pagesListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.pages = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getPages = getPages;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deletePage = deletePage;
        $scope.isAll = false;
        $scope.deleteMulti = deleteMulti;
        function deleteMulti() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            })

            var config = {

                params: {
                    listProductCategories: JSON.stringify(listId)
                }
            }
            apiService.del('api/page/deletemulti', config, function (result) {
                notificationService.displaySuccess('Đã xóa thành công ' + $scope.selected.length + ' mục');
                search();

            }, function () {
                notificationService.displayError('Xóa thất bại');
            })
        }




        $scope.CheckAll = CheckAll;
        function CheckAll() {
            if ($scope.isAll == false) {
                angular.forEach($scope.pages, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.pages, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }


        
        $scope.$watch("pages", function (n, o) {// enable nut xoa nhieu neu co checked

            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);



        function deletePage(id) {

            $ngBootbox.confirm('Bạn có chắc muốn xóa ko ?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/page/delete', config, function (result) {

                    notificationService.displaySuccess('Xóa thành công ' + result.data.Name);
                    search();
                }, function () {

                    notificationService.displayError('Xoá thất bại ');
                })
            });
        }




        function search() {
            getPages();


        }

        function getPages(page) {
            page = page || 0;

            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 15//thay doi so item lay ra
                }
            }



            apiService.get('api/page/getall',
                config,
                function (result) {
                    if (result.data.TotalCount == 0) {
                        notificationService.displayWarning('Không có bản ghi nào được tìm thấy !')
                    } else {
                        notificationService.displaySuccess('Có ' + result.data.TotalCount + ' bản ghi được tìm thấy !')

                    }


                    $scope.pages = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPage;
                    $scope.totalCount = result.data.TotalCount;

                }, function () {
                    notificationService.displayError('Có lỗi xảy ra')

                    console.log('Load pages failed');
                });
        }

        $scope.getPages();
    }
})(angular.module('tedushop.pages'));