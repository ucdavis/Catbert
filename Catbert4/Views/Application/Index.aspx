<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Catbert4.Core.Domain.Application>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Applications
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Applications</h2>

    <p id="pageactions">
        <img class="back" src="<%: Url.Image("add.png") %>" alt="New token! TODO!" /> &nbsp;<%: Html.ActionLink("Create New Application", "Create") %>
    </p>

    <table>
        <tr>
            <th></th>
            <th>Name</th>
            <th>Abbr</th>
            <th>Location</th>
            <th>Active</th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td><a href="<%: Url.Action("Edit", new { id = item.Id }) %>" class="img"><img src="<%: Url.Image("edit.png") %>" alt="Edit" /></a> | <a href="<%: Url.Action("Details", new { id = item.Id }) %>" class="img"><img src="<%: Url.Image("file.png") %>" alt="Details" /></a></td>
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

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

