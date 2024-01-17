function addSkill(newSkill) {
	$.ajax({
		url: '/Profile/AddSkill', 
		type: 'POST',
		data: { newSkill: newSkill },
		success: function (response) {
			// Hantera framgångsrikt svar
			window.location.reload(); // Ladda om sidan för att se den nya färdigheten
		},
		error: function (xhr, status, error) {
			if (xhr.status === 400) { 
				alert(xhr.responseText); // Visa felmeddelandet om man försöker lägga till dubblett
			} else {
				console.error("Error: " + error);
			}
		}

	});
}

function deleteSkill(itemId) {
	// Bekräfta att användaren verkligen vill ta bort utbildningen.
	var confirmDelete = confirm("Är du säker på att du vill ta bort färdigheten?");
	if (confirmDelete) {
		// Skicka en AJAX-förfrågan för att ta bort utbildningen.
		$.ajax({
			type: "POST",
			url: '/Profile/DeleteSkill/' + itemId,
			success: function () {
				// Ta bort det relevanta elementet från DOM.
				$('#skill-' + itemId).remove();

				// Ladda om sidan för att uppdatera CV:et.
				location.reload();
			},
			error: function () {
				alert("Något gick fel vid borttagning.");
			}
		});
	}
}