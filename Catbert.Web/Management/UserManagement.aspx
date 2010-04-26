<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="Management_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../CSS/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="../JS/jquery.ajaxQueue.js" type="text/javascript"></script>
    <script src="../JS/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="../JS/jquery.tablesorter.min.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        var baseURL = '../Services/CatbertWebService.asmx/';
        var autocompleteUnitsURL = '../Services/AutocompleteService.asmx/GetUsers';
        $(document).ready(function() {
            var application = $("#app").val();
            var search = null, unit = null, role = null; //start with no search, unit, or role filters
            var sortname = "LastName";
            var sortorder = "ASC";

            PopulateUserTable(application, search, unit, role, sortname, sortorder); //Populate the user table

            $("#txtSearch").autocomplete(autocompleteUnitsURL, {
                width: 260,
                minChars: 2,
                selectFirst: false,
                autoFill: false,
                extraParams: { application: application },
                formatItem: function(row, i, max) {
                    return row.Name + " (" + row.Login + ")<br/>" + row.Email; //i + "/" + max + ": \"" + row.name + "\" [" + row.to + "]";
                }
            });

            $("#txtSearch").keypress(function(event) {
                if (event.keyCode == 13) {
                    search = $(this).val() /*textbox value*/;
                    PopulateUserTable(application, search, unit, role, sortname, sortorder);
                    $(".ac_results").hide(); //Hide the results whenever you hit enter
                    return false; //Don't post back
                }
            });

            $("#tblUsers").tablesorter({
                headers: { 4: { sorter: false }, 5: { sorter: false} },
                cssAsc: 'headerSortUp',
                cssDesc: 'headerSortDown',
                cssHeader: 'header',
                widgets: ['zebra']
            });

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

            $("#btnSearchUser").click(function() {
                $("#spanSearchProgress").show(0); //Show the loading dialog
                $("#divSearchResultsSuccess").hide(0); //Hide the content

                var data = { eid: null, firstName: null, lastName: null, login: $("#txtLoginID").val() };

                //Call the search service
                AjaxCall(baseURL + "SearchNewUser", data, SearchNewUserSuccess, null);
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
                function() {
                    $("#dialogFindUser").dialog("close");
                    $("#divNewUserNotification").show("slow");
                    $("#spanAddUserProgress").hide(0); //First hide the progress since we are done
                    PopulateUserTable(application, null, null, null, null, null);
                },
                null);
            });
        });

        function SearchNewUserSuccess(data) {
                        
            var divSearchResults = $("#divSearchResultsSuccess"); //Get the search results div
            $("#spanSearchProgress").hide(0); //Hide the loading dialog
            
            if (data.length == 0) {                
                return alert("No Users Found");
            }
            else {
                var user = data[0];
                $("#spanNewUserFirstName").html(user.FirstName);
                $("#spanNewUserLastName").html(user.LastName);
                $("#spanNewUserLogin").html(user.Login);
                $("#txtNewUserEmail").val(user.Email);
                $("#txtNewUserPhone").val(user.Phone);
            }

            divSearchResults.show();
        }

        function PopulateUserTable(application, search, unit, role, sortname, sortorder) {
            ShowLoadingIndicator(true);
            
            //Setup the parameters
            var data = { application: application, search: search, unit: unit, role: role, sortname: sortname, sortorder: sortorder };
            
            //Call the webservice
            AjaxCall(baseURL + 'jqGetUsers', data, PopulateUserTableSuccess, null);
        }

        function PopulateUserTableSuccess(data) {
            //data = data.d; //Grab the inner container of data

            //Clear the usertable
            $("#tblUsersBody").empty();
            
            //Render out each row
            $(data.rows).each(RenderRow);

            SortTable();
            ShowLoadingIndicator(false);
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

            newrow.append('<td class="FirstName">' + row.FirstName + '</td>');
            newrow.append('<td class="LastName">' + row.LastName + '</td>');
            newrow.append('<td class="Login">' + row.Login + '</td>');
            newrow.append('<td class="Email">' + row.Email + '</td>');

            var units = CreateDomFromUserInfoArray(row.Units);
            var roles = CreateDomFromUserInfoArray(row.Roles);

            newrow.append('<td class="Units">' + units + '</td>');
            newrow.append('<td class="Roles">' + roles + '</td>');

            $("#tblUsersBody").append(newrow);
        }

        //Trigger a sort update, and sort on the last name column
        function SortTable() {
            $("#tblUsers").trigger("update");
            var sorting = [[1, 0]];
            // sort on the first column 
            $("#tblUsers").trigger("sorton", [sorting]);
        }

        function CreateDomFromUserInfoArray(array) {
            var dom = "";

            $(array).each(function(index, obj) {
                if (index != 0) //For everything other than the first element, include the comma
                    dom += ", ";

                dom += Trim(obj.Name);
            });

            return dom;
        }

        function Trim(str) {
            return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
        }

        function OpenDialog(dialog /*The dialog DIV JQuery object*/, buttons /*Button collection */, title) {

            dialog.dialog("destroy"); //Reset the dialog to its initial state
            dialog.dialog({
                autoOpen: true,
                width: 600,
                modal: true,
                title: title,
                buttons: buttons
            });
        }
    </script>
    
    <a href="javascript:;" id="addUser" class="dialog_link ui-state-default ui-corner-all">
        <span class="ui-icon ui-icon-newwin"></span>Add User
    </a>
    <div class="ui-widget" id="divNewUserNotification" style="display:none;">
        <br />
        <div class="ui-state-highlight ui-corner-all" >
            <p>
                <span class="ui-icon ui-icon-info" style="float:left;"></span>
                User Added Successfully
            </p>
        </div>
    </div>
<br /><br />

    <div id="divHeader">
        <span id="search">
            Search Users: <input type="text" id="txtSearch" />
        </span>
    </div>
    <div id="divLoading" style="display:none;">
        Loading...
    </div>
    <table id="tblUsers" class="tablesorter">
        <thead>
            <tr>
                <th >First Name</th>
                <th >Last Name</th>
                <th >Login</th>
                <th >Email</th>
                <th >Departments</th>
                <th >Roles</th>
            </tr>
        </thead>
        <tbody id="tblUsersBody">
            <%--Each row is a new person--%>
        </tbody>
    </table>
    
    <div id="dialogFindUser" title="Add a User" style="display: none;">
        <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
        Kerberos LoginID: <input type="text" id="txtLoginID" /><input type="button" id="btnSearchUser" value="Search" />
        <span id="spanSearchProgress" style="display:none;">Searching...</span>
        <div id="divSearchResultsSuccess" style="display:none;">
            <span id="spanNewUserFirstName"></span> <span id="spanNewUserLastName"></span> (<span id="spanNewUserLogin"></span>)<br />
            Email: <input type="text" id="txtNewUserEmail" /><br />
            Phone: <input type="text" id="txtNewUserPhone" /><br />
            Role:
            <asp:ListView ID="lviewRoles" runat="server" DataSourceID="odsRoles">
                <LayoutTemplate>
                    <select id="applicationRoles">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option>
                        <%# Eval("Name") %>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsRoles" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetRolesByApplication" TypeName="CAESDO.Catbert.BLL.RoleBLL">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="app" Name="application" DefaultValue="Catbert" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            Unit:
            <asp:ListView ID="lviewUnits" runat="server" DataSourceID="odsUnits">
                <LayoutTemplate>
                    <select id="units">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option value='<%# Eval("FISCode") %>'>
                        <%# Eval("ShortName")%>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsUnits" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetAll" TypeName="CAESDO.Catbert.BLL.UnitBLL">
                <SelectParameters>
                    <asp:Parameter DefaultValue="ShortName" Name="propertyName" Type="String" />
                    <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br /><br />
            <input type="button" id="btnAddUser" value="Add" /><span id="spanAddUserProgress" style="display:none;">Processing...</span>
        </div>
    </div>
        
</asp:Content>

