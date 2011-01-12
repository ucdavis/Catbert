<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.UserManagementViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Catbert: User Management</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

<div>
    <a href="#" id="add-user">Add User</a>    
</div>
<br />
<div style="width: 100%">
    <div>
        <%= this.Select("roles").Class("user-filter").Attr("data-filter-column", 5).Options(Model.Roles, x=>x.Value, x=>x.Value).FirstOption("-- Filter By Role --") %>
        <%= this.Select("units").Class("user-filter").Attr("data-filter-column", 4).Options(Model.Units.OrderBy(x => x.Value), x => x.Value, x => x.Value).FirstOption("-- Filter By Unit --")%>
    </div>
    <table id="users" class="display">
        <thead>
            <tr>
                <th>
                    Login
                </th>
                <th>
                    FirstName
                </th>
                <th>
                    LastName
                </th>
                <th>
                    Email
                </th>
                <th>
                    Departments
                </th>
                <th>
                    Roles
                </th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var item in Model.UserShowModel)
               { %>
            <tr>
                <td>
                    <a href="#" title="Modify <%: item.FullNameAndLogin %>"><%: item.Login %></a>
                </td>
                <td>
                    <%: item.FirstName %>
                </td>
                <td>
                    <%: item.LastName %>
                </td>
                <td>
                    <%: item.Email %>
                </td>
                <td>
                    <%: string.Join(", ", item.UnitAssociations.OrderBy(x=>x.UnitName).Select(x=>x.UnitName.Trim())) %>
                </td>
                <td>
                    <%: string.Join(", ", item.Permissions.OrderBy(x=>x.RoleName).Select(x=>x.RoleName.Trim())) %>
                </td>
            </tr>
            <% } %>
        </tbody>
    </table>
</div>

<% Html.RenderPartial("NewUserDialog"); %>

</asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
    <% Html.RenderPartial("IncludeDataTables"); %>

    <style type="text/css">
        .dataTables_length {
            float: right;
            text-align: right;
            width: 40%;
        }
        .dataTables_filter {
            float: left;
            text-align: left;            
            width: 50%;
        }
    </style>
    <script type="text/javascript">
        var Catbert = { Services: { }, Indicators: { } };
        Catbert.Services.FindUser = "<%: Url.Action("FindUser") %>";

        $(function () {

            CreateButtons();

            AssignIndicators(); //Find and assign loading indicators

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

                //var data = { eid: null, firstName: null, lastName: null, email: null, login: $("#txtLoginID").val() };
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

                //Pull out the required fields


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
        }

        function SearchNewUserSuccess(data){
            Log(data);

            Catbert.Indicators.SearchProgress.hide(0);

            if (data == null){
                alert("No Users Found");
            }
            else {
                var searchResults = $("#search-results");

                $("#new-user-first-name", searchResults).html(data.FirstName);
                $("#new-user-last-name", searchResults).html(data.LastName);
                $("#new-user-login", searchResults).html(data.Login);
                $("#new-user-email", searchResults).val(data.Email);
                $("#new-user-phone", searchResults).val(data.Phone);

                searchResults.show(0);
            }
        }

        function Log(text){
            if (typeof console != "undefined") {
                console.info(text);
            }
        }
    </script>
</asp:Content>
