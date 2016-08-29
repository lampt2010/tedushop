var common = {
    init: function () {//tat ca nhung gi tu dong chay viet vao day
        common.registerEvent();
    },

    registerEvent: function () {
        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, respone) {
                $.ajax({
                    url: "/Home/GetListSearch",
                    dataType: "json",
                    data: { keyword: request.term },
                    success: function (result) {
                        respone(result.data);
                    }
                })
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            }
        })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<div>" + item.label  + "</div>")
          .appendTo(ul);
    };
    }




}
common.init();