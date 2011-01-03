<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Catbert4.Core.Domain.Application>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Applications
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Applications</h2>

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
            <th>
                Active
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
                <%: Html.ActionLink("Details", "Details", new { id = item.Id })%>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.Abbr %>
            </td>
            <td>
                <%: Html.DisplayFor(x=> item.Location) %>
            </td>
            <td>
                <%: !item.Inactive %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New Application", "Create") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

