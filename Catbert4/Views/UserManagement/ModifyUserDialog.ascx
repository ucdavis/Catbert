﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Catbert4.Controllers.UserManagementViewModel>" %>

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
                            <a href="#" class="remove-role remove-link" data-type="permission" data-id="${$value.Id}" data-login="${Login}">Remove</a>                    
                        </td>
                    </tr>
                {{/each}}
            </tbody>
        </table>
        <br />
        <%= this.Select("userRoles").Options(Model.Roles, x=>x.Key, x=>x.Value) %>
        <a href="#" id="add-role" class="add-link" data-type="permission" data-login="${Login}">Add Role</a>
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
                            <a href="#" class="remove-unit remove-link" data-type="unit" data-id="${$value.Id}" data-login="${Login}">Remove</a>
                        </td>
                    </tr>
                {{/each}}
            </tbody>
        </table>
        <br />
        <%= this.Select("userUnits").Options(Model.Units, x=>x.Key, x=>x.Value) %>
        <a href="#" id="add-unit" class="add-link" data-type="unit" data-login="${Login}">Add Unit</a>
    </fieldset>
</div>
</script>
