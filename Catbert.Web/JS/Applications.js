///<reference path="jquery-1.3.1-vsdoc.js"/>

var baseUrl = '../Services/CatbertWebService.asmx/';
var roleList;

$(document).ready(function() {
    roleList = $('#ulRoles');
    //Sort table
    $("#tblApplications").tablesorter({
        headers: { 0: { sorter: false} },
        sortList: [[1, 0]],
        cssAsc: 'headerSortUp',
        cssDesc: 'headerSortDown',
        cssHeader: 'header',
        widgets: ['zebra']
    });

    $(".connectedSortable").sortable({
        connectWith: '.connectedSortable',
        placeholder: 'ui-state-highlight'
    }).disableSelection();

    //Bind the ShowApplicationInfo method to the select links
    $("#tblApplications tbody tr td[title=Select] a").click(ShowApplicationInfo);

    //Create a live binding to images with the activeIndicator class' click event
    $(":image.activeIndicator").live("click", (function() {
        //Swap the images
        var el = $(this);
        var activeText = 'Active';
        var inactiveText = 'Inactive';

        if (el.attr('alt') == activeText) {
            el.attr('src', '../Images/Inactive.gif');
            el.attr('alt', inactiveText);
        }
        else {
            el.attr('src', '../Images/Active.gif');
            el.attr('alt', activeText);
        }

        var containingRow = $(this).parents('tr');
        containingRow.children("td").effect('highlight', {}, 300);

        //Now call the webservice to do the active switching
        AjaxCall(baseUrl + 'ChangeApplicationActiveStatus', { application: el.val() }, null, OnError);

        return false;
    }));

    //Bind the click method of the add user button
    $("#addApplication").click(function() {
        var buttons = {
            "Close": function() {
                $(this).dialog("close");
            },
            "Create": function() {
                CreateApplication(this);
                $(this).dialog("close");
            }
        }

        //Clear out the input boxes
        $("#dialogUserInfo :text").val('');

        var sortableRolesList = $("#sortableRoles");
        var nonSortableRolesList = $("#nonSortableRoles");
        var allRolesList = $("#availableRoles");

        //Clear out the associated roles lists, and unhide the allRolesList
        sortableRolesList.empty();
        nonSortableRolesList.empty();
        allRolesList.find("li").show(0);

        //Make sure the application info div is visible, and the loading span is hidden
        $("#divApplicationInfo").css('visibility', 'visible');
        $("#spanLoading").fadeTo(0, 0);

        OpenDialog(buttons, "Create New Application");
    });

    //Bind the click method of the add role button
    $("#btnAddRole").click(function() {
        //Call the add role method, and add the role to the rolelist
        var roleName = $("#txtAddRole").val();

        var newListRole = $("<li class='ui-state-default' id='" + roleName + "'></li>");

        //var checkbox = $("<input type='checkbox' value='" + roleName + "' />");
        var newListElement = newListRole.append(roleName);

        $("#availableRoles").append(newListElement);

        //Now flash the new element!
        newListElement.effect("highlight", {}, 2000); //highlight the whole row that was changed

        AjaxCall(baseUrl + 'AddRole', { role: roleName }, null, OnError);
    });
});

function ShowApplicationInfo() {
    var row = $(this).parents("tr");
    var applicationID = row.attr('id');
    var applicationName = row.attr('title');

    var buttons = {
        "Close": function() {
            $(this).dialog("close");
        },
        "Update": function() {
            UpdateApplication(applicationID, applicationName);
            $(this).dialog("close");
        }
    }

    OpenDialog(buttons, applicationName);

    ShowApplicationInformation(false);  //Don't show the information until it loads

    //Clear out the roles list checked options
    $(":checked", roleList).attr('checked', false);


    AjaxCall(
                baseUrl + 'GetApplication',
                { application: applicationName },
                function(data) { PopulateApplication(data); },
                OnError //TODO: Error method
            );
}

function OpenDialog(buttons, title) {
    var dialog = $("#dialogUserInfo"); //the dialog div

    dialog.dialog("destroy"); //Reset the dialog to its initial state
    dialog.dialog({
        autoOpen: true,
        width: 600,
        modal: true,
        title: title,
        buttons: buttons
    });
}

function PopulateApplication(app) {
    //Populate the application info boxes
    $("#txtApplicationName").val(app.Name);
    $("#txtApplicationAbbr").val(app.Abbr);
    $("#txtApplicationLocation").val(app.Location);

    var sortableRolesList = $("#sortableRoles");
    var nonSortableRolesList = $("#nonSortableRoles");
    var allRolesList = $("#availableRoles");

    //Clear out the associated roles lists, and unhide the allRolesList
    sortableRolesList.empty();
    nonSortableRolesList.empty();
    allRolesList.find("li").show(0);
    
    //All of the applications with a non null level go in the sortableRoles list, others go in the nonSortableRoles list
    for (var i in app.Roles) {
        var roleName = app.Roles[i].Name;
        var listElement = $("<li>").attr("id", roleName).addClass("ui-state-default").append(roleName);

        //If this role is in the allRolesList, hide it
        allRolesList.find("#" + roleName).hide(0);

        if (app.Roles[i].Level == null) {
            LogMessage("Null Level", roleName);
            
            nonSortableRolesList.append(listElement);
        }
        else {
            LogMessage("Has Level", roleName);
            
            sortableRolesList.append(listElement);
        }
    }

    //Application is populated, so show the information div
    ShowApplicationInformation(true);
}

///Update the given application, which resides in the row identified by the ID
function UpdateApplication(ID, name) {
    var application = CollectApplicationInformation();

    //Now we have the update information, send it to the web service
    AjaxCall(baseUrl + 'UpdateApplication', {
        application: name,
        newName: application.appName,
        newAbbr: application.appAbbr,
        newLocation: application.appLocation,
        leveledRoles: application.sortableRoles,
        nonLeveledRoles: application.nonSortableRoles
    },
            function() {
                UpdateApplicationComplete(ID, application.appName, application.appAbbr, application.appLocation);
            },
            OnError);
}

function UpdateApplicationComplete(ID, appName, appAbbr, appLocation) {
    var row = $("#" + ID); //The changed row

    var nameCell = $("td[title=Name]", row);
    var abbrCell = $("td[title=Abbr]", row);
    var locationCell = $("td[title=Location] a", row);

    nameCell.html(appName);
    abbrCell.html(appAbbr);

    locationCell.html(appLocation);
    locationCell.attr('href', appLocation);

    $("td", row).effect("highlight", {}, 3000); //highlight the whole row that was changed
}

//Create the new application
function CreateApplication(el) {
    var application = CollectApplicationInformation();

    AjaxCall(
                baseUrl + 'CreateApplication',
                {
                    application: application.appName,
                    abbr: application.appAbbr,
                    location: application.appLocation,
                    leveledRoles: application.sortableRoles,
                    nonLeveledRoles: application.nonSortableRoles
                },
                function() { window.location.reload(); },
                OnError
            );
}

function OnCreateApplicationComplete(applicationName, abbr, location) {
    //Create a new row in the application table with this application information
}

//Gets all of the information out of the application popup
function CollectApplicationInformation() {
    var application = new Object();
    application.appName = $("#txtApplicationName").val();
    application.appAbbr = $("#txtApplicationAbbr").val();
    application.appLocation = $("#txtApplicationLocation").val();
    
    var sortableList = $("#sortableRoles");
    var nonSortableList = $("#nonSortableRoles");

    var sortableRoles = sortableList.sortable('toArray');
    var nonSortableRoles = nonSortableList.sortable("toArray");

    LogMessage("Sortable", sortableRoles);
    LogMessage("NonSortable", nonSortableRoles);

    application.sortableRoles = sortableRoles;
    application.nonSortableRoles = nonSortableRoles;  

    return application;
}

function ShowApplicationInformation(loaded) {
    var infoVisible = loaded ? 'visible' : 'hidden';
    var loadingOpacity = loaded ? 0 : 1; //go to invisible if we have loaded
    var updateButton = $(":button:contains('Update')"); //.attr("disabled", "disabled");

    if (loaded) updateButton.attr('disabled', false);
    else updateButton.attr('disabled', 'disabled');

    $("#divApplicationInfo").css('visibility', infoVisible);
    $("#spanLoading").fadeTo('fast', loadingOpacity);
}

function LogMessage(message, data) {

    var canDebug = typeof (window.console) != "undefined";

    if (canDebug) {
        console.log(message, data);
    }
}

function OnError() {
    alert("Danger Will Robinson!");
}