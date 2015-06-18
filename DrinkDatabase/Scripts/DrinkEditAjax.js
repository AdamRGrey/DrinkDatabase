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

    $("#editableDrinkSubmitButton").click(function (event) {
        event.preventDefault();
   
        var formData = $('#editDrinkForm');
        formData.__RequestVerificationToken =  $('#editDrinkForm input[name="__RequestVerificationToken"]').val(); //there's one in the logout header. Make sure we have the *right* antiforgery token.

        var x = new XMLHttpRequest();
        x = $.post(window.location, formData.serialize())
        .done(function () {
            $("#editDrinkContainer").replaceWith($.parseHTML(x.responseText));
        })

    })
});