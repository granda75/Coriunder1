function Validate()
{
    var isSuccess;
    isSuccess = ValidateEmail();
    if (!isSuccess)
        return false;

    isSuccess = ValidatePhone();
    if (!isSuccess)
        return false;

    return true;
}
function ValidateEmail()
{
    var email = $('#MainContent_txtEmail').val();

    if (! /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))
    {
        alert("You have entered an invalid email address!");
        return false;
    }
    return true;
}

function ValidatePhone()
{
    //debugger;
    var phone = $('#MainContent_txtPhone').val();
    var isNumeric = $.isNumeric(phone);
    if (!isNumeric)
    {
        alert("You have entered an invalid phone number!");
        return false;
    }
    return true;
}
