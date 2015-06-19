function AjaxSave(event) {
    if(event != null)
        event.preventDefault();
   
    var formData = $('#editDrinkForm');
    formData.__RequestVerificationToken =  $('#editDrinkForm input[name="__RequestVerificationToken"]').val(); //there's one in the logout header. Make sure we have the *right* antiforgery token.
    
    var x = new XMLHttpRequest();
    x = $.post(window.location, formData.serialize())
    .done(function () {
        $("#editDrinkContainer").html(x.responseText);
        $("#editableDrinkSubmitButton").after("<span id=\"temporaryText\">Changes Saved!</span>");
        var tempSpan = $("#temporaryText");
        tempSpan.fadeOut(4500, function () {
            tempSpan.remove();
        });

        clearTimeout(TimerID);
        eventSetup();
    });
}

function eventSetup()
{
    $("#editableDrinkSubmitButton").off("click.DrinkDatabase");
    $("#editableDrinkSubmitButton").on("click.DrinkDatabase", AjaxSave);

    $(".autosave-delayed").off("input.DrinkDatabase");
    $(".autosave-delayed").on("input.DrinkDatabase", startTimeout);

    $(".autosave-delayed-instant").off("change.DrinkDatabase");
    $(".autosave-delayed-instant").on("change.DrinkDatabase", AjaxSave);
}

var TimerID = 0;
function startTimeout()
{
    console.log("restarting timeout, current TimerID = " + TimerID);
    clearTimeout(TimerID);
    TimerID = setTimeout(AjaxSave, 1000, null);
}

$(document).ready(function ()
{
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

    eventSetup();
});