$(document).ready(function ()
{
	var dateIssuedFrom = $('#dateissuedfromS')[0];
	var dateIssuedTo = $('#dateissuedtoS')[0];
	var dateIssuedFromWeeks = $('#dateissuedfromWeeksS')[0];
	var dateIssuedFromDays = $('#dateissuedfromDaysS')[0];
	var dateIssuedToWeeks = $('#dateissuedtoWeeksS')[0];
	var dateIssuedToDays = $('#dateissuedtoDaysS')[0];
	var dateSentFrom = $('#datesentfromS')[0];
	var dateSentTo = $('#datesenttoS')[0];
	var dateSentFromWeeks = $('#datesentfromWeeksS')[0];
	var dateSentFromDays = $('#datesentfromDaysS')[0];
	var dateSentToWeeks = $('#datesenttoWeeksS')[0];
	var dateSentToDays = $('#datesenttoDaysS')[0];

	var helpBtn = $('#helpBtn')[0];
	var helpPanel = $('#helpPanel')[0];

	$('#selectPredefinedFilter').click(function ()
	{
		var fid = $('#fid').find(":selected").val();
		var actionUrl = 'Reports/GetFilter?id=' + fid;
		//alert("id = " + fid);
		$.getJSON(actionUrl, SetFilterInputs);
	});

	$('#dateissuedfromWeeksS').on('change', function (e)
	{
		var from = Number(dateIssuedFromWeeks.value * 7) + Number(dateIssuedFromDays.value);
		var to = Number(dateIssuedToWeeks.value * 7) + Number(dateIssuedToDays.value);

		if (from < to)
		{
			if (dateIssuedFromDays.value < dateIssuedToDays.value)
				$(this).val(dateIssuedToWeeks.value - 1);
			else
				$(this).val(dateIssuedToWeeks.value);
			from = Number(dateIssuedFromWeeks.value * 7) + Number(dateIssuedFromDays.value);
		}
		var toSet = new Date();
		toSet.setDate(toSet.getDate() - from);
		var toSetString = toSet.toISOString();
		$('#dateissuedfromS').val(toSetString.slice(0, 10));
		dateIssuedTo.setAttribute("min", dateIssuedFrom.value);
	});

	$('#dateissuedfromDaysS').on('change', function (e)
	{
		var from = Number(dateIssuedFromWeeks.value * 7) + Number(dateIssuedFromDays.value);
		var to = Number(dateIssuedToWeeks.value * 7) + Number(dateIssuedToDays.value);

		if (from < to)
		{
			$(this).val(dateIssuedToDays.value);
			from = Number(dateIssuedFromWeeks.value * 7) + Number(dateIssuedFromDays.value);
		}
		var toSet = new Date();
		toSet.setDate(toSet.getDate() - from);
		var toSetString = toSet.toISOString();
		$('#dateissuedfromS').val(toSetString.slice(0, 10));
		dateIssuedTo.setAttribute("min", dateIssuedFrom.value);
	});

	$('#dateissuedtoWeeksS').on('change', function (e)
	{
		var from = Number(dateIssuedFromWeeks.value * 7) + Number(dateIssuedFromDays.value);
		var to = Number(dateIssuedToWeeks.value * 7) + Number(dateIssuedToDays.value);

		if (from < to)
		{
			if (dateIssuedFromDays.value < dateIssuedToDays.value)
				$(this).val(dateIssuedFromWeeks.value - 1);
			else
				$(this).val(dateIssuedFromWeeks.value);
			to = Number(dateIssuedToWeeks.value * 7) + Number(dateIssuedToDays.value);
		}
		var toSet = new Date();
		toSet.setDate(toSet.getDate() - to);
		var toSetString = toSet.toISOString();
		$('#dateissuedtoS').val(toSetString.slice(0, 10));
		dateIssuedFrom.setAttribute("max", dateIssuedTo.value);
	});

	$('#dateissuedtoDaysS').on('change', function (e)
	{
		var from = Number(dateIssuedFromWeeks.value * 7) + Number(dateIssuedFromDays.value);
		var to = Number(dateIssuedToWeeks.value * 7) + Number(dateIssuedToDays.value);

		if (from < to)
		{
			$(this).val(dateIssuedFromDays.value);
			to = Number(dateIssuedToWeeks.value * 7) + Number(dateIssuedToDays.value);
		}
		var toSet = new Date();
		toSet.setDate(toSet.getDate() - to);
		var toSetString = toSet.toISOString();
		$('#dateissuedtoS').val(toSetString.slice(0, 10));
		dateIssuedFrom.setAttribute("max", dateIssuedTo.value);
	});

	$('#datesentfromWeeksS').on('change', function (e)
	{
		var from = Number(dateSentFromWeeks.value * 7) + Number(dateSentFromDays.value);
		var to = Number(dateSentToWeeks.value * 7) + Number(dateSentToDays.value);

		if (from < to)
		{
			$(this).val(dateSentToWeeks.value);
			from = Number(dateSentFromWeeks.value * 7) + Number(dateSentFromDays.value);
		}
		var toSet = new Date();
		toSet.setDate(toSet.getDate() - from);
		var toSetString = toSet.toISOString();
		$('#datesentfromS').val(toSetString.slice(0, 10));
		dateSentTo.setAttribute("min", dateSentFrom.value);
	});

	$('#datesentfromDaysS').on('change', function (e)
	{
		var from = Number(dateSentFromWeeks.value * 7) + Number(dateSentFromDays.value);
		var to = Number(dateSentToWeeks.value * 7) + Number(dateSentToDays.value);

		if (from < to)
		{
			$(this).val(dateSentToDays.value);
			from = Number(dateSentFromWeeks.value * 7) + Number(dateSentFromDays.value);
		}
		var toSet = new Date();
		toSet.setDate(toSet.getDate() - from);
		var toSetString = toSet.toISOString();
		$('#datesentfromS').val(toSetString.slice(0, 10));
		dateSentTo.setAttribute("min", dateSentFrom.value);
	});

	$('#datesenttoWeeksS').on('change', function (e)
	{
		var from = Number(dateSentFromWeeks.value * 7) + Number(dateSentFromDays.value);
		var to = Number(dateSentToWeeks.value * 7) + Number(dateSentToDays.value);

		if (from < to)
		{
			$(this).val(dateSentFromWeeks.value);
			to = Number(dateSentToWeeks.value * 7) + Number(dateSentToDays.value);
		}
		var toSet = new Date();
		toSet.setDate(toSet.getDate() - to);
		var toSetString = toSet.toISOString();
		$('#datesenttoS').val(toSetString.slice(0, 10));
		dateSentFrom.setAttribute("max", dateSentTo.value);
	});

	$('#datesenttoDaysS').on('change', function (e)
	{
		var from = Number(dateSentFromWeeks.value * 7) + Number(dateSentFromDays.value);
		var to = Number(dateSentToWeeks.value * 7) + Number(dateSentToDays.value);

		if (from < to)
		{
			$(this).val(dateSentFromDays.value);
			to = Number(dateSentToWeeks.value * 7) + Number(dateSentToDays.value);
		}
		var toSet = new Date();
		toSet.setDate(toSet.getDate() - to);
		var toSetString = toSet.toISOString();
		$('#datesenttoS').val(toSetString.slice(0, 10));
		dateSentFrom.setAttribute("max", dateSentTo.value);
	});
	//zeroing relative data input when given concrete ones by the user
	$('#dateissuedfromS').on('change', function ()
	{
		dateIssuedTo.setAttribute("min", dateIssuedFrom.value);
		dateIssuedFromWeeks.value = dateIssuedFromDays.value = dateIssuedToWeeks.value = dateIssuedToDays.value = "";
	});

	$('#dateissuedtoS').on('change', function ()
	{
		dateIssuedFrom.setAttribute("max", dateIssuedTo.value);
		dateIssuedFromWeeks.value = dateIssuedFromDays.value = dateIssuedToWeeks.value = dateIssuedToDays.value = "";
	});

	$('#datesentfromS').on('change', function ()
	{
		dateSentTo.setAttribute("min", dateSentFrom.value);
		dateSentFromWeeks.value = dateSentFromDays.value = dateSentToWeeks.value = dateSentToDays.value = "";
	});

	$('#datesenttoS').on('change', function ()
	{
		dateSentFrom.setAttribute("max", dateSentTo.value);
		dateSentFromWeeks.value = dateSentFromDays.value = dateSentToWeeks.value = dateSentToDays.value = "";
	});

	//In case we want to handle creating a filter via jquery it needs
	//to be completed and controller action modified a bit
	/*$('#saveOrUpdateFilter').click(function ()
	{
		var etrNumber = $('#etrnumberS').val();
		var rfaId = $('#rfaidS').val();
		var rfaName = $('#rfanameS').val();
		var priority = $('#priorityS').val();
		var grade = $('#gradeS').val();
		var troubleType = $('#troubletypeS').val();

		var dateIssuedFrom = $('#dateissuedfromS').val();
		var dateIssuedTo = $('#dateissuedtoS').val();
		var dateIssuedFromDaysAgo = $('#dateissuedfromDaysS').val();
		var dateIssuedFromWeeksAgo = $('#dateissuedfromWeeksS').val();
		var dateIssuedToDaysAgo = $('#dateissuedtoDaysS').val();
		var dateIssuedToWeeksAgo = $('#dateissuedtoWeeksS').val();

		var dateSentFrom = $('#datesentfromS').val();
		var dateSentTo = $('#datesenttoS').val();
		var dateSentFromDaysAgo = $('#datesentfromDaysS').val();
		var dateSentFromWeeksAgo = $('#datesentfromWeeksS').val();
		var dateSentToDaysAgo = $('#datesenttoDaysS').val();
		var dateSentToWeeksAgo = $('#datesenttoWeeksS').val();

		var etrStatus = $('#etrstatusS').val();
		var etrType = $('#etrtypeS').val();
		var nsnCoordinator = $('#nsncoordS').val();
		var subcontractor = $('#subconS').val();
		var zone = $('#zoneS').val();
		var name = $('#filterName').val();

		$.post(

		);
	});*/
	
});

function SetFilterInputs(response)
{
	if (response != null)
	{
		$('#etrnumberS').val(response.etrNumber);
		$('#rfaidS').val(response.rfaId);
		$('#rfanameS').val(response.rfaName);
		$('#priorityS').val(response.priority);
		$('#gradeS').val(response.grade);
		$('#troubletypeS').val(response.troubleType);

		$('#dateissuedfromS').val(response.dateIssuedFrom);
		$('#dateissuedtoS').val(response.dateIssuedTo);
		$('#dateissuedfromDaysS').val(response.dateIssuedFromDaysAgo);
		$('#dateissuedfromWeeksS').val(response.dateIssuedFromWeeksAgo);
		$('#dateissuedtoDaysS').val(response.dateIssuedToDaysAgo);
		$('#dateissuedtoWeeksS').val(response.dateIssuedToWeeksAgo);

		$('#datesentfromS').val(response.dateSentFrom);
		$('#datesenttoS').val(response.dateSentTo);
		$('#datesentfromDaysS').val(response.dateSentFromDaysAgo);
		$('#datesentfromWeeksS').val(response.dateSentFromWeeksAgo);
		$('#datesenttoDaysS').val(response.dateSentToDaysAgo);
		$('#datesenttoWeeksS').val(response.dateSentToWeeksAgo);

		$('#etrstatusS').val(response.etrStatus);
		$('#etrtypeS').val(response.etrType);
		$('#nsncoordS').val(response.nsnCoordinatorId);
		$('#subconS').val(response.subcontratorId);
		$('#zoneS').val(response.zone);
		$('#filterName').val(response.name);

		if ($('#dateissuedfromWeeksS').val()) $('#dateissuedfromWeeksS').trigger('change');
		if ($('#dateissuedfromDaysS').val()) $('#dateissuedfromDaysS').trigger('change');
		if ($('#dateissuedtoWeeksS').val()) $('#dateissuedtoWeeksS').trigger('change');
		if ($('#dateissuedtoDaysS').val()) $('#dateissuedtoDaysS').trigger('change');
		if ($('#datesentfromWeeksS').val()) $('#datesentfromWeeksS').trigger('change');
		if ($('#datesentfromDaysS').val()) $('#datesentfromDaysS').trigger('change');
		if ($('#datesenttoWeeksS').val()) $('#datesenttoWeeksS').trigger('change');
		if ($('#datesenttoDaysS').val()) $('#datesenttoDaysS').trigger('change');
	}
}

var options_before = {
	steps: [
		{
			element: '#reportPanel',
			intro: 'Here are your reports.'
		},
		{
			element: '#reportTableHeaders',
			intro: "You can sort your reports by clicking on column headers."
		},
		{
			element: '#filterCollapsibleToggle',
			intro: "Click the header to open a panel with filters."
		},
		{
			element: '#filterDatePickers',
			intro: "You can filter tables with a calendar or pick reports with dates relative to this day."
		},
		{
			element: '#filterPicker',
			intro: "Pick a filter then click <b>Select</b> to select a previously saved filter."
		},
		{
			element: '#filterSaverRemover',
			intro: "You can save current filter by giving it a name and clicking on <b>Save</b> button or remove an existing filter by providing its name and clicking on the trash bin."
		},
		{
			element: '#applyFilter',
			intro: "Hit <b>Search</b> to apply chosen filters."
		},
		{
			element: '#createReport',
			intro: "Click on <b>Create</b> button to manually create a new report."
		},
		{
			element: '#exportReports',
			intro: "Click on <b>Export</b> button to download an Excel sheet with reports you see in the table."
		}
	]
};

function startIntro()
{
	var intro = introJs();
	intro.setOptions(options_before);
	intro.start().onbeforechange(function ()
	{
		if (intro._currentStep == "3")
		{
			$('#filterCollapsible').collapse("show");
		}
	});
}