<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="ApplicationsOld.aspx.cs" Inherits="Applications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="CSS/fcbklistselection.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="JS/jquery.tablesorter.min.js" type="text/javascript"></script>
    
    <script type="text/javascript">
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
            
            //Go through each role and check the corresonding box          
            for (var i in app.Roles) {
                var roleName = app.Roles[i].Name;
                var roleBox = $("input[value=" + roleName + "]", roleList); //Find the one role with the value of roleName                
                roleBox.attr('checked', 'checked');//Check it
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
    </script>

    <a href="javascript:;" id="addApplication" class="dialog_link ui-state-default ui-corner-all">
        <span class="ui-icon ui-icon-newwin"></span>Add Application
    </a>
    <br /><br />
    <asp:ListView ID="lviewApplications" runat="server" DataSourceID="odsApplications">
        <LayoutTemplate>
            <table id="tblApplications" class="tablesorter">
                <thead>
                    <tr>
                        <th>&nbsp;                            
                        </th>
                        <th>
                            Application Name
                        </th>
                        <th>
                            Abbreviation
                        </th>
                        <th>
                            Location
                        </th>
                        <th>
                            Active
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr id='<%# Eval("ID") %>' title='<%# Eval("Name") %>'>
                <td title="Select">
                    <a href="javascript:;" class="dialog_link ui-state-default ui-corner-all" title="Select Application">
                        <span class="ui-icon ui-icon-newwin"></span>
                        Select 
                    </a>
                </td>
                <td title="Name">
                    <%# Eval("Name") %>
                </td>
                <td title="Abbr">
                    <%# Eval("Abbr") %>
                </td>
                <td title="Location">
                    <a href='<%# Eval("Location") %>' target='_blank'><%# Eval("Location") %></a>
                </td>
                <td title="Active">
                    <input class="activeIndicator" type="image" style="border-width: 0;" value='<%# Eval("Name") %>'
                        src='<%# (bool)Eval("Inactive") ? "Images/Inactive.gif" : "Images/Active.gif"%>' 
                        alt='<%# (bool)Eval("Inactive") ? "Inactive" : "Active" %>' />
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            No Applications Found (How did you do it?)
        </EmptyDataTemplate>
    </asp:ListView>

    <asp:ObjectDataSource ID="odsApplications" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" 
        TypeName="CAESDO.Catbert.BLL.ApplicationBLL">
        <SelectParameters>
            <asp:Parameter DefaultValue="Name" Name="propertyName" Type="String" />
            <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
    <!-- ui-dialog -->
	<div id="dialogUserInfo" title="Application Information" style="display: none;">
		<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
		<div>
		    <span id="spanLoading">Loading....</span>
		</div>
		<div id="divApplicationInfo" style="visibility:hidden">
		<table>
		    <tr>
		        <td>Name</td>
		        <td><input type="text" id="txtApplicationName" size="40" /></td>
		    </tr>
		    <tr>
		        <td>Abbr</td>
		        <td><input type="text" id="txtApplicationAbbr" size="40" /></td>
		    </tr>
		    <tr>
		        <td>Location</td>
		        <td><input type="text" id="txtApplicationLocation" size="40" /></td>
		    </tr>
		</table>
        <br /><br />
            <div id="Roles">
                <div id="roletabs" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
                    <ul id="roleViewOptions" class="ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
                        <li id="activeRoles" class="ui-corner-top"><a id="activeRolesLink" href="javascript:;" class="ui-state-active"><span>Active Roles:</span></a></li>
                        <li id="allRoles" class="ui-corner-top"><a id="allRolesLink" href="javascript:;" class="ui-state-default"><span>All Roles:</span></a></li>
                    </ul>
                    <div class="ui-tabs-panel ui-widget-content ui-corner-bottom">
                        <asp:ListView ID="lviewRoles" runat="server" DataSourceID="odsRoles">
                            <LayoutTemplate>
                                <ul id="ulRoles">
                                    <li id="itemPlaceholder" runat="server"></li>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <input type="checkbox" value="<%# Eval("Name") %>" /><%# Eval("Name") %>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                        <asp:ObjectDataSource ID="odsRoles" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetAll" TypeName="CAESDO.Catbert.BLL.RoleBLL">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="Name" Name="propertyName" Type="String" />
                                <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                </div>
            </div>
            <div id="addRole" style="display: none">
                <span id="spanAddRole">
                    <input type="text" id="txtAddRole" />
                </span>
                <a href="javascript:;" id="btnAddRole" class="dialog_link ui-state-default ui-corner-all">
                    <span class="ui-icon ui-icon-plusthick"></span>Add Role 
                </a>
            </div>
        </div>
	</div>
</asp:Content>

