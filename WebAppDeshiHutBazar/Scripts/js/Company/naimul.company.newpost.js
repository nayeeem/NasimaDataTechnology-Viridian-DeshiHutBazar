var notify;
function FailedMessage() {
    notify = $.notify('<strong>&nbsp;' + 'File upload failed!' + '</strong>', {
        type: 'danger'
    });
}

function NotifySuccessMessage() {
    notify = $.notify({
        type: 'success',
        message: '<strong><i class="fa fa-thumbs-up"></i>&nbsp;' + SUCCESS + '</strong> ' + IMAGE_LOAD_SUCCESSFUL,
        allow_dismiss: true,
        placement: {
            from: "top",
            align: "right"
        },
        delay: { show: 80, hide: 50 },
        template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
            '<span data-notify="icon"></span>' +
            '<span data-notify="title">{1}</span>' +
            '<span data-notify="message">{2}</span>' +
            '</div>'
    });
}

function LoadCarousel() {
    $.ajax({
        type: "GET",
        url: loadCarouselURL,
        cache: false,
        async: true,
        success: function (result) {
            $("#imagePanelContainer").append(result);
            NotifySuccessMessage();
        },
        error: function (e) {
            FailedMessage();
        }
    });
}

function LoadThumbnail() {
    $.ajax({
        type: "GET",
        url: loadThumbnailURL,
        cache: false,
        async: true,
        success: function (result) {
            $("#divThumbnail").append(result);
            NotifySuccessMessage();
        },
        error: function (e) {
            FailedMessage();
        }
    });
}

function LoadSquare() {
    $.ajax({
        type: "GET",
        url: loadSquareURL,
        cache: false,
        async: true,
        success: function (result) {
            $("#divSquare").append(result);
            NotifySuccessMessage();
        },
        error: function (e) {
            FailedMessage();
        }
    });
}

function LoadRectangle() {
    $.ajax({
        type: "GET",
        url: loadRectangleURL,
        cache: false,
        async: true,
        success: function (result) {
            $("#divRectangle").append(result);
            NotifySuccessMessage();
        },
        error: function (e) {
            FailedMessage();
        }
    });
}

$("#UploadRectangle").ajaxUpload({
    url: uploadRectangleURL,
    name: "file",
    data: true,
    onSubmit: function () {
    },
    onComplete: function (result) {
        if (result != 'false') {
            LoadRectangle();
        }
        else {
            FailedMessage();
        }
    }
});

$("#UploadSquare").ajaxUpload({
    url: uploadSquareURL,
    name: "file",
    data: true,
    onSubmit: function () {
    },
    onComplete: function (result) {
        if (result != 'false') {
            LoadSquare();
        }
        else {
            FailedMessage();
        }
    }
});

$("#UploadThumbnail").ajaxUpload({
    url: uploadThumbnailURL,
    name: "file",
    data: true,
    onSubmit: function () {
    },
    onComplete: function (result) {
        if (result != 'false') {
            LoadThumbnail();
        }
        else {
            FailedMessage();
        }
    }
});

$("#UploadCarousel").ajaxUpload({
    url: uploadCarouselURL,
    name: "file",
    data: true,
    onSubmit: function () {       
    },
    onComplete: function (result) {
        if (result != 'false') {
            LoadCarousel();
        }
        else {
            FailedMessage();
        }
    }
});

function PopulateSubCategory(items) {
    $('#SubCategoryID').empty();
    $.each(items, function (i, item) {
        if (item.Text != null) {
            $('#SubCategoryID').append($('<option>', {
                value: item.Value,
                text: item.Text
            }));
        } else {
            $('#SubCategoryID').append($('<option>', {
                value: "",
                text: ""
            }));
        }
    });
}

function ResetSubCategory(category) {
    $("#SubCategoryID").val("");
    $("#spanCategoryDropdown").text("");
    if (category == "" || category == null || category == undefined) {
        $('#SubCategoryID').empty();
        return false;
    }
    return true;
}

$(function () {
    $(document).on("change", "#CategoryID", function () {
        var category = $(this).val();        
        if (ResetSubCategory(category)) {
            var requestData = { "categoryValueID": $.trim(category) };
            $.ajax({
                url: getSubCategoryURL,
                type: 'POST',
                data: JSON.stringify(requestData),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                error: function (xhr) {
                },
                success: function (items) {
                    PopulateSubCategory(items);
                },
                processData: false,
                cache: false
            });
        }
    });

    $(document).on("click", ".btn-remove-image", function () {
        var that = this;
        var id = $(that).data("id");
        var rem = $(that).closest(".image-content");
        $(rem).remove();
        $.ajax({
            url: removeProductImageURL + id,
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
            },
            success: function (categorytext) {
            },
            processData: false,
            cache: false
        });
    });
});