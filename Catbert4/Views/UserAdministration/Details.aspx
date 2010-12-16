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
                    <th>Application</th>
                    <th>Role</th>
                </tr>
            </thead>
            <tbody>
            <% foreach(var perm in Model.Permissions.OrderBy(x=>x.ApplicationName).ThenBy(x=>x.RoleName)) { %>
                <tr>
                    <td><%: perm.ApplicationName %></td>
                    <td><%: perm.RoleName %></td>
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
                    <th>Application</th>
                    <th>Unit</th>
                </tr>
            </thead>
            <tbody>
            <% foreach(var unitassociation in Model.UnitAssociations.OrderBy(x=>x.ApplicationName).ThenBy(x=>x.UnitName)) { %>
                <tr>
                    <td><%: unitassociation.ApplicationName %></td>
                    <td><%: unitassociation.UnitName %></td>
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

