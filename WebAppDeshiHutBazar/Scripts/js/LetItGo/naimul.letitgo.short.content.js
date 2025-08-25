var notify;
var $parentObjectASense = $("#posteditrootnewpost", document);
var parentObjectSelectorASense  = "#posteditrootnewpost";
var $titleObject = $("#Title");
function isContentFormRequiredFieldsValid() {
    var isValid = true;   
    if (!validateRequiredTextData($("#PosterName"), "Poster Name")) {
        isValid = false;
    }
    if (!validateRequiredTextData($("#Description"), "Description")) {
        isValid = false;
    }
    if (!validateRequiredTextData($("#Title"), "Title")) {
        isValid = false;
    }
    return isValid;
}

function updateShortContentValidation() {
    resetValidation(parentObjectSelectorASense);
    isContentFormRequiredFieldsValid();
}

function isValidContentForm() {
    resetValidation($parentObjectASense);
    if (isContentFormRequiredFieldsValid()) {
        return true;
    }
    return false;
}

function collectContentFormData() {
    var searchTag = $("#SearchTag").val();
    var title = $("#Title").val();
    var price = $("#Price").val();
    var clientName = $("#PosterName").val();   
    var websiteUrl = $("#WebsiteUrl").val();    
    var description = $("#Description").val(); 
    var postTypeID = $("#PostTypeID").val();
    var requestData = {
        "Title": $.trim(title),
        "Price": $.trim(price),        
        "PosterName": $.trim(clientName),
        "SearchTag": $.trim(searchTag),
        "WebsiteUrl": $.trim(websiteUrl),
        "Description": $.trim(description),
        "PostTypeID": $.trim(postTypeID)
    };
    return requestData;
}

function LoadImageContentNP1() {
    $.ajax({
        type: "GET",
        url: imageLoadURL,
        cache: false,
        async: true,
        success: function (result) {
            $("#imagePanelContainer").append(result);
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
        },
        error: function (e) {
        }
    });
}

$("#UploadNP1").ajaxUpload({
    url: imageUploadURL,
    name: "file",
    data: true,
    onSubmit: function () {
    },
    onComplete: function (result) {
        if (result != 'false') {
            LoadImageContentNP1();
            $("#infoErrorUpload1").hide();
        }
        else {
            $("#infoErrorUpload1").show();
            notify = $.notify('<strong>&nbsp;' + 'File upload failed!' + '</strong>', {
                type: 'danger'
            });
        }
    }
});

$(function () {
    $(document).on("click", ".btn-remove-image", function () {
        var that = this;
        var id = $(that).data("id");
        var rem = $(that).closest(".image-content");
        $(rem).remove();
        $.ajax({
            url: removeImageShrtContentURL + id,
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

    $(document).on("click", "#btnSubmitShortContentPost", function () {
        notify = $.notify('<strong><i class="fa fa-spinner"></i>&nbsp;' + SAVING_DATEBASE + '</strong>...', {
            type: 'info'
        });        
        if (isValidContentForm()) {
            var requestData = collectContentFormData();
            $.ajax({
                url: saveNewShortContentURL,
                type: 'POST',
                data: JSON.stringify(requestData),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                error: function (xhr) {
                },
                success: function (result) {
                    if (result == "TitleInvalid") {
                        addCustomErrorMessage($("#Title"), TITLE_IS_REQUIRED);
                        notify.update({
                            type: 'danger',
                            message: '<strong>' + VALIDATION_FAILED + ' </strong>&nbsp;' + POST_NOT_SAVED +
                                PLEASE_ENTER_TEXT_FOR_INPUT_FIELDS,
                            allow_dismiss: true,
                            placement: {
                                from: "top",
                                align: "right"
                            },
                            delay: { show: 500, hide: 100 },
                            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                                '<span data-notify="icon"></span>' +
                                '<span data-notify="title">{1}</span>' +
                                '<span data-notify="message">{2}</span>' +
                                '</div>'
                        });
                    }
                    else if (result == "PosterNameInvalid") {
                        addCustomErrorMessage($titleObject, CLIENT_NAME_REQUIRED);
                        notify.update({
                            type: 'danger',
                            message: '<strong>' + VALIDATION_FAILED + '</strong>&nbsp;' + POST_NOT_SAVED +
                                PLEASE_ENTER_TEXT_FOR_INPUT_FIELDS,
                            allow_dismiss: true,
                            placement: {
                                from: "top",
                                align: "right"
                            },
                            delay: { show: 500, hide: 100 },
                            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                                '<span data-notify="icon"></span>' +
                                '<span data-notify="title">{1}</span>' +
                                '<span data-notify="message">{2}</span>' +
                                '</div>'
                        });
                    }
                    else {
                        notify.update({
                            type: 'success',
                            message: '<strong><i class="fa fa-thumbs-up"></i>&nbsp;' + SUCCESS + '</strong> ' + YOUR_POST_HAVE_BEEN_SUBMITTED,
                            allow_dismiss: true,
                            placement: {
                                from: "top",
                                align: "right"
                            },
                            delay: { show: 500, hide: 100 },
                            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                                '<span data-notify="icon"></span>' +
                                '<span data-notify="title">{1}</span>' +
                                '<span data-notify="message">{2}</span>' +
                                '</div>'
                        });                       
                    }
                },
                processData: false
            });
        } else {
            notify.update({
                type: 'danger',
                message: '<strong>' + VALIDATION_FAILED + '</strong>&nbsp;' + POST_NOT_SAVED +
                    PLEASE_ENTER_TEXT_FOR_INPUT_FIELDS,
                allow_dismiss: true,
                placement: {
                    from: "top",
                    align: "right"
                },
                delay: { show: 500, hide: 100 },
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span>' +
                    '<span data-notify="title">{1}</span>' +
                    '<span data-notify="message">{2}</span>' +
                    '</div>'
            });
        }
    });

    $(document).on("change", "#posteditrootnewpost", function () {
        updateShortContentValidation();
    });
});