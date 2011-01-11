<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Catbert4.Core.Domain.Application>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UserManagement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>UserManagement</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Name
            </th>
            <th>
                Abbr
            </th>
            <th>
                Location
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Manage", "Manage", "UserManagement", new { application = item.Name }, null) %>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.Abbr %>
            </td>
            <td>
                <%: item.Location %>
            </td>
        </tr>
    
    <% } %>

    </table>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

