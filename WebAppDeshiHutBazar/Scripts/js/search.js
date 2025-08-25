function resetSubCategory(category) {
    $("select[name='SubCategoryID'").val("");
    if (category == "" || category == null || category == undefined) {
        $("select[name='SubCategoryID'").empty();
        return false;
    }
    return true;
}

function populateSubCategory(items) {
    $("select[name='SubCategoryID'").empty();
    $.each(items, function (i, item) {
        if (item.Text != null) {
            $("select[name='SubCategoryID'").append($('<option>', {
                value: item.Value,
                text: item.Text
            }));
        } else {
            $("select[name='SubCategoryID'").append($('<option>', {
                value: "",
                text: ""
            }));
        }
    });
}

$(document).on("change", "select[name='CategoryID']", function () {
    var category = $(this).val();    
    if (resetSubCategory(category)) {
        var requestData = { "categoryValueID": $.trim(category) };
        $.ajax({
            url: getsubcategoryurlsearch,
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
