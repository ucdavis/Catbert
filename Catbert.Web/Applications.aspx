<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="Applications.aspx.cs" Inherits="Applications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
		body{ font: 62.5% Verdana, sans-serif; margin: 50px;}
		/*demo page css*/
		#dialog_link {padding: .4em 1em .4em 20px;text-decoration: none;position: relative;}
		#dialog_link span.ui-icon {margin: 0 5px 0 0;position: absolute;left: .2em;top: 50%;margin-top: -8px;}
	</style>
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

            //List selection TEST ONLY
            //$.fcbkListSelection(id[ul id],width,height[element height],row[elements in row]);
            //$.fcbkListSelection("#ulPermissions", 400, 50, 2);
        });

        function ShowUserInfo(applicationID, name) {
            var dialog = $("#dialogUserInfo"); //the dialog div

            dialog.dialog("destroy"); //Reset the dialog to its initial state
            dialog.dialog({
                autoOpen: true,
                width: 600,
                modal: true,
                title: name,
                buttons: {
                    "Close": function() {
                        $(this).dialog("close");
                    },
                    "Update": function() {
                        UpdateApplication(applicationID, name);
                        $(this).dialog("close");
                    }
                }
            });

            //var buttons = $(dialog.dialog('option', 'buttons').Update).attr('disabled', true);
            //debugger;

            ShowApplicationInformation(false);  //Don't show the information until it loads
            
            //Clear out the roles list checked options
            $(":checked", roleList).attr('checked', false);

            
            AjaxCall(
                baseUrl + 'GetApplication',
                { application: name },
                function(data) { PopulateApplication(data); },
                OnError //TODO: Error method
            );
                        
            dialog.dialog('open'); //show
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

            //Application is populated, so show the information div
            ShowApplicationInformation(true);
        }

        ///Update the given application, which resides in the row identified by the ID
        function UpdateApplication(ID, name) {
            var appName = $("#txtApplicationName").val();
            var appAbbr = $("#txtApplicationAbbr").val();
            var appLocation = $("#txtApplicationLocation").val();

            var roles = new Array();
            $(":checked", roleList).each(function() {
                roles.push($(this).val());
            });

            //Now we have the update information, send it to the web service
            AjaxCall(baseUrl + 'UpdateApplication', {
                application: name,
                newName: appName,
                newAbbr: appAbbr,
                newLocation: appLocation,
                roles: roles
            },
            function() {
                UpdateApplicationComplete(ID, appName, appLocation);
            },
            OnError);
            
            //UpdateApplicationComplete(ID, appName, appLocation);   
        }

        function UpdateApplicationComplete(ID, appName, appLocation) {
            var row = $("#row" + ID); //The changed row
            
            var nameCell = $("td[title=Name]", row);
            var locationCell = $("td[title=Location] a", row);

            nameCell.html(appName);
            locationCell.html(appLocation);
            locationCell.attr('href', appLocation);
            
            $("td", row).effect("highlight", {}, 3000); //highlight the whole row that was changed
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
            <tr id='row<%# Eval("ID") %>'>
                <td>
                    <a href="javascript:;" id="dialog_link" class="ui-state-default ui-corner-all" onclick='ShowUserInfo(<%# Eval("ID") %>, "<%# Eval("Name") %>");'>
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
                <td>
                    <%# !(bool)Eval("Inactive") %>
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
            Roles:
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
            <asp:ObjectDataSource ID="odsRoles" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" 
                TypeName="CAESDO.Catbert.BLL.RoleBLL">
                <SelectParameters>
                    <asp:Parameter DefaultValue="Name" Name="propertyName" Type="String" />
                    <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
        </div>
	</div>
</asp:Content>

