function getSelectedContactMethod() {
    var radios = document.getElementsByName('btnradio');
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            return radios[i].getAttribute('data-value');
        }
    }
}


function addContactInfo(newContactInfo) {


    var newContactType = getSelectedContactMethod();

    console.log("SKRIPT: " + "info: " + newContactInfo + "typ: " + newContactType)



    $.ajax({
        url: '/Profile/AddContactInfo',
        type: 'POST',
        data: { newContactType: newContactType, newContactInfo: newContactInfo },
        success: function (response) {
            // Hantera framgångsrikt svar
            window.location.reload(); // Ladda om sidan
        },
        error: function (xhr, status, error) {
            if (xhr.status === 400) {
                alert(xhr.responseText); // Visa felmeddelande för dubbletter
            } else {
                console.error("Error: " + error);
            }
        }
    });
}