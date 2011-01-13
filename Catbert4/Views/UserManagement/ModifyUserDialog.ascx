<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Catbert4.Controllers.UserManagementViewModel>" %>

<div id="manage-user" title="User Information" style="display: none;">
    <span id="user-info-progress" style="display:none;">Loading User...</span>
    <div id="user-info" style="display: block;">
        <h2>
            <span id="UserInfoName"></span>(<span id="UserInfoLogin"></span>)</h2>
        <br />
        <br />
        <table id="UserInfoRoles">
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
            </tbody>
        </table>
        <br />
        <%= this.Select("userRoles").Options(Model.Roles, x=>x.Key, x=>x.Value) %>
        <input type="button" id="btnAddUserRole" value="Add Role" />
        <br />
        <br />
        <br />
        <br />
        <table id="UserInfoUnits">
            <thead>
                <tr>
                    <th>
                        Unit
                    </th>
                    <th>
                        FISCode
                    </th>
                    <th>
                        Remove
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <br />
        <%= this.Select("userUnits").Options(Model.Units, x=>x.Key, x=>x.Value) %>
        <input type="button" id="btnAddUserUnit" value="Add Unit" />
    </div>
</div>
