///<reference path="jquery-1.3.1-vsdoc.js"/>

var baseURL = '../Services/Admin/CatbertAdminService.asmx/';

var application = null;
var user = null;

var search = null, unit = null, role = null; //start with no search, unit, or role filters

var page = 1;
var pageSize = 3;
var totalPages = 1;

var sortname = "LastName";
var sortorder = "ASC";

var userTableDirty = false;
var rowEven = true;

$(document).ready(function() {
    user = $("#user").val();

    PopulateUserTable(application, search, unit, role, sortname, sortorder); //Populate the user table

    $("#txtSearch").autocomplete(baseURL + 'GetUsersAutoComplete', {
        width: 260,
        minChars: 2,
        selectFirst: false,
        autoFill: false,
        extraParams: { application: application },
        formatItem: function(row, i, max) {
            return row.Name + " (" + row.Login + ")<br/>" + row.Email; //i + "/" + max + ": \"" + row.name + "\" [" + row.to + "]";
        }
    }).result(SearchUsers);

    $("#txtSearch").keypress(function(event) {
        if (event.keyCode == 13) {
            return SearchUsers();
        }
    });

    $("#imgSearch").click(function() {
        search = "";
        $("#txtSearch").val(search); //clear out the text
        page = 1; //Go back to the first page

        PopulateUserTable(application, search, unit, role, sortname, sortorder);
        $(".ac_results").hide(); //Hide the results whenever you hit enter

        return false;
    });

    $("#txtLoginID").keypress(function(event) {
        if (event.keyCode == 13) { //On enter, fire the search click event
            $("#btnSearchUser").click();
        }
    });

    $(".pager").click(function() {
        var pagerType = $(this).attr("name");

        if (pagerType == "First") {
            page = 1;
        }
        else if (pagerType == "Last") {
            page = totalPages;
        }
        else if (pagerType == "Previous") {
            page--;
            if (page == 0) page = 1;
        }
        else { //pagerType = "Next"
            page++;
            if (page > totalPages) page = totalPages;
        }

        PopulateUserTableDefault(application);
    });

    $("#tblUsers thead tr th.header").click(ChangeSortOrder);

    $(".ShowUserLink").live("click", function() { ShowUserInfo(application, $(this).html()); });

    $("#addUser").click(function() {
        var findUserDialog = $("#dialogFindUser");

        var buttons = {
            "Close": function() {
                $("#divSearchResultsSuccess").hide(0);
                $(this).dialog("close");
            }
        }

        $("#divNewUserNotification").hide(0); //If there was a previous user added, hide the notification

        OpenDialog(findUserDialog, buttons, "Add a User");
    });

    $("#btnAddUserRole").click(function() { AddUserRole(application); });

    $("#btnAddUserUnit").click(function() { AddUserUnit(application); });

    $("#btnSearchUser").click(function() {
        $("#spanSearchProgress").show(0); //Show the loading dialog
        $("#divSearchResultsSuccess").hide(0); //Hide the content

        //var data = { eid: null, firstName: null, lastName: null, email: null, login: $("#txtLoginID").val() };
        var data = { searchTerm: $("#txtLoginID").val() };

        //Call the search service
        AjaxCall(baseURL + "FindUser", data, SearchNewUserSuccess, null);
        //AjaxCall(baseURL + "SearchNewUser", data, SearchNewUserSuccess, null);
    });

    $("#btnAddUser").click(function() {
        $("#spanAddUserProgress").show(0); //First show the loading dialog

        //First fill in the user information
        var user = new Object();
        user.FirstName = $("#spanNewUserFirstName").html();
        user.LastName = $("#spanNewUserLastName").html();
        user.Login = $("#spanNewUserLogin").html();
        user.Email = $("#txtNewUserEmail").val();
        user.Phone = $("#txtNewUserPhone").val();

        //Now get the role
        var role = $("#applicationRoles").val();

        //Now the unit FIS code
        var unit = $("#units").val();

        AjaxCall(baseURL + "InsertUserWithRoleAndUnit", {
            serviceUser: user,
            role: role,
            unit: unit,
            application: application
        },
                function() { AddUserSuccess(application, search); },
                null);
    });

    $("#filterRoles").change(function() {
        page = 1; //Reset the paging

        role = $(this).val();

        PopulateUserTableDefault(application);
    });

    $("#filterUnits").change(function() {
        page = 1; //Reset the paging    

        unit = $(this).val();

        PopulateUserTableDefault(application);
    });
});

function SearchUsers() {
    search = $("#txtSearch").val() /*textbox value*/;
    page = 1; //Change the page when a new search is executed

    //Clear unit and role filters
    $("#filterRoles option:nth(0)").attr("selected", true);
    $("#filterUnits option:nth(0)").attr("selected", true);

    role = null;
    unit = null;
    
    PopulateUserTable(application, search, unit, role, sortname, sortorder);

    setTimeout(function() {
        $(".ac_results").hide(); //Hide the results whenever you hit enter
    }, 250);

    return false; //Don't post back
}

function ChangeSortOrder() {
    sortname = $(this).attr("title");
    sortorder = $(this).hasClass("headerSortUp") ? "DESC" : "ASC";

    var sortableHeaders = $(".header");
    sortableHeaders.removeClass("headerSortUp").removeClass("headerSortDown"); //remove the sort direction classes from each sortable col

    //add the sort direction class back into the current header object
    if (sortorder == "ASC")
        $(this).addClass("headerSortUp");
    else
        $(this).addClass("headerSortDown");

    //After changing the sortvars, repopulate the grid
    PopulateUserTableDefault(application);
}

function AddUserRole(application) {
    var login = $("#UserInfoLogin").html();
    var roles = $("#UserInfoRoles");
    var newrole = $("#UserRoles").val();

    //Find out if the role is already in the roles table
    var existingRoleMatch = roles.find("tbody tr:visible td:contains(" + newrole + ")").filter(
                function() {
                    if ($(this).text() == newrole)
                        return true;
                    else
                        return false;
                }
            );

    if (existingRoleMatch.size() == 0) {
        //Add this role
        var newrow = CreateRoleRow(newrole, login, application);
        roles.append(newrow);

        $(newrow).effect("highlight", {color: "#555555"}, 3000);

        AjaxCall(baseURL + "AssociateRole",
                    { login: login, role: newrole, application: application },
                    null,
                    null
                );

        userTableDirty = true; //Users have been modified
    }
    else {
        alert("User already has the role " + newrole);
    }
}

function AddUserUnit(application) {
    var login = $("#UserInfoLogin").html();
    var units = $("#UserInfoUnits");
    var newunit = $("#UserUnits").val();
    var newunitname = $("#UserUnits option:selected").text();

    //Find out if the unitFIS is already in the unit table
    var existingUnitMatch = units.find("tbody tr:visible td:contains(" + newunit + ")").filter(
                function() {
                    if ($(this).text() == newunit)
                        return true;
                    else
                        return false;
                }
            );

    if (existingUnitMatch.size() == 0) {
        //Add the unit
        var newrow = CreateUnitRow(newunitname, newunit, login, application);
        units.append(newrow);

        $(newrow).effect("highlight", {color: "#555555"}, 3000);

        AjaxCall(baseURL + "AddUnit",
                    { login: login, application: application, unitFIS: newunit },
                    null,
                    null
                );

        userTableDirty = true; //Users have been modified
    }
    else {
        alert("User already has the unit " + $.trim(newunitname));
    }
}

function ShowUserInfo(applicationName, Login) {
    var dialogUserInfo = $("#dialogUserInfo");

    var buttons = {
        "Close": function() {
            $("#divUserInfo").hide(0);
            $(this).dialog("close");
        }
    }

    OpenDialog(dialogUserInfo, buttons, "User Information",
                function() {
                    if (userTableDirty) {
                        if (Login == user) {
                            //We must repopulate the page since the currently logged in user changed their account
                            window.location.reload();
                        }
                        else {
                            //We can just repopulate the user's table since someone else was modified
                            PopulateUserTableDefault(applicationName);
                        }
                    }
                }
            );

    var baseUrl = baseURL;

    AjaxCall(
                baseUrl + 'GetUser',
                { login: Login, application: applicationName },
                function(data) { PopulateUserInfo(data, applicationName); },
                null //TODO: Error method
            );
}

function PopulateUserInfo(data, application) {
    $("#UserInfoName").html(data.FirstName + " " + data.LastName);
    $("#UserInfoLogin").html(data.Login);

    var roles = $("#UserInfoRoles tbody");
    var units = $("#UserInfoUnits tbody");

    roles.empty();
    units.empty();

    $(data.Roles).each(function(index, row) {
        var newrow = CreateRoleRow(row.Name, data.Login, application);

        roles.append(newrow);
    });

    $(data.Units).each(function(index, row) {
        var newrow = CreateUnitRow(row.Name, row.UnitFIS, data.Login, application);

        units.append(newrow);
    });

    $("#divUserInfo").show(0); //Show the user information
}

function CreateRoleRow(role, login, application) {
    var newrow = $('<tr></tr>');

    var deleteLink = $('<input type="button" value="X" />');
    deleteLink.click(function() { DeleteRole(login, role, application, newrow); });

    newrow.append('<td>' + role + '</td>');

    var deleteCol = $('<td>').append(deleteLink);
    newrow.append(deleteCol);

    return newrow;
}

function CreateUnitRow(unit, unitFIS, login, application) {
    var newrow = $('<tr></tr>');

    var deleteLink = $('<input type="button" value="X" />');
    deleteLink.click(function() { DeleteUnit(login, unitFIS, application, newrow); });

    newrow.append('<td>' + unit + '</td>');
    newrow.append('<td>' + unitFIS + '</td>');

    var deleteCol = $('<td>').append(deleteLink);
    newrow.append(deleteCol);

    return newrow;
}

function DeleteUnit(login, unit, application, rowToDelete) {
    $(rowToDelete).fadeOut('slow');

    AjaxCall(baseURL + "DeleteUnit",
                { login: login, application: application, unitFIS: unit },
                null,
                null
            );

    userTableDirty = true; //Users have been modified
}

function DeleteRole(login, role, application, rowToDelete) {
    $(rowToDelete).fadeOut('slow');

    AjaxCall(baseURL + "DissociateRole",
                { login: login, role: role, application: application },
                null,
                null
            );

    userTableDirty = true; //Users have been modified
}

function AddUserSuccess(application, search) {
    PopulateUserTable(application, search, null, null, null, null); //Repopulate the table

    $("#dialogFindUser").dialog("close"); //Close the dialog
    $("#divSearchResultsSuccess").hide(0); //Hide the search results
    $("#divNewUserNotification").show("slow"); //Show the new user notification
    $("#spanAddUserProgress").hide(0); //Hide the progress since we are done
}

function SearchNewUserSuccess(data) {

    var divSearchResults = $("#divSearchResultsSuccess"); //Get the search results div
    $("#spanSearchProgress").hide(0); //Hide the loading dialog

    if (data == null) {
        return alert("No Users Found");
    }
    else {
        var user = data;
        $("#spanNewUserFirstName").html(user.FirstName);
        $("#spanNewUserLastName").html(user.LastName);
        $("#spanNewUserLogin").html(user.Login);
        $("#txtNewUserEmail").val(user.Email);
        $("#txtNewUserPhone").val(user.Phone);
    }

    divSearchResults.show();
}

function PopulateUserTableDefault(application) {
    PopulateUserTable(application, search, unit, role, sortname, sortorder);
}

function PopulateUserTable(application, search, unit, role, sortname, sortorder) {
    ShowLoadingIndicator(true);

    //Setup the parameters
    var data = {
        application: application,
        search: search,
        unit: unit,
        role: role,
        page: page,
        pagesize: pageSize,
        sortname: sortname,
        sortorder: sortorder
    };
    
    //Call the webservice
    AjaxCall(baseURL + 'GetUsers', data, PopulateUserTableSuccess, null);
}

function PopulateUserTableSuccess(data) {
    //data = data.d; //Grab the inner container of data

    //Clear the usertable
    $("#tblUsersBody").empty();

    //Reset the rowEven to true since the first row will be 0
    rowEven = true;

    //Render out each row
    $(data.rows).each(RenderRow);

    totalPages = data.total;

    //Populate Page Info
    $("#spanPageInfo").html(page + " of " + totalPages);

    SortTable();
    ShowLoadingIndicator(false);
    userTableDirty = false; //The user table is now up to date
}

function ShowLoadingIndicator(on) {
    var loadingDiv = $("#divLoading");

    if (on)
        loadingDiv.show();
    else
        loadingDiv.hide();
}

function RenderRow(index, row) {
    var newrow = $('<tr></tr>');

    rowEven == true ? newrow.addClass("even") : newrow.addClass("odd");
    rowEven = !rowEven;

    newrow.append('<td class="Login"><a href="javascript:;" class="ShowUserLink">' + row.Login + '</a></td>');
    newrow.append('<td class="FirstName">' + row.FirstName + '</td>');
    newrow.append('<td class="LastName">' + row.LastName + '</td>');
    // newrow.append('<td class="Login">' + row.Login + '</td>');
    newrow.append('<td class="Email">' + row.Email + '</td>');

    var units = CreateDomFromUserInfoArray(row.Units);
    var roles = CreateDomFromUserInfoArray(row.Roles);

    newrow.append('<td class="Units">' + units + '</td>');
    newrow.append('<td class="Roles">' + roles + '</td>');

    $("#tblUsersBody").append(newrow);
}

//Trigger a sort update, and sort on the last name column
function SortTable() {
    /*
    if ($("#tblUsersBody tr").size() > 0) { //Only resort sort if there are rows to sort
            
    $("#tblUsers").trigger("update");
    var sorting = [[1, 0]];
    // sort on the first column 
    $("#tblUsers").trigger("sorton", [sorting]);
    }
    */
}

function CreateDomFromUserInfoArray(array) {
    var dom = "";

    $(array).each(function(index, obj) {
        if (index != 0) //For everything other than the first element, include the comma
            dom += ", ";

        dom += $.trim(obj.Name);
    });

    return dom;
}

function OpenDialog(dialog /*The dialog DIV JQuery object*/, buttons /*Button collection */, title, onClose) {

    dialog.dialog("destroy"); //Reset the dialog to its initial state
    dialog.dialog({
        autoOpen: true,
        closeOnEscape: false,
        width: 600,
        height: 600,
        modal: true,
        title: title,
        buttons: buttons,
        //show: 'fold',
        close: onClose
    });
}