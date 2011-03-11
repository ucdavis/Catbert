<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Catbert4.Core.Domain.Message>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Text
            </th>
            <th>
                BeginDisplayDate
            </th>
            <th>
                EndDisplayDate
            </th>
            <th>
                Critical
            </th>
            <th>
                Active
            </th>
            <th>
                Application
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <%--<td>
                <%: Html.ActionLink("Edit", "Edit", new { id = item.Id }) %> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
            </td>--%>
            <td><a href="TODO!" class="img"><img src="<%: Url.Image("edit.png") %>" alt="Edit/Details" /></a> | <a href="TODO!" class="img"><img src="<%: Url.Image("delete.png") %>" alt="Delete" /></a></td>
            <td>
                <%: item.Text %>
            </td>
            <td>
                <%: String.Format("{0:d}", item.BeginDisplayDate) %>
            </td>
            <td>
                <%: String.Format("{0:d}", item.EndDisplayDate) %>
            </td>
            <td>
                <%: item.Critical %>
            </td>
            <td>
                <%: item.Active %>
            </td>
            <td>
                <%: item.Application == null ? "All" : item.Application.Name %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <img class="back" src="<%: Url.Image("add.png") %>" alt="New token! TODO!" /> &nbsp;<%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

