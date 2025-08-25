var notify;
var $parentObjectASense = $("#posteditrootnewpost", document);
var parentObjectSelectorASense  = "#posteditrootnewpost";
var $form1 = $("#frmPostPart1");
var $titleObject = $("#Title");
var $priceObject = $("#Price");
var $form2 = $("#frmPostPart2");
var $posterNameObject = $("#PosterName");

function isAdSpaceFormRequiredFieldsValid() {
    var isValid = true;   
    if (!validateRequiredTextData($posterNameObject, "Poster Name")) {
        isValid = false;
    }
    return isValid;
}

function updateValidation() {
    resetValidation(parentObjectSelectorASense);
    isAdSpaceFormRequiredFieldsValid();
}

function isValidForm() {
    resetValidation($parentObjectASense);
    if (isAdSpaceFormRequiredFieldsValid()) {
        return true;
    }
    return false;
}

function collectFormData() {
    var searchTag = $("#SearchTag").val();
    var title = $("#Title").val();
    var price = $("#Price").val();
    var clientName = $("#PosterName").val();   
    var websiteUrl = $("#WebsiteUrl").val();    
   
    var requestData = {
        "Title": $.trim(title),
        "Price": $.trim(price),        
        "PosterName": $.trim(clientName),
        "SearchTag": $.trim(searchTag),
        "WebsiteUrl": $.trim(websiteUrl)
    };
    return requestData;
}

function LoadImageContent1() {
    $.ajax({
        type: "GET",
        url: imgload1url,
        cache: false,
        async: true,
        success: (function (result) {
            $("#imgDiv1").html(result);
            notify.update({
                type: 'success',
                message: '<strong><i class="fa fa-thumbs-up"></i>&nbsp;'+ SUCCESS +'</strong> ' + IMAGE_LOAD_SUCCESSFUL,
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
        }),
        error: (function (e) {
        })
    });
}

function LoadImageContent2() {
    $.ajax({
        type: "GET",
        url: imgload2url,
        cache: false,
        async: true,
        success: (function (result) {
            $("#imgDiv2").html(result);
            notify.update({
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

        }),
        error: (function (e) {
        })
    });
}

function LoadImageContent3() {
    $.ajax({
        type: "GET",
        url: imgload3url,
        cache: false,
        async: true,
        success: (function (result) {
            $("#imgDiv3").html(result);
            notify.update({
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
        }),
        error: (function (e) {
        })
    });
}

function LoadImageContent4() {
    $.ajax({
        type: "GET",
        url: imgload4url,
        cache: false,
        async: true,
        success: (function (result) {
            $("#imgDiv4").html(result);
            notify.update({
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
        }),
        error: (function (e) {
        })
    });
}

$("#Upload1").ajaxUpload({
    url: imgupload1url,
    name: "file",
    data: true,
    onSubmit: function () {
        notify = $.notify('<strong><i class="fa fa-spinner"></i>&nbsp;' + LOADING +'</strong>...', {
            type: 'info'
        });
    },
    onComplete: function (result) {
        LoadImageContent1();
    }
});

$("#Upload2").ajaxUpload({
    url: imgupload2url,
    name: "file",
    data: true,
    onSubmit: function () {
        notify = $.notify('<strong><i class="fa fa-spinner"></i>&nbsp;' + LOADING +'</strong>...', {
            type: 'info'
        });
    },
    onComplete: function (result) {
        LoadImageContent2();
    }
});

$("#Upload3").ajaxUpload({
    url: imgupload3url,
    name: "file",
    data: true,
    onSubmit: function () {
        notify = $.notify('<strong><i class="fa fa-spinner"></i>&nbsp;' + LOADING +'</strong>...', {
            type: 'info'
        });
    },
    onComplete: function (result) {
        LoadImageContent3();
    }
});

$("#Upload4").ajaxUpload({
    url: imgupload4url,
    name: "file",
    data: true,
    onSubmit: function () {
        notify = $.notify('<strong><i class="fa fa-spinner"></i>&nbsp;' + LOADING +'</strong>...', {
            type: 'info'
        });
    },
    onComplete: function (result) {
        LoadImageContent4();
    }
});

$(function () {
    $(document).on("click", "#btnSubmitAdSpacePost", function () {
        notify = $.notify('<strong><i class="fa fa-spinner"></i>&nbsp;' + SAVING_DATEBASE + '</strong>...', {
            type: 'info'
        });
        
        if (isValidForm()) {
            var requestData = collectFormData();
            $.ajax({
                url: savenewposturl,
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
        updateValidation();
    });

});