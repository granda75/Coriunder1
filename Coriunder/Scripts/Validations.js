function Validate()
{
    var isSuccess;
    isSuccess = ValidateEmail();
    if (!isSuccess)
        return false;

    isSuccess = ValidatePhone();
    if (!isSuccess)
        return false;

    isSuccess = ValidateCardNumber();
    if (!isSuccess)
        return false;

    isSuccess = ValidateCvv();
    if (!isSuccess)
        return false;

    isSuccess = ValidateZipCode();
    if (!isSuccess)
        return false;

    return true;
}

function ValidateZipCode() {
    var zipCode = $('#MainContent_txtZipCode').val();
    var isNumeric = $.isNumeric(zipCode);
    if (!isNumeric)
    {
        $('#MainContent_txtZipCode').focus();
        alert("You have entered an invalid Zip code!");
        return false;
    }
    return true;
}

function ValidateCvv() {
    var cvv = $('#MainContent_txtCvv').val();
    var isNumeric = $.isNumeric(cvv);
    if (!isNumeric)
    {
        $('#MainContent_txtCvv').focus();
        alert("You have entered an invalid CVV number!");
        return false;
    }
    return true;
}

function ValidateCardNumber()
{
    var cardNumber = $('#MainContent_txtCardNumber').val();
    var isNumeric = $.isNumeric(cardNumber);
    if (!isNumeric)
    {
        $('#MainContent_txtCardNumber').focus();
        alert("You have entered an invalid card number!");
        return false;
    }
    return true;
}

function ValidateEmail()
{
    var email = $('#MainContent_txtEmail').val();

    if (! /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))
    {
        $('#MainContent_txtEmail').focus();
        alert("You have entered an invalid email address!");
        return false;
    }
    return true;
}

function ValidatePhone()
{
    var phone = $('#MainContent_txtPhone').val();
    var isNumeric = $.isNumeric(phone);
    if (!isNumeric)
    {
        $('#MainContent_txtPhone').focus();
        alert("You have entered an invalid phone number!");
        return false;
    }
    return true;
}
