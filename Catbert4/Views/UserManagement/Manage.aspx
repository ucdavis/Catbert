<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.UserManagementViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Catbert: User Management</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

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
                    <%: item.Login %>
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
        var userTable = null;

        $(function () {
            userTable = $("#users").dataTable({
                "bJQueryUI": true,
                "iDisplayLength": 25,
                "sPaginationType": "full_numbers",
                "aoColumns": [null, null, null, null, { "bSortable": false }, { "bSortable": false}], //Don't sort the last two cols
                "aaSorting": [[2, "asc"]]
            });

            $(".user-filter").change(function () {
                var element = $(this);
                userTable.fnFilter(element.val(), element.data("filter-column"));
            });
        });
    </script>
</asp:Content>
