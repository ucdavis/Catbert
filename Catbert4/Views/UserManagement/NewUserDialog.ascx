<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Catbert4.Controllers.UserManagementViewModel>" %>

<div id="find-user" title="Add a User" style="display: none;">
    <p>Kerberos or Email: <input type="text" id="search-login" />
    <a href="#" id="search-user">Search</a></p>
    <span id="search-progress" style="display:none;">Searching...</span>
    <div id="search-results" style="display:none;">
        <h2><span id="new-user-first-name"></span> <span id="new-user-last-name"></span> (<span id="new-user-login"></span>)</h2>
        <label>Email:</label> <input type="text" id="new-user-email" /><br />
        <label>Phone:</label> <input type="text" id="new-user-phone" /><br />
        <label>Role:</label>
        <%= this.Select("new-user-roles").Options(Model.Roles, x=>x.Key, x=>x.Value) %>
        <br />
        <label>Unit:</label>
        <%= this.Select("new-user-units").Options(Model.Units, x=>x.Key, x=>x.Value) %>
        <br /><br />
        <a href="#" id="add-new-user">Add User</a><span id="add-new-user-progress" style="display: none;">Processing...</span>
    </div>
</div>