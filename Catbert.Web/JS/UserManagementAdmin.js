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

var tabs;

$(document).ready(function() {
    user = $("#user").val();

    PopulateUserTable(application, search, unit, role, sortname, sortorder); //Populate the user table

    tabs = $('#tabs').tabs();

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

    $(".ShowUserLink").live("click", ShowUserInfo);

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

    $("#btnAddPermission").click(AddUserRole);

    $("#btnAddUnits").click(AddUserUnit);

    $("#btnSearchUser").click(function() {
        $("#spanSearchProgress").show(0); //Show the loading dialog
        $("#divSearchResultsSuccess").hide(0); //Hide the content

        //var data = { eid: null, firstName: null, lastName: null, email: null, login: $("#txtLoginID").val() };
        var data = { searchTerm: $("#txtLoginID").val() };

        //Call the search service
        AjaxCall(baseURL + "FindUser", data, SearchNewUserSuccess, null);
        //AjaxCall(baseURL + "SearchNewUser", data, SearchNewUserSuccess, null);
    });

    $("#btnAddUser").click(AddUser);

    $("#filterApplications").change(function() {
        page = 1; //Reset the paging

        application = $(this).val();

        console.info(application);
        PopulateUserTableDefault(application);
    });

    $("#applications").change(function() {
        OnApplicationSelected($(this).val(), $("#applicationRoles"));
    });

    $("#applicationsPermissions").change(function() {
        OnApplicationSelected($(this).val(), $("#rolesPermissions"));
    });
});

//Populate an associated roles list for the selected application
function OnApplicationSelected(selectedApp, associatedRolesList) {
    
    if (selectedApp == "") {
        associatedRolesList.attr("disabled", "disabled"); //disable the application roles ddl
        return;
    }

    AjaxCall(
                baseURL + "GetRolesForApplication",
                { application: selectedApp },
                function(data) { PopulateRolesForApplication(data, associatedRolesList); },
                null
            );

    console.info(selectedApp);
}

function PopulateRolesForApplication(data, rolesList) {
    console.dir(data);

    var appRoles = rolesList;
    appRoles.empty();

    appRoles.removeAttr("disabled");
    
    $(data).each(function() {
        appRoles.append("<option>" + this +"</option>");
    });
}

function SearchUsers() {
    search = $("#txtSearch").val() /*textbox value*/;
    page = 1; //Change the page when a new search is executed

    //Clear unit and role filters
    $("#filterApplications option:nth(0)").attr("selected", true);

    application = null;
    
    PopulateUserTable(application, search, unit, role, sortname, sortorder);

    setTimeout(function() {
        $(".ac_results").hide(); //Hide the results whenever you hit enter
    }, 250);

    return false; //Don't post back
}

function AddUser() {
    var applicationName = $("#applications").val();
    var applicationRole = $("#applicationRoles").val();

    if (applicationName == "") applicationName = null;
    if (applicationRole == "") applicationRole = null;

    console.info(applicationName);
    console.info(applicationRole);

    if (applicationName == null || applicationRole == null) {
        alert("Select an Application and Role");
        return;
    }
    
    $("#spanAddUserProgress").show(0); //First show the loading dialog

    //First fill in the user information
    var user = new Object();
    user.FirstName = $("#spanNewUserFirstName").html();
    user.LastName = $("#spanNewUserLastName").html();
    user.Login = $("#spanNewUserLogin").html();
    user.Email = $("#txtNewUserEmail").val();
    user.Phone = $("#txtNewUserPhone").val();

    //Now get the role
    var role = applicationRole

    //Now the unit FIS code
    var unit = $("#units").val();

    AjaxCall(baseURL + "InsertUserWithRoleAndUnit", 
        {
            serviceUser: user,
            role: role,
            unit: unit,
            application: applicationName
        },
        function() { AddUserSuccess(application, search); },
        null);
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

function AddUserRole() {
    var login = $("#UserInfoLogin").html();

    var existingRoles = $("#tblPermissions");
    
    var newrole = $("#rolesPermissions").val();
    var app = $("#applicationsPermissions").val();
    
    if (app == '') app = null;
    if (newrole == '') newrole = null;

    if (app === null || newrole === null) {
        alert("You must select an application and a role to associate a new permission");
        return;
    }

    console.info(app + "  " + newrole);

    //Find out if the role is already in associated
    var existingRoleMatch = existingRoles.find("tbody tr:visible").filter(
                function() {
                    var appMatch = $(this).find("td:contains(" + app + ")").size();
                    var roleMatchingTds = $(this).find("td:contains(" + newrole + ")");

                    var roleMatch = 0;
                    if (roleMatchingTds.text() == newrole) roleMatch = 1;

                    if (appMatch === 1 && roleMatch === 1) //Did we find an application and role match in the same row?
                        return true;
                    else
                        return false;
                }
            );

    if (existingRoleMatch.size() == 0) {

        //Add this role
        var newrow = CreateRoleRow(newrole, login, app);
        existingRoles.append(newrow);

        $(newrow).effect("highlight", {color: "#555555"}, 3000);

        AjaxCall(baseURL + "AssociateRole",
                    { login: login, role: newrole, application: app },
                    null,
                    null
                );
    }
    else {
        alert("User already has the role " + $.trim(newrole) + ' in ' + $.trim(app));
    }
}

function AddUserUnit() {
    var login = $("#UserInfoLogin").html();
    var units = $("#tblUnits");
    
    var newunit = $("#unitsForAssociation").val();
    var newunitname = $("#unitsForAssociation option:selected").text();

    var app = $("#applicationsUnits").val();

    //Find out if the unitFIS is already in the unit table
    var existingUnitMatch = units.find("tbody tr:visible").filter(
                function() {
                    var appMatch = $(this).find("td:contains(" + app + ")").size();
                    var unitMatch = $(this).find("td:contains(" + newunit + ")").size();
                    
                    if (appMatch === 1 && unitMatch === 1) //Did we find an application and unit match in the same row?
                        return true;
                    else
                        return false;
                }
            );

    if (existingUnitMatch.size() == 0) {
        
        //Add the unit
        var newrow = CreateUnitRow(newunit, login, app);
        
        units.append(newrow);

        $(newrow).effect("highlight", {color: "#555555"}, 3000);

        AjaxCall(baseURL + "AssociateUnit",
                    { login: login, application: app, unitFIS: newunit },
                    null,
                    null
                );
    }
    else {
        alert("User already has the unit " + $.trim(newunitname) + ' in ' + $.trim(app));
    }
}

function ShowUserInfo() {
    var loginId = $(this).html();

    var roles = $("#tblPermissions tbody");
    var units = $("#tblUnits tbody");

    //Clear out the old roles and units
    roles.empty();
    units.empty();

    console.info(loginId);
    //alert(loginId);

    var dialogUserInfo = $("#dialogUserInfo");

    var buttons = {
        "Close": function() {
            //$("#divUserInfo").hide(0);
            $(this).dialog("close");
        }
    }

    tabs.tabs('select', 0); //select the first tab by default when viewing a new user

    OpenDialog(dialogUserInfo, buttons, "User Information", null);

    var url = baseURL + 'GetUserInfo';

    AjaxCall(
                url,
                { loginId: loginId },
                function(data) { PopulateUserInfo(data); },
                null //TODO: Error method
            );
}

function PopulateUserInfo(data) {

    var loginId = data.LoginId;
    var roles = $("#tblPermissions tbody");
    var units = $("#tblUnits tbody");

    //Clear out the old roles and units
    roles.empty();
    units.empty();

    PopulateUserInformation(data);
    
    $(data.PermissionAssociations).each(function() {
        var newRoleRow = CreateRoleRow(this.RoleName, loginId, this.ApplicationName);

        roles.append(newRoleRow);
    });

    $(data.UnitAssociations).each(function() {
        var newUnitRow = CreateUnitRow(this.UnitFIS, loginId, this.ApplicationName);

        units.append(newUnitRow);
    });
}

function PopulateUserInformation(data) {
    //Insert the userInfo
    $("#UserInfoLogin").html(data.LoginId);
    $("#UserInfoName").html(data.FirstName + " " + data.LastName);
    $("#UserInfoEmail").html(data.Email);
    $("#UserInfoPhone").html(data.Phone);
}

function CreateRoleRow(role, login, application) {
    var newrow = $('<tr></tr>');

    var deleteLink = $('<input type="button" value="X" />');
    deleteLink.click(function() { DeleteRole(login, role, application, newrow); });

    newrow.append('<td>' + application + '</td>');
    newrow.append('<td>' + role + '</td>');

    var deleteCol = $('<td>').append(deleteLink);
    newrow.append(deleteCol);

    return newrow;
}

function CreateUnitRow(unit, login, application) {
    var newrow = $('<tr></tr>');

    var deleteLink = $('<input type="button" value="X" />');
    deleteLink.click(function() { DeleteUnit(login, unit, application, newrow); });

    newrow.append('<td>' + application + '</td>');
    newrow.append('<td>' + unit + '</td>');

    var deleteCol = $('<td>').append(deleteLink);
    newrow.append(deleteCol);

    return newrow;
}

function DeleteUnit(login, unit, application, rowToDelete) {
    $(rowToDelete).fadeOut('slow');

    AjaxCall(baseURL + "DissociateUnit",
                { login: login, application: application, unitFIS: unit },
                null,
                null
            );
}

function DeleteRole(login, role, application, rowToDelete) {
    $(rowToDelete).fadeOut('slow');

    AjaxCall(baseURL + "DissociateRole",
                { login: login, role: role, application: application },
                null,
                null
            );
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