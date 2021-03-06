<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Catbert4.Models.UserListModel>>"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
</asp:Content>

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
                    First Name
                </th>
                <th>
                    Last Name
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
                    <a href="<%: Url.Action("Edit", new { id = item.Login }) %>" class="img"><img src="<%: Url.Image("edit.png") %>" alt="Edit" /></a>
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