<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Catbert4.Core.Domain.School>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Schools
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Schools</h2>

    <p id="pageactions">
        <img class="back" src="<%: Url.Image("add.png") %>" alt="New token! TODO!" /> &nbsp;<%: Html.ActionLink("Create New School", "Create") %>
    </p>

    <table>
        <tr>
            <th></th>
            <th>
                ShortDescription
            </th>
            <th>
                LongDescription
            </th>
            <th>
                Abbreviation
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
            <a href="<%: Url.Action("Edit", new { id = item.Id }) %>" class="img"><img src="<%: Url.Image("edit.png") %>" alt="Edit" /></a> | <a href="<%: Url.Action("Details", new { id = item.Id }) %>" class="img"><img src="<%: Url.Image("file.png") %>" alt="Details" /></a> | <a href="<%: Url.Action("Delete", new { id = item.Id }) %>" class="img"><img src="<%: Url.Image("delete.png") %>" alt="Details" /></a>
            </td>
            <td>
                <%: item.ShortDescription %>
            </td>
            <td>
                <%: item.LongDescription %>
            </td>
            <td>
                <%: item.Abbreviation %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

