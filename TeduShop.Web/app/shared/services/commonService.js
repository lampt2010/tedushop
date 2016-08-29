/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.factory('commonService', commonService);// khai bao sevice
    commonService.$inject = ['$http', 'notificationService'];//notificationservice chi la thu vien giup hien thi thong bao
    function commonService($http, notificationService) {
        return {
            getSeoTitle: getSeoTitle
        }
        function getSeoTitle(input) {
            if (input == undefined || input == '') {
                return '';
            }
            //Đổi chữ hoa thành chữ thường
            var slug = input.toLowerCase();
            // Đổi ký tự có dấu thành không dấu

            slug = slug.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/gi,'a');
            slug = slug.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/gi, 'e');
            slug = slug.replace(/i|ì|í|ị|ỉ|ĩ/gi, "i");
            slug = slug.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/gi, 'o');
            slug = slug.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/gi, 'u');
            slug = slug.replace(/ỳ|ý|ỵ|ỷ|ỹ/gi, 'y');
            slug = slug.replace(/đ/gi, 'd');


            //Xóa các ký tự đặc biệt
            slug = slug.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'||\"|\&|\#|\[|\]|~|$|_/gi, '');
            //đổi khoảng trắng thành ký tự gạch ngang
            slug = slug.replace(/ /g, "-");
            //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
            //phòng trường hợp người dùng nhập vào quá nhiều ký tự trắng
            slug = slug.replace(/\-\-\-\-\-/gi, '-');
            slug = slug.replace(/\-\-\-\-/gi, '-');
            slug = slug.replace(/\-\-\-/gi, '-');
            //Xóa các ký tự gạch ngang ở đầu và cuối
            slug = '@' + slug + '@';
            slug = slug.replace(/\@\-|\-\@|\@/gi, '');
            return slug;








        }
        

    }



})(angular.module('tedushop.common'));