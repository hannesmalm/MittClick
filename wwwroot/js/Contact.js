
//Hämtar valet från radioknapparna
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

function deleteContactInfo(itemId) {
    // Bekräfta att användaren verkligen vill ta bort utbildningen.
    var confirmDelete = confirm("Är du säker på att du vill ta bort kontaktinformationen?");
    if (confirmDelete) {
        // Skicka en AJAX-förfrågan för att ta bort utbildningen.
        $.ajax({
            type: "POST",
            url: '/Profile/DeleteContactInfo/' + itemId,
            success: function () {
                // Ta bort det relevanta elementet från DOM.
                $('#contact-' + itemId).remove();

                // Ladda om sidan för att uppdatera CV:et.
                location.reload();
            },
            error: function () {
                alert("Något gick fel vid borttagning.");
            }
        });
    }
}