var notify;
var $parentObject = $("#posteditrootnewpostNP", document);
var parentObjectSelector = "#posteditrootnewpostNP";
var $form1 = $("#frmPostPart1");
var $titleObject = $("#Title");
var $categoryObject = $("#CategoryID");
var $subCategoryObject = $("#SubCategoryID");
var $priceObject = $("#Price");
var $form2 = $("#frmPostPart2");
var $emailObject = $("#Email");
var $clientNameObject = $("#ClientName");
var $passwordObject = $("#Password");
var $repasswordObject = $("#RePassword");
var $phoneObject = $("#Phone");

function isFormPasswordValid() {
    if (!$("#hSignin").is(":visible"))
        return isPasswordRulesValid($passwordObject, $repasswordObject);
    else {
        return true;
    }
}

function isFormRequiredFieldsValid() {
    var isValid = true;
    if (!validateMaxLengthTextData($titleObject, 80, TitleField)) {
        isValid = false;
    }
    if (!validateRequiredTextData($titleObject, TitleField)) {
        isValid = false;
    }
    if (!validateRequiredTextData($categoryObject, CategoryField)) {
        isValid = false;
    }   
    if (!validateRequiredTextData($subCategoryObject, SubCategoryField)) {
        isValid = false;
    }  
    if (!validateRequiredTextData($priceObject, PriceField)) {
        isValid = false;
    }
    if (!validateRequiredTextData($clientNameObject, ClientNameField)) {
        isValid = false;
    }
    if (!validateRequiredTextData($phoneObject, PhoneField)) {
        isValid = false;
    }
    if (!validateRequiredTextData($emailObject, EmailField)) {
        isValid = false;
    }
    if (!validateRequiredTextData($passwordObject, PasswordField)) {
        isValid = false;
    }
    if (!$("#hSignin").is(":visible")) {
        if (!validateRequiredTextData($repasswordObject, RePasswordField)) {
            isValid = false;
        }
        if (!isTermsChecked()) {
            $("#chkTerms").parent().parent().append("<span class='custom-inline-field-error' style='margin-bottom:10px;margin-top:25px;'>You have to check Terms checkbox.</span>");
            isValid = false;
        }
    }
    return isValid;
}

function isTermsChecked() {
    if ($('#chkTerms').is(':checked'))
        return true;
    else
        return false;
}

function updateValidation() {
    resetValidation(parentObjectSelector);
    isFormRequiredFieldsValid();
    isFormPasswordValid();
}

function isValidForm() {
    resetValidation($parentObject);
    if (isFormRequiredFieldsValid() && isFormPasswordValid()) {
        return true;
    }
    return false;
}

function collectFormData() {
    var searchTag = $("#SearchTag").val();
    var title = $("#Title").val();
    var categoryId = $("#CategoryID").val();
    var subCategoryId = $("#SubCategoryID").val();
    var description = $("#Description").val();
    var price = $("#Price").val();
    var email = $("#Email").val();
    var clientName = $("#ClientName").val();
    var phone = $("#Phone").val();
    var password = $("#Password").val();
    var rePassword = $("#RePassword").val();
    var state = $("#StateID").val();
    var areadesc = $("#AreaDescription").val();
    var forSell = $("input[id='IsForSell1']:checked").val();
    var privateSeller = $("input[id='IsPrivateSeller1']:checked").val();
    var urgentDeal = $("input[id='IsUrgent1']:checked").val();
    var newItem = $("input[id='IsBrandNew1']:checked").val();

    var requestData = {
        "Title": $.trim(title),
        "CategoryID": $.trim(categoryId),
        "SubCategoryID": $.trim(subCategoryId),
        "Description": $.trim(description),
        "Price": $.trim(price),
        "Email": $.trim(email),
        "ClientName": $.trim(clientName),
        "Phone": $.trim(phone),
        "Password": $.trim(password),
        "RePassword": $.trim(rePassword),
        "StateID": $.trim(state),
        "AreaDescription": $.trim(areadesc),
        "IsBrandNew": newItem === "True",
        "IsUsed": newItem !== "True",
        "IsUrgent": urgentDeal === "True",
        "IsPrivateSeller": privateSeller === "True",
        "IsCompanySeller": privateSeller !== "True",
        "IsForSell": forSell === "True",
        "IsForRent": forSell !== "True",
        "SearchTag": $.trim(searchTag)
    };
    return requestData;
}

function populateSubCategory(items) {
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

function LoadImageContentNP1() {
    $.ajax({
        type: "GET",
        url: imgload1url,
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
    url: imgupload1url,
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

function resetSubCategory(category) {
    $("#SubCategoryID").val("");
    $("#spanCategoryDropdown").text("");
    if (category == "" || category == null || category == undefined) {
        $('#SubCategoryID').empty();
        return false;
    }
    return true;
}

function resetForSellRentSection(categoryid) {
    if (categoryid != "") {
        var requestData = { "CategoryID": $.trim(categoryid) };
        $.ajax({
            url: getcategorytexturl, 
            type: 'POST',
            data: JSON.stringify(requestData),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
            },
            success: function (categorytext) {
                if (categorytext == "Vehicles" || categorytext == "Real Estate") {
                    $("#divSellRent").show();
                } else {
                    $("#divSellRent").hide();
                }
            },
            processData: false,
            cache: false
        });
    } else {
        $("#divSellRent").hide();
    }
}

$(document).on("change", "#CategoryID", function () {
    var category = $(this).val();
    resetForSellRentSection(category);
    if (resetSubCategory(category)) {
        var requestData = { "categoryValueID": $.trim(category) };
        $.ajax({
            url: getsubcategoryurl, 
            type: 'POST',
            data: JSON.stringify(requestData),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
            },
            success: function (items) {
                populateSubCategory(items);
            },
            processData: false,
            cache: false
        });
    }
});

$(function () {
    $(document).on("click", "#btnSubmitPost", function () {
        notify = $.notify('<strong><i class="fa fa-spinner"></i>&nbsp;' + SAVING_DATEBASE + '</strong>...', {
            type: 'info'
        });
        var isValid = isValidEmailAddress($("#frmPostPart3").find("#Email").val());
        if (!isValid) {
            resetValidation($parentObject);
            addCustomErrorMessage($("#frmPostPart3").find("#Email"), EMAIL_NOT_IN_CORRECT_FORMAT);
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
            return false;
        }
        if (isValidForm() && isValid) {
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
                    if (result == "EmailInvalid") {
                        addCustomErrorMessage($("#frmPostPart3").find("#Email"), EMAIL_NOT_IN_CORRECT_FORMAT);
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
                    else if (result == "PhoneInvalid") {
                        addCustomErrorMessage($phoneObject, PHONE_IS_NOT_IN_CORRECT_FORMAT);
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
                    else if (result == "AuthenticationFailed") {
                        notify.update({
                            type: 'danger',
                            message: '<strong>' + 'Authentication Failed!' + '</strong>&nbsp;' + POST_NOT_SAVED +
                                'Please enter valid credentials.',
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
                    } else {
                        notify.update({
                            type: 'success',
                            message: '<strong><i class="fa fa-thumbs-up"></i>&nbsp;' + SUCCESS + '</strong> ' + YOUR_POST_HAVE_BEEN_SUBMITTED,
                            allow_dismiss: true,
                            placement: {
                                from: "top",
                                align: "right"
                            },
                            delay: { show: 900, hide: 100 },
                            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                                '<span data-notify="icon"></span>' +
                                '<span data-notify="title">{1}</span>' +
                                '<span data-notify="message">{2}</span>' +
                                '</div>'
                        });

                        notify.update({
                            type: 'success',
                            message: '<strong><i class="fa fa-thumbs-up"></i>&nbsp;' + '' + '</strong> ' + 'Please Wait...',
                            allow_dismiss: true,
                            placement: {
                                from: "top",
                                align: "right"
                            },
                            delay: { show: 900, hide: 2000 },
                            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                                '<span data-notify="icon"></span>' +
                                '<span data-notify="title">{1}</span>' +
                                '<span data-notify="message">{2}</span>' +
                                '</div>'
                        });
                        window.location = result.Url;
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

    $(document).on("change", "#posteditrootnewpostNP", function () {
        updateValidation();
    });

    $(document).on("change", "#Email", function () {
        var $passobject = $("#rePasswordDiv");
        resetValidation($("#posteditrootnewpostNP"));
        var email = $(this).val();
        var requestData = {
            "Email": $.trim(email),
        };
        $.ajax({
            url: checkemailexistsnewpost, //Server script to process data
            type: 'POST',
            data: JSON.stringify(requestData),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result == "EmailInvalid") {
                    resetValidation($("#rootContact"));
                    addCustomErrorMessage($("#Email"), PLEASE_ENTER_VALID_EMAIL_ADDRESS);
                }
                if (result == "EmailNotFound") {
                    $("#hSignup").show();
                    $("#divTerms").show();
                    $("#hSignin").hide();
                    $passobject.show();
                    return false;
                }
                if (result == "EmailFound") {
                    $("#hSignup").hide();
                    $("#divTerms").hide();
                    $("#hSignin").show();
                    $passobject.hide();
                    updateValidation();
                    return false;
                }
            }
        });
    });

    $(document).on("click", ".btn-remove-image", function () {
        var that = this;
        var id = $(that).data("id");
        var rem = $(that).closest(".image-content");
        $(rem).remove();
        $.ajax({
            url: removeImageURL + id,
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