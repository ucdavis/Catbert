<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="Management_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../CSS/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="../JS/jquery.ajaxQueue.js" type="text/javascript"></script>
    <script src="../JS/jquery.autocomplete.js" type="text/javascript"></script>
    
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

            $("#selectSort").change(function(even, other) {
                sortname = this.value; //set the new sortname
                
                PopulateUserTable(application, search, unit, role, sortname, "ASC");
            });
        });

        function PopulateUserTable(application, search, unit, role, sortname, sortorder) {
            //Setup the parameters
            var data = { application: application, search: search, unit: unit, role: role, sortname: sortname, sortorder: sortorder };
            
            //Call the webservice
            AjaxCall('jqGetUsers', data, PopulateUserTableSuccess, null);
        }

        function PopulateUserTableSuccess(data) {
            data = data.d; //Grab the inner container of data

            //Clear the usertable
            $("#tblUsersBody").empty();
            
            //Render out each row
            $(data.rows).each(RenderRow);
        }

        function RenderRow(index, row) {
            var newrow = $('<tr></tr>');

            newrow.append('<td class="Name">' + row.FirstName + " " + row.LastName + '</td>');
            newrow.append('<td class="Login">' + row.Login + '</td>');
            newrow.append('<td class="Email">' + row.Email + '</td>');

            var units = CreateDomFromUserInfoArray(row.Units);
            var roles = CreateDomFromUserInfoArray(row.Roles);

            newrow.append('<td class="Units">' + units + '</td>');
            newrow.append('<td class="Roles">' + roles + '</td>');

            $("#tblUsersBody").append(newrow);
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

        function AjaxCall(method, data, onSuccess, onError) {
            //Serialize the data to json format
            data = JSON2.stringify(data);
            //debugger;
            $.ajax({
                type: 'POST',
                url: baseURL + method,
                data: data,
                dataType: 'json', //JSON
                contentType: 'application/json',
                processData: true,
                success: onSuccess,
                error: onError
            });
        }
    </script>
    
<br /><br />

    <div id="divHeader">
        <span id="search">
            Search Users: <input type="text" id="txtSearch" />
        </span>
        <span id="sort">
            Sort by 
            <select id="selectSort">
                <option value="LastName" selected="selected">Last Name</option>
                <option value="FirstName">First Name</option>
                <option>Login</option>
                <option>Email</option>
            </select>
        </span>
    </div>
    <table id="tblUsers">
        <tbody id="tblUsersBody">
            <%--Each row is a new person--%>
        </tbody>
    </table>
        
</asp:Content>

