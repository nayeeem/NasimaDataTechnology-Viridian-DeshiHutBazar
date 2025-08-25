
function isStringWithRightLength(str, length) {
    if (!isValidData(str))
        return false;
    if (!isValidData(length))
        return false;
    if (str.length >= length) {
        return true;
    }
    return false;
}

function isSameLength(password, repassword) {
    if (isValidData(password) && isValidData(repassword)) {
        if (password.length == repassword.length)
            return true;
    }
    return false;
}

function isSameLiteral(password, repassword) {
    if (isValidData(password) && isValidData(repassword)) {
        if (password == repassword)
            return true;
    }
    return false;
}

function getPasswordErrorMessage(type) {
    if (type == LENGTH_RULE) {
        return "<span class='custom-inline-field-error'>" + PASSWORD_MINIMUM_HAS_TO_BE + "</span>";
    }
    if (type == SAME_LENGTH_RULE) {
        return "<span class='custom-inline-field-error'>" + PASSWORD_AND_REPASSWORD_HAS_TO_BE + "</span>";
    }
    if (type == LITERAL_COMPARE_RULE) {
        return "<span class='custom-inline-field-error'>" + PASSWORD_AND_REPASSWORD_HAS_TO_BE_SAME + "</span>";
    }
}

function isPasswordWithRightLength(password, length) {
    return isStringWithRightLength(password, length);
}

function isPasswordRulesValid(password, repassword) {
    var isValid = true;
    if (!isPasswordWithRightLength($(password).val(), 7)) {
        $(password).parent().append(getPasswordErrorMessage(LENGTH_RULE));
        isValid = false;
    }
    if (!isPasswordWithRightLength($(repassword).val(), 7)) {
        $(repassword).parent().append(getPasswordErrorMessage(LENGTH_RULE));
        isValid = false;
    }
    if (!isSameLength($(password).val(), $(repassword).val())) {
        $(password).parent().append(getPasswordErrorMessage(SAME_LENGTH_RULE));
        $(repassword).parent().append(getPasswordErrorMessage(SAME_LENGTH_RULE));
        isValid = false;
    }
    if (!isSameLiteral($(password).val(), $(repassword).val())) {
        $(password).parent().append(getPasswordErrorMessage(LITERAL_COMPARE_RULE));
        $(repassword).parent().append(getPasswordErrorMessage(LITERAL_COMPARE_RULE));
        isValid = false;
    }
    return isValid;
}


