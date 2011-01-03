<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Models.UserShowModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete <%: Model.Login %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="ui-widget">
		<div style="padding: 0pt 0.7em;" class="ui-state-error ui-corner-all"> 
			<p><span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span> 
			<strong>Alert:</strong> Deleting <%: Model.FullNameAndLogin %> will remove all of their permission and unit associations. 
            Please click "Delete" after reviewing this account to proceed.</p>
		</div>
	</div>

    <h2>Delete: <%: Model.FullNameAndLogin %></h2>

    <fieldset>
        <legend>User Properties</legend>

        <table>
            <%: Html.DisplayForModel() %>
        </table>

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

    <% using (Html.BeginForm()) { %>
        <%: Html.AntiForgeryToken() %>

        <fieldset>
            <legend>Delete:</legend>

            <button class="button-trash" type="submit">Delete user</button>
        </fieldset>

    <% } %>

    <p>
        <%: Html.ActionLink("Edit", "Edit", new { id = Model.Login }) %> |
        <%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

