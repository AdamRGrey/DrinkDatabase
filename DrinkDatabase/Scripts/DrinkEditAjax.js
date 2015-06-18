function AjaxSave(event) {
    event.preventDefault();
   
    var formData = $('#editDrinkForm');
    formData.__RequestVerificationToken =  $('#editDrinkForm input[name="__RequestVerificationToken"]').val(); //there's one in the logout header. Make sure we have the *right* antiforgery token.

    console.log("we'll be sending this formData.__RequestVerificationToken: " + formData.__RequestVerificationToken);

    var x = new XMLHttpRequest();
    x = $.post(window.location, formData.serialize())
    .done(function () {
        $("#editDrinkContainer").html(x.responseText);
        $("#editableDrinkSubmitButton").after("<span id=\"temporaryText\">Changes Saved!</span>");
        var tempSpan = $("#temporaryText");
        tempSpan.fadeOut(4500, function () {
            tempSpan.remove();
        });
        $("#editableDrinkSubmitButton").click(AjaxSave);
    });
}

$(document).ready(function () {
   
    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        console.log("ajaxerror. Event: ");
        console.log(event);
        console.log("jqxhr: ");
        console.log(jqxhr);
        console.log(" settings: ");
        console.log(settings);
        console.log(" thrownError: ");
        console.log(thrownError);
    });

    $("#editableDrinkSubmitButton").click(AjaxSave);
});