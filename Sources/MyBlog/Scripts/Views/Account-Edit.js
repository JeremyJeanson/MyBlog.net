$(function () {
    $("#sendvalidationmail").click(function () {
        // Create dialog
        var dialog = $("<div class='modal fade' tabindex='-1' role='dialog'><div class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'>"
            + "<h4 class='modal-title'></h4><button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div>"
            + "<div class='modal-body'></div>"
             + "<div class='modal-footer'>"
             + "<button type='button' class='btn btn-default' data-dismiss='modal'>Ok</button>"
             + "</div>"
            + "</div></div></div>");
        // Remove dialog on close
        dialog.on("hidden.bs.modal", function () {
            dialog.remove();
        });

        // Add title
        dialog.find(".modal-title").html($(this).html());

        $.post("/Account/SendValidationMail", null,
            function (data) {
                // Load HTML view
                dialog.find(".modal-body").html(data);
                // Show
                dialog.modal('show');
            }
        );
    });
});