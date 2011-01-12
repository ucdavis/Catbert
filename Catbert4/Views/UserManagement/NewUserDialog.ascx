<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Catbert4.Controllers.UserManagementViewModel>" %>

<div id="find-user" title="Add a User" style="display: none;">
    <p>Kerberos or Email: <input type="text" id="search-login" />
    <a href="#" id="search-user">Search</a></p>
    <span id="search-progress" style="display:none;">Searching...</span>
    <div id="search-results" style="display:none;">
        <h2><span id="new-user-first-name-display"></span> <span id="new-user-last-name-display"></span> (<span id="new-user-login-display"></span>)</h2>

        <form id="insert-new-user" action="<%: Url.Action("InsertNewUser") %>">
            <%: Html.AntiForgeryToken() %>
            <input id="new-user-first-name" name="FirstName" type="hidden" />
            <input id="new-user-last-name" name="LastName" type="hidden" />
            <input id="new-user-login" name="Login" type="hidden" />
            <label>Email:</label> <input type="text" id="new-user-email" name="Email" class="required email" /><br />
            <label>Phone:</label> <input type="text" id="new-user-phone" name="Phone" /><br />
            <label>Role:</label>
            <%= this.Select("new-user-roles").Options(Model.Roles, x=>x.Key, x=>x.Value).Class("required") %>
            <br />
            <label>Unit:</label>
            <%= this.Select("new-user-units").Options(Model.Units, x=>x.Key, x=>x.Value).Class("required") %>
            <br /><br />
            <a href="#" id="add-new-user">Add User</a><span id="add-new-user-progress" style="display: none;">Processing...</span>
        </form>
    </div>
</div>