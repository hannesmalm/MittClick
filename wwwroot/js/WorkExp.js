document.addEventListener('DOMContentLoaded', function () {
    var addWorkButton = document.getElementById('addWorkButton');
    var workErrorMessageDiv = document.getElementById('workErrorMessage');
    var formatErrorMessageDiv = document.getElementById('formatErrorMessage');

    addWorkButton.addEventListener('click', function () {
        var workplaceValue = document.getElementById('workplaceInput').value;
        var roleValue = document.getElementById('roleInput').value;
        var startValue = parseInt(document.getElementById('startInput').value, 10);
        var endValue = parseInt(document.getElementById('endInput').value, 10);

        // Validera att startdatumet inte är mindre än tilldatumet
        if (isNaN(startValue) || isNaN(endValue) || startValue > endValue) {
            workErrorMessageDiv.textContent = 'Felaktiga datum. Kontrollera att startdatumet är mindre än tilldatumet.';
            return; // Avbryt funktionen om valideringen misslyckas
        }

        // Anropa funktionen för att lägga till arbete om valideringen är framgångsrik
        addWork(workplaceValue, roleValue, startValue, endValue);
    });
});

function addWork(workplace, role, start, end) {
    // Skicka data till servern med Ajax
    $.ajax({
        url: '/Profile/AddWorkExperience',
        type: 'POST',
        data: { workplace: workplace, role: role, from: start, to: end },
        success: function (response) {
            // Hantera framgångsrikt svar
            window.location.reload(); // Ladda om sidan för att se den nya arbetslivserfarenheten
        },
        error: function (xhr, status, error) {
            if (xhr.status === 400) {
                workErrorMessageDiv.textContent = xhr.responseText; // Visa felmeddelandet
            } else {
                console.error("Error: " + error);
            }
        }
    });
}

function deleteWork(itemId) {
    // Bekräfta att användaren verkligen vill ta bort arbetslivserfarenheten.
    var confirmDelete = confirm("Är du säker på att du vill ta bort arbetslivserfarenheten?");

    if (confirmDelete) {
        // Skicka en AJAX-förfrågan för att ta bort arbetslivserfarenheten.
        $.ajax({
            type: "POST",
            url: '/Profile/DeleteWorkExperience/' + itemId,
            success: function () {
                // Ta bort det relevanta elementet från DOM.
                $('#workExperience-' + itemId).remove();

                // Ladda om sidan för att uppdatera arbetslivserfarenheterna.
                location.reload();
            },
            error: function () {
                alert("Något gick fel vid borttagning av arbetslivserfarenheten.");
            }
        });
    }
}


