$(function () {
    $("#CurrentUserSubscibed").change(function () {
        var title = $("label[for='CurrentUserSubscibed']").html();
        var id = $("#Post_Id").val();
        var subscription = $(this).is(":checked");
        PostAndDisplay(
            title,
            "/Post/SubscribToCommentNotification",
            { id : id, subscription : subscription });
    });
});