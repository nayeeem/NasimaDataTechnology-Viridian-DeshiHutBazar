$(function () {
    var $rootObject = $("#SigninRoot", document);
    var signinSubmitSelector = "#btnSignin";
    var $emailObject = $("#Email", $rootObject);
    var $passwordObject = $("#Password", $rootObject);

    //function processUIAfterUserLogin(userid) {
    //    var data = { "UserID": userid };
    //    $.ajax({
    //        url: loginUrl,
    //        type: 'POST',
    //        data: data,
    //        cache: true
    //    }).done(function (user) {
    //        if (user != -1) {
    //            setUserCookei(user.UserID, user.ClientName, user.Email);
    //            window.location = homeurl;
    //        }
    //    });
    //}

    function login(user, pass) {
        var data = { "Email": user, "Password": pass };
        $.ajax({
            url: authenticateurl,
            type: 'POST',
            data: data,
            cache: true
        }).done(function (userid) {
            if (userid != -1) {
                window.location = homeurl;
            } else {
                addCustomErrorMessage($emailObject, "");
                addCustomErrorMessage($passwordObject, EMAIL_OR_PASSWORD_INCORRECT);
            }
        });
    }

    function loginAdmin(user, pass) {
        var data = { "Email": user, "Password": pass };
        $.ajax({
            url: loginadminurl,
            type: 'POST',
            data: data,
            cache: true
        }).done(function (result) {
            if (result == "GeneralUser") {
                var userID = login(user, pass);
            } else {
                window.location = result;
            }

        });
    }

    $rootObject.on("click", signinSubmitSelector, function () {
        var isValid = true;
        resetValidation($rootObject);
        if (!validateRequiredTextData($emailObject, "Email")) {
            isValid = false;
        }
        if (!validateRequiredTextData($passwordObject, "Password")) {
            isValid = false;
        }
        if (isValid) {
            var email = $emailObject.val();
            var pass = $passwordObject.val();
            loginAdmin(email, pass);
        }
    });
});