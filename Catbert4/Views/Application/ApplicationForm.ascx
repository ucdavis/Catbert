<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Catbert4.Controllers.ApplicationViewModel>" %>

<fieldset>
    <legend>Application Details</legend>
            
        <table>
        <%: Html.EditorFor(x=>x.Application) %>
        </table>

</fieldset>

<fieldset>
    <legend>Roles</legend>
    [<%: Html.ActionLink<RoleController>(x=>x.Index(), "Manage Roles") %>]

    <h3>Ordered:</h3>
    <ul id="ordered-roles" class="connectedSortable">
        <% foreach (var role in Model.Application.ApplicationRoles.Where(x=>x.Level != null).OrderBy(x=>x.Level)) { %>
            <li class="ui-state-default" id="<%: role.Role.Name %>">
                (<%: role.Level %>): <%: role.Role.Name %>
            </li>
        <% } %>
    </ul>

    <h3>Unordered:</h3>
    <ul id="unordered-roles" class="connectedSortable">
        <% foreach (var role in Model.Application.ApplicationRoles.Where(x=>x.Level == null)) { %>
            <li class="ui-state-default" id="<%: role.Role.Name %>">
                <%: role.Role.Name %>
            </li>
        <% } %>
    </ul>

    <h3>Available:</h3>
    
    <ul class="connectedSortable">
        <% foreach (var role in Model.GetAvailableRoles()) { %>
            <li class="ui-state-default" id="<%: role.Name %>">
                <%: role.Name %>
            </li>
        <% } %>
    </ul>
</fieldset>