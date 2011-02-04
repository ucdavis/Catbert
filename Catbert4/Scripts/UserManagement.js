$(function () {

	CreateButtons();

	AssignIndicators(); //Find and assign loading indicators

	AssignVerificationToken();

	$("#insert-new-user").validate();

	Catbert.UserTable = $("#users").dataTable({
		"bJQueryUI": true,
		"iDisplayLength": 25,
		"sPaginationType": "full_numbers",
		"aoColumns": [null, null, null, null, { "bSortable": false }, { "bSortable": false}], //Don't sort the last two cols
		"aaSorting": [[2, "asc"]]
	});

	$(".user-filter").change(function () {
		var element = $(this);
		Catbert.UserTable.fnFilter(element.val(), element.data("filter-column"));
	});

	$("#add-user").click(function (e) {
		e.preventDefault();

		$("#find-user").dialog({ modal: true, width: '50%' });
	});

	$("#search-user").click(function (e) {
		e.preventDefault();

		Catbert.Indicators.SearchProgress.show(0); //Show the loading dialog
		$("#search-results").hide(0); //Hide the content

		var data = { searchTerm: $("#search-login").val() };

		Log(data);
		Log(Catbert.Services.FindUser);

		$.getJSON(Catbert.Services.FindUser, data, SearchNewUserSuccess);
	});

	$("#search-login").keypress(function (e) {
		if (e.keyCode == 13) {
			$("#search-user").click();
		}
	});

	$("#add-new-user").click(function(e) {
		e.preventDefault();

		Catbert.Indicators.AddNewUserProgress.show(0);

		var form = $("#insert-new-user");

		if (!form.valid()) return;
				
		$.post(Catbert.Services.InsertNewUser, form.serialize(), function(result) { AddNewUserSuccess(result); }, 'json');
	});

	$(".modify-user").live("click", function(e){
		e.preventDefault();

		Catbert.Indicators.UserInfoProgress.show(0);

		//Load the user info
		var login = this.id;

		Log(login);

		$.getJSON(Catbert.Services.LoadUser,
			{ login: login },
			function (data) {
				PopulateUserInfo(data);
				Catbert.Indicators.UserInfoProgress.hide(0);
			}
		);

		$("#manage-user").dialog({ 
			modal: true, 
			width: '50%', 
			position: 'top',
			beforeClose: function(event, ui) {
				UpdateRecord(login); //Update the record before closing if it's changed
				//unbind click handlers
				$(".remove-link, .add-link").unbind('click');
				$("#user-info").remove();
			},
			buttons: {
				"Close": function() {
					$(this).dialog("close");
				}
			}
		});
	});
});

function CreateButtons() {
    $("#add-user").button({
		icons: {
			primary: "ui-icon-person"
		}
	});

	$("#search-user").button({
		icons: {
			primary: "ui-icon-search"
		}
	});

	$("#add-new-user").button({
		icons: {
			primary: "ui-icon-plus"
		}
	});
}

function AssignIndicators(){
	Catbert.Indicators.SearchProgress = $("#search-progress");
	Catbert.Indicators.AddNewUserProgress = $("#add-new-user-progress");
	Catbert.Indicators.UserInfoProgress = $("#user-info-progress");
}

function AssignVerificationToken() {
	Catbert.VerificationToken = $("input[name=__RequestVerificationToken]").val();
}

function SearchNewUserSuccess(data){
	Log(data);

	Catbert.Indicators.SearchProgress.hide(0);

	if (data == null){
		alert("No Users Found");
	}
	else {
		var searchResults = $("#search-results");

		$("#new-user-first-name-display", searchResults).html(data.FirstName);
		$("#new-user-last-name-display", searchResults).html(data.LastName);
		$("#new-user-login-display", searchResults).html(data.Login);
		$("#new-user-first-name", searchResults).val(data.FirstName);
		$("#new-user-last-name", searchResults).val(data.LastName);
		$("#new-user-login", searchResults).val(data.Login);
		$("#new-user-email", searchResults).val(data.Email);
		$("#new-user-phone", searchResults).val(data.Phone);

		searchResults.show(0);
	}
}

function AddNewUserSuccess(data){
	var userTable = Catbert.UserTable;

	//First find if the given login is already in the table
	var userRow = GetRowFor(data.Login);

	if (userRow.length != 0) { //if we are already in the table, remove the row first                
		var position = userTable.fnGetPosition(userRow[0]);

		userTable.fnDeleteRow(position);
	}

	var userLink = "<a href=\"#\" id=\""+ data.Login + "\" class=\"modify-user\" title=\""+ "Modify " + data.FullNameAndLogin +"\">"+ data.Login + "</a>";

	userTable.fnAddData([
		userLink,
		data.FirstName,
		data.LastName,
		data.Email,
		data.Units,
		data.Roles
	]);            

	Catbert.Indicators.AddNewUserProgress.hide(0);
	$("#search-results").hide(0);
	$("#find-user").dialog("close"); //Close the dialog            
	$("#message-text").html(data.FullNameAndLogin + " has been added successfully");
	$("#message").show("slow"); //Show the new user notification
}

function PopulateUserInfo(data) {
    //Check to see which permissions and units are editable by this user
    for (i in data.Permissions)
    {
        var perm = data.Permissions[i];

        var arrayIndex = $.inArray(perm.RoleId, Catbert.User.Roles);
        
        perm.UserEditable = arrayIndex == -1 ? false : true;
    }

    for (i in data.UnitAssociations) {
        var ua = data.UnitAssociations[i];

        var arrayIndex = $.inArray(ua.UnitId, Catbert.User.Units);

        ua.UserEditable = arrayIndex == -1 ? false : true;
    }

	var userInfo = $("#user-info-template").tmpl(data);
	$("#manage-user").append(userInfo);
			
	//Assign the click handlers
	$(".remove-link").click(function(e){
		e.preventDefault();
				
		RemoveAssociation($(this));
	});

	$(".add-link").click(function(e) {
		e.preventDefault();

		AddAssociation($(this));
	});

	StyleUserInfoButtons();
}
		
function RemoveAssociation(removeLink){
	var link = removeLink;

	if (removeLink.hasClass("no-delete")) {
	    return;
	}

	var row = link.parent().parent();

	row.fadeOut("slow", function() { row.remove(); });

	var data = { login: link.data("login"), id: link.data("id"), __RequestVerificationToken: Catbert.VerificationToken };
	Log(data);

	var url = link.data("type") == "permission" ? Catbert.Services.RemovePermission : Catbert.Services.RemoveUnit;
			
	$.post(url, data, null, null);
}

function AddAssociation(addLink){
	var link = addLink;

	var type = link.data("type");
				
	var url, list, table;

	if (type == "permission"){
		url = Catbert.Services.AddPermission;
		list = $("#userRoles");
		table = $("#user-info-roles");
	}
	else {
		url = Catbert.Services.AddUnit;
		list = $("#userUnits");
		table = $("#user-info-units");
	}

	var login = link.data("login");
	var id = list.val();

	var selectedText = list.find("option:selected").text();

	var existingColumnMatch = table.find("tbody tr td:contains(" + selectedText + ")").filter(
		function() {
			if ($(this).text() == selectedText)
				return true;
			else
				return false;
		}
	);

	if (existingColumnMatch.size() != 0) {
		alert("The user already has the " + type + " " + selectedText);
		return;
	}

	var rowValues = { value: selectedText, login: login, type: type, id: id };
	//Add a new row
	var newRow = $("#user-info-row-template").tmpl(rowValues);
	table.append(newRow);

	//Add the click binding on the new row
	$(".remove-link", newRow).click(function(e){
		e.preventDefault();
				
		RemoveAssociation($(this));
	});

	list.effect("transfer", { to: newRow }, "slow");
	newRow.effect("highlight", {}, 'slow');

	StyleUserInfoButtons();

	var data = { login: login, id: id, __RequestVerificationToken: Catbert.VerificationToken };
	Log(data);

	$.post(url, data, null, null);
}

//Update the row for the given login, setting the unit associations and permissions
function UpdateRecord(login){
	var row = GetRowFor(login); //The current row in the table
	var rowElement = row.get(0);
			
	//Get the user's roles and permissions, then push them into arrays and then join them into readable strings
	var roles = $("#user-info-roles tbody tr td:first-child");
	var units = $("#user-info-units tbody tr td:first-child");

	var rolesArray = [], unitsArray = [];

	roles.each(function(i){
			rolesArray.push(this.innerHTML);
	});            
			
	units.each(function(i){
			unitsArray.push(this.innerHTML);
	});
			
	var allRoles = rolesArray.sort().join(', ');
	var allUnits = unitsArray.sort().join(', ');

	Log(allRoles);
	Log(allUnits);

	if (rolesArray.length == 0 || unitsArray.length == 0) {
	    //Remove the user's row if they don't have any roles or units
	    Catbert.UserTable.fnDeleteRow(rowElement);
	}
	else {
		Catbert.UserTable.fnUpdate(allUnits, rowElement, 4, false, false); //False params to not redraw until second call
		Catbert.UserTable.fnUpdate(allRoles, rowElement, 5);
	}
}

function GetRowFor(login){
	return $("#" + login).parent().parent(); //get the user link, then find it's parent (td)'s parent (tr)
}

function StyleUserInfoButtons(){
	//style the buttons
	$(".remove-link").button({
		icons: {
			primary: "ui-icon-trash"
		}
	});

	$(".add-link").button({
		icons: {
			primary: "ui-icon-plusthick"
		}
	});

    $(".no-delete").button("option", "disabled", true);
}

function Log(text){
	if (typeof console != "undefined") {
		console.info(text);
	}
}