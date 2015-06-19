go();

function go()
{
    var usedOptions = {};
    var selects = $("select");

    //first pass: get used options
    selects.each(function (i) {
        var thisSelect = $(selects[i]);
        var options = thisSelect.children("option");

        options.each(function (j) {
            var thisOption = $(options[j]);
            if (thisOption.attr("selected"))
            {
                usedOptions[thisOption.attr("value")] = thisOption.text();
            }
        });
        thisSelect.unbind("change.DrinkDatabase_RedundantIngredientRemover")
        thisSelect.bind("change.DrinkDatabase_RedundantIngredientRemover", go);
    });

    //second pass: remove redundant ones
    selects.each(function (i)
    {
        var thisSelect = $(selects[i]);
        var options = thisSelect.children("option");
        options.each(function (j)
        {
            var thisOption = $(options[j]);
            if(usedOptions.propertyIsEnumerable(thisOption.val()) && !thisOption.attr("selected"))
            {
                thisOption.hide();
            }
            else
            {
                thisOption.show();
            }
        });
    });
}