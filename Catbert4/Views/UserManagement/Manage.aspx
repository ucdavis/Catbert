<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.UserManagementViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Catbert: User Management</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

<div style="width: 100%">
    <table id="users" class="display">
        <thead>
            <tr>
                <th>
                </th>
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
            </tr>
        </thead>
        <tbody>
            <% foreach (var item in Model.Users)
               { %>
            <tr>
                <td>
                    <%: Html.ActionLink("Edit", "Edit", new { id=item.LoginId }) %>
                </td>
                <td>
                    <%: item.LoginId %>
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
        $(function () {
            $("#users").dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "aaSorting": [[3, "asc"]]
            });
        });
    </script>>
</asp:Content>
