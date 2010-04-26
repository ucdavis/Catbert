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

        var application = null;
        
        var search = null, unit = null, role = null; //start with no search, unit, or role filters
        
        var page = 1;
        var pageSize = 3;
        var totalPages = 1;
        
        var sortname = "LastName";
        var sortorder = "ASC";

        var userTableDirty = false;

        $(document).ready(function() {
            application = $("#app").val();

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
                    page = 1; //Change the page when a new search is executed
                    PopulateUserTable(application, search, unit, role, sortname, sortorder);
                    $(".ac_results").hide(); //Hide the results whenever you hit enter
                    return false; //Don't post back
                }
            });

            $("#imgSearch").click(function() {
                search = "";
                $("#txtSearch").val(search); //clear out the text

                PopulateUserTable(application, search, unit, role, sortname, sortorder);
                $(".ac_results").hide(); //Hide the results whenever you hit enter

                return false;
            });

            $("#txtLoginID").keypress(function(event) {
                if (event.keyCode == 13) { //On enter, fire the search click event
                    $("#btnSearchUser").click();
                }
            });
            /*
            $("#tblUsers").tablesorter({
            headers: { 4: { sorter: false }, 5: { sorter: false} },
            cssAsc: 'headerSortUp',
            cssDesc: 'headerSortDown',
            cssHeader: 'header',
            widgets: ['zebra']
            });
            */

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
                function() { AddUserSuccess(application, search); },
                null);
            });
        });

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

                $(newrow).effect("highlight", {}, 3000);

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

                $(newrow).effect("highlight", {}, 3000);

                AjaxCall(baseURL + "AddUnit",
                    { login: login, unitFIS: newunit },
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
                        PopulateUserTableDefault(applicationName);
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
                { login: login, unitFIS: unit },
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

        function PopulateUserTableDefault(application) {
            PopulateUserTable(application, search, null, null, sortname, sortorder);
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
                sortorder: sortorder };
            
            //Call the webservice
            AjaxCall(baseURL + 'GetUsers', data, PopulateUserTableSuccess, null);
        }

        function PopulateUserTableSuccess(data) {
            //data = data.d; //Grab the inner container of data

            //Clear the usertable
            $("#tblUsersBody").empty();
            
            //Render out each row
            $(data.rows).each(RenderRow);

            totalPages = data.total;
            
            //Populate Page Info
            $("#spanPageInfo").html(page + " of " + totalPages);

            SortTable();
            ShowLoadingIndicator(false);
            userTableDirty = false;//The user table is now up to date
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

            newrow.append('<td class="ShowUser"><a href="javascript:;" class="ShowUserLink">'+row.Login+'</a></td>');
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

                dom += Trim(obj.Name);
            });

            return dom;
        }

        function Trim(str) {
            return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
        }

        function OpenDialog(dialog /*The dialog DIV JQuery object*/, buttons /*Button collection */, title, onClose) {

            dialog.dialog("destroy"); //Reset the dialog to its initial state
            dialog.dialog({
                autoOpen: true,
                closeOnEscape: false,
                width: 600,
                modal: true,
                title: title,
                buttons: buttons,
                //show: 'fold',
                close: onClose
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
            Search Users: <input type="text" id="txtSearch" /><input type="image" id="imgSearch" title="Clear Search" alt="Clear Search" src="../Images/checked.gif" />
        </span>
    </div>
    <div id="divLoading" style="display:none;">
        Loading...
    </div>
    <table id="tblUsers" class="tablesorter">
        <thead>
            <tr>
                <th ></th>
                <th class="header" title="FirstName">First Name</th>
                <th class="header headerSortUp" title="LastName">Last Name</th>
                <th class="header" title="LoginID">Login</th>
                <th class="header" title="Email">Email</th>
                <th >Departments</th>
                <th >Roles</th>
            </tr>
        </thead>
        <tbody id="tblUsersBody">
            <%--Each row is a new person--%>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="6" align="center">
                    <input id="btnFirst" class="pager" name="First" type="button" value="First" />
                    <input id="btnPrevious" class="pager" name="Previous" type="button" value="Previous" />
                    <span id="spanPageInfo"></span>
                    <input id="btnNext" class="pager" name="Next" type="button" value="Next" />
                    <input id="btnLast" class="pager" name="Last" type="button" value="Last" />
                </td>
            </tr>
        </tfoot>
    </table>
    
    <div id="dialogUserInfo" title="User Information" style="display: none;">
        <p>
            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
            incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud
            exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
        <div id="divUserInfo" style="display:none;">
            <span id="UserInfoName"></span> (<span id="UserInfoLogin"></span>)<br />
            <table id="UserInfoRoles">
                <thead>
                    <tr>
                        <th>
                            Role
                        </th>
                        <th>
                            Remove
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <br />
            <asp:ListView ID="lviewUserRoles" runat="server" DataSourceID="odsRoles">
                <LayoutTemplate>
                    <select id="UserRoles">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option>
                        <%# Eval("Name") %>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <input type="button" id="btnAddUserRole" value="Add Role" />
            <br /><br />
            <table id="UserInfoUnits">
                <thead>
                    <tr>
                        <th>
                            Unit
                        </th>
                        <th>
                            FISCode
                        </th>
                        <th>
                            Remove
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <br />
            <asp:ListView ID="lviewUserUnits" runat="server" DataSourceID="odsUnits">
                <LayoutTemplate>
                    <select id="UserUnits">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option value='<%# Eval("FISCode") %>'>
                        <%# Eval("ShortName") %>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <input type="button" id="btnAddUserUnit" value="Add Unit" />
        </div>
    </div>
    
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

