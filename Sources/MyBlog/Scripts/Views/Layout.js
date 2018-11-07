function PostAndDisplay(title, url, data) {
    // Create dialog
    var dialog = $("<div class='modal fade' tabindex='-1' role='dialog'><div class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'>"
        + "<h5 class='modal-title'></h5><button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div>"
        + "<div class='modal-body'></div>"
        + "</div></div></div>");
    // Remove dialog on close
    dialog.on("hidden.bs.modal", function () {
        dialog.remove();
    });

    // Add title
    dialog.find(".modal-title").html(title);

    $.post(url, data,
        function (view) {
            // Load HTML view
            dialog.find(".modal-body").html(view);
            // Show
            dialog.modal('show');
        }
    );
}

$(function () {
    // Block UI Configuration
    $.blockUI.defaults.message =
        "<i class='fa fa-cog fa-spin fa-5x fa-fw'></i>"
        + "<i class='fa fa-cog fa-spin2 fa-5x fa-fw' style='margin:-37px;'></i>"
        + "<i class='fa fa-cog fa-spin fa-5x fa-fw'></i>";
    $.blockUI.defaults.baseZ = 9999;
    $.blockUI.defaults.css.border = "none";
    $.blockUI.defaults.css.height = 0;
    $.blockUI.defaults.overlayCSS.cursor = "wait";
    $.blockUI.defaults.overlayCSS.backgroundColor = "rgba(0, 0, 0, 0.2)";

    // utilisation du block Ui quand Ajax est utilisé
    $(document).ajaxStop($.unblockUI);
    $(document).ajaxStart($.blockUI);

    // Header scroll
    window.addEventListener('scroll', function (e) {
        var distanceY = window.pageYOffset || document.documentElement.scrollTop;

        if (distanceY > 300) {
            if ($("body").hasClass("bigheader")) {
                $("body").removeClass("bigheader");
            }
        } else {
            $("body").addClass("bigheader");
        }
    });

    // Login
    $(".login").click(function () {
        PostAndDisplay(
            $(this).html(),
            "/Account/_Login",
            { "url": window.location.href }
        );
    });
});