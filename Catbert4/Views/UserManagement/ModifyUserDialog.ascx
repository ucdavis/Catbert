<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Catbert4.Controllers.UserManagementViewModel>" %>

<div id="manage-user" title="User Information" style="display: none;">
    <span id="user-info-progress" style="display:none;">Loading User...</span>
    <%--Filled in with the below template info--%>
</div>

<script id="user-info-template" type="text/x-jquery-tmpl">    
<div id="user-info">
    <h2>
        <span id="user-info-name">${FullNameAndLogin}</span>
    </h2>
    <fieldset>
        <legend>Roles</legend>
        <table id="user-info-roles">
            <thead>
                <tr>
                    <th>
                        Role
                    </th>
                    <th>
                        Remove
                    </th>
                </tr>
            </thead>
            <tbody>
                {{each Permissions}}
                    <tr>
                        <td>${$value.RoleName}</td>
                        <td>
                            <a href="#" class="remove-permission remove-link {{if $value.UserEditable == false}}no-delete{{/if}}" data-type="permission" data-id="${$value.RoleId}" data-login="${Login}">Remove</a>
                        </td>
                    </tr>
                {{/each}}
            </tbody>
        </table>
        <br />
        <p><%= this.Select("userRoles").Options(Model.Roles, x=>x.Key, x=>x.Value) %>
        <a href="#" id="add-permission" class="add-link" data-type="permission" data-login="${Login}">Add Role</a></p>
    </fieldset>
    <fieldset>
        <legend>Units</legend>
        <table id="user-info-units">
            <thead>
                <tr>
                    <th>
                        Unit
                    </th>
                    <th>
                        Remove
                    </th>
                </tr>
            </thead>
            <tbody>
                {{each UnitAssociations}}
                    <tr>
                        <td>${$value.UnitName}</td>
                        <td>
                            <a href="#" class="remove-unit remove-link {{if $value.UserEditable == false}}no-delete{{/if}}" data-type="unit" data-id="${$value.UnitId}" data-login="${Login}">Remove</a>
                        </td>
                    </tr>
                {{/each}}
            </tbody>
        </table>
        <br />
        <p><%= this.Select("userUnits").Options(Model.Units, x=>x.Key, x=>x.Value) %>
        <a href="#" id="add-unit" class="add-link" data-type="unit" data-login="${Login}">Add Unit</a></p>
    </fieldset>
</div>
</script>

<script id="user-info-row-template" type="text/x-jquery-tmpl">
<tr>
    <td>${value}</td>
    <td>
        <a href="#" class="remove-${type} remove-link" data-type="${type}" data-id="${id}" data-login="${login}">Remove</a>                    
    </td>
</tr>
</script>