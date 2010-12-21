<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Catbert4.Models.UserListModel>>"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
</asp:Content>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
<div style="width: 60%">
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
            <% foreach (var item in Model)
               { %>
            <tr>
                <td>
                    <%: Html.ActionLink("Edit", "Edit", new { id=item.Login }) %>
                </td>
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
            </tr>
            <% } %>
        </tbody>
    </table>
</div>
</asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
    <% Html.RenderPartial("IncludeDataTables"); %>
    
    <script type="text/javascript">
        $(function () {
            $("#users").dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "aaSorting": [[ 3, "asc" ]]
            });
        });
    </script>>
</asp:Content>