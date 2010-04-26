///<reference path="jquery-1.3.1-vsdoc.js"/>

var baseUrl = 'Services/CatbertWebService.asmx/';
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
            el.attr('src', 'Images/Inactive.gif');
            el.attr('alt', inactiveText);
        }
        else {
            el.attr('src', 'Images/Active.gif');
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

        //Clear out the checkboxes
        $("#dialogUserInfo :checked").each(function() {
            $(this).attr('checked', false);
        });

        //Show all the checkboxes
        ChangeRoleDisplay($("#allRolesLink"));
        ShowActiveRolesOnly(false);

        //Make sure the application info div is visible, and the loading span is hidden
        $("#divApplicationInfo").css('visibility', 'visible');
        $("#spanLoading").fadeTo(0, 0);

        OpenDialog(buttons, "Create New Application");
    });

    //Bind the click method of the add role button
    $("#btnAddRole").click(function() {
        //Call the add role method, and add the role to the rolelist
        var roleName = $("#txtAddRole").val();

        var checkbox = $("<input type='checkbox' value='" + roleName + "' />");
        var newListElement = $("<li></li>");

        newListElement.append(checkbox).append(roleName);

        $("#ulRoles").append(newListElement);

        //Now flash the new element!
        newListElement.effect("highlight", {}, 2000); //highlight the whole row that was changed

        AjaxCall(baseUrl + 'AddRole', { role: roleName }, null, OnError);
    });

    //Hook up a click handler for the 'active roles only' button
    $("#roleViewOptions li a").click(function() {
        ChangeRoleDisplay($(this));

        //Now if this is the allRoles button, show all, else just show checked roles
        if (this.id == "activeRolesLink")
            ShowActiveRolesOnly(true); //just active roles
        else
            ShowActiveRolesOnly(false);
    });
});

function ChangeRoleDisplay(el) {
    var activeState = "ui-state-active";
    var defaultState = "ui-state-default";

    //Reset all of the anchors to their default classes
    $("#roleViewOptions li a").removeClass(activeState).addClass(defaultState);
    el.removeClass(defaultState).addClass(activeState);
}

function ShowActiveRolesOnly(activeOnly) {
    if (typeof (rolelist) == undefined) roleList = $("#ulRoles");
    var addRoleButton = $("#addRole");

    if (activeOnly) {
        //var checkedlist = $("li", roleList).filter(" :has(:checked)");
        $("li", roleList).filter(" :has(:checked)").show();
        $("li", roleList).filter(" :has(:not(:checked))").hide();
        addRoleButton.hide(0);
    }
    else {
        $("li", roleList).show();
        addRoleButton.show(0);
    }
}

function ShowApplicationInfo() {
    var row = $(this).parents("tr");
    var applicationID = row.attr('id');
    var applicationName = row.attr('title');

    var buttons = {
        "Close": function() {
            $(this).dialog("close");
        },
        "Update": function() {
            var sortableList = $("#sortableRoles");
            var info = sortableList.sortable('toArray');
            console.log(info);
            
            ///TODO: Testing
            //UpdateApplication(applicationID, applicationName);
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
    
    //All of the applications with a non null level go in the sortableRoles list, others go in the nonSortableRoles list
    for (var i in app.Roles) {
        var roleName = app.Roles[i].Name;
        var listElement = $("<li>").attr("id", roleName).addClass("ui-state-default").append(roleName);

        //If this role is in the allRolesList, hide it
        allRolesList.find("#" + roleName).hide(0);

        if (app.Roles[i].Level == null) {
            console.info("Null Level", roleName);
            
            //var listElement = $("<li>").attr("id", roleName).addClass("ui-state-default").append(roleName);
            nonSortableRolesList.append(listElement);
        }
        else {
            console.info("Has Level", roleName);

            sortableRolesList.append(listElement);
        }
    }
    
    //Go through each role and check the corresonding box          
    for (var i in app.Roles) {
        var roleName = app.Roles[i].Name;
        var roleBox = $("input[value=" + roleName + "]", roleList); //Find the one role with the value of roleName                
        roleBox.attr('checked', 'checked'); //Check it
    }

    //Now show only the checked ones
    ChangeRoleDisplay($("#activeRolesLink"));
    ShowActiveRolesOnly(true);

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
        roles: application.roles
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
                    roles: application.roles
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

    application.roles = new Array();
    $(":checked", roleList).each(function() {
        application.roles.push($(this).val());
    });

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

function OnError() {
    alert("Danger Will Robinson!");
}