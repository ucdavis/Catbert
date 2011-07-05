<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Catbert4.Controllers.ApplicationViewModel>" %>

<fieldset>
    <legend>Application Details</legend>
            
        <table>
        <%: Html.EditorFor(x=>x.Application) %>
        </table>

</fieldset>

<fieldset>
    <legend>Roles</legend>
    <p>[<%: Html.ActionLink<RoleController>(x=>x.Index(), "Manage Roles") %>]</p>

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

<fieldset>
    <legend>Units</legend>
    <p>Note: This section is <strong>optional</strong>. If you leave it blank, all departments will be available for association.</p>
    <p>[<%: Html.ActionLink<UnitController>(x=>x.Index(), "Manage Units") %>]</p>

    <%: this.MultiSelect("units").Options(Model.Units, x=>x.Id, x=>x.ShortName).Selected(Model.Application.ApplicationUnits.Select(x=>x.Unit.Id)).Class("multiselect") %>
</fieldset>
