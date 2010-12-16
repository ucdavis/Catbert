<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Models.UserShowModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details <%: Model.Login %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details: <%: Model.FullNameAndLogin %></h2>

    <fieldset>
        <legend>User Properties</legend>

        <%: Html.DisplayForModel() %>
        
    </fieldset>
    <fieldset>
        <legend>Permissions</legend>

        <table>
            <thead>
                <tr>
                    <td>Application</td>
                    <td>Role</td>
                </tr>
            </thead>
            <tbody>
            <% foreach(var perm in Model.Permissions.OrderBy(x=>x.Application.Name).ThenBy(x=>x.Role.Name)) { %>
                <tr>
                    <td><%: perm.Application.Name %></td>
                    <td><%: perm.Role.Name %></td>
                </tr>    
            <% } %>
            </tbody>
        </table>
    </fieldset>
        <fieldset>
        <legend>Unit Associations</legend>

        <table>
            <thead>
                <tr>
                    <td>Application</td>
                    <td>Unit</td>
                </tr>
            </thead>
            <tbody>
            <% foreach(var unitassociation in Model.UnitAssociations.OrderBy(x=>x.Application.Name).ThenBy(x=>x.Unit.ShortName)) { %>
                <tr>
                    <td><%: unitassociation.Application.Name %></td>
                    <td><%: unitassociation.Unit.ShortName %></td>
                </tr>    
            <% } %>
            </tbody>
        </table>
    </fieldset>
    <p>
        <%: Html.ActionLink("Edit", "Edit", new { id = Model.Login }) %> |
        <%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

