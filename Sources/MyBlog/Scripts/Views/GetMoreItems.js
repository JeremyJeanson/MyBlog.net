$(function () {
    $("#btn-getmore").click(function () {
        $.post(
            "/Post/" + $("#btn-getmore").data("action") + "GetMore/" + $("#btn-getmore").data("args"),
            function (data) {
                $("#items").append(data);
            })
    });
});