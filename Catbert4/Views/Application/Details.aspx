<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Core.Domain.Application>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Application Details</legend>
        
        <table>
        <%: Html.DisplayForModel() %>
        </table> 

    </fieldset>

    <fieldset>
        <legend>Roles</legend>
        <h3>Ordered:</h3>
        <ul>
            <% foreach (var role in Model.ApplicationRoles.Where(x=>x.Level != null).OrderBy(x=>x.Level)) { %>
                <li>
                    (<%: role.Level %>): <%: role.Role.Name %>
                </li>
            <% } %>
        </ul>

        <h3>Unordered:</h3>
        <ul>
            <% foreach (var role in Model.ApplicationRoles.Where(x=>x.Level == null)) { %>
                <li>
                    <%: role.Role.Name %>
                </li>
            <% } %>
        </ul>

    </fieldset>
    <p>
        <img class="back" src="<%: Url.Image("edit.png") %>" alt="Edit TODO!" />&nbsp;<%: Html.ActionLink("Edit", "Edit", new { id = Model.Id }) %> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

