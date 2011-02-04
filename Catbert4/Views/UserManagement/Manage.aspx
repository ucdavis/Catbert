<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.UserManagementViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Catbert v4: User Management for
        <%: Model.Application %>
    </title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="<%: Url.Css("Site.css")%>" rel="stylesheet" type="text/css" />
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.7/themes/ui-lightness/jquery-ui.css"
        rel="Stylesheet" type="text/css" media="screen" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.8/jquery-ui.min.js"></script>
    <%--<script src="https://ajax.microsoft.com/ajax/jquery.validate/1.5.5/jquery.validate.min.js"></script>--%>
    <script src="<%: Url.Script("jquery.validate.min.js") %>"></script>
    <script src="<%= Url.Script("xVal.jquery.validate.js") %>" type="text/javascript"></script>
    <% Html.RenderPartial("IncludeDataTables"); %>
    <%: Catbert4.Helpers.HtmlHelpers.IncludeJqueryTemplate() %>
    <style type="text/css">
        .dataTables_length
        {
            float: right;
            text-align: right;
            width: 40%;
        }
        .dataTables_filter
        {
            float: left;
            text-align: left;
            width: 50%;
        }
    </style>
    <script type="text/javascript">
        var Catbert = { Services: { }, Indicators: { }, User: { } };
        Catbert.Services.FindUser = "<%: Url.Action("FindUser") %>";
        Catbert.Services.InsertNewUser = "<%: Url.Action("InsertNewUser", new { application = Model.Application }) %>";
        Catbert.Services.LoadUser = "<%: Url.Action("LoadUser", new { application = Model.Application }) %>";
        Catbert.Services.RemoveUnit = "<%: Url.Action("RemoveUnit", new { application = Model.Application }) %>";
        Catbert.Services.RemovePermission = "<%: Url.Action("RemovePermission", new { application = Model.Application }) %>";
        Catbert.Services.AddUnit = "<%: Url.Action("AddUnit", new { application = Model.Application }) %>";
        Catbert.Services.AddPermission = "<%: Url.Action("AddPermission", new { application = Model.Application }) %>";    

        Catbert.User.Roles = [];
        Catbert.User.Units = [];

        <% foreach(var r in Model.Roles) {  %>
            Catbert.User.Roles.push(<%: r.Key %>);
        <% } %>
        
        <% foreach(var u in Model.Units) {  %>
            Catbert.User.Units.push(<%: u.Key %>);
        <% } %>

    </script>
    <script src="<%: Url.Script("UserManagement.js") %>"></script>
</head>
<body>
    <div class="ui-widget" id="message" style="display: none;">
        <div class="ui-state-highlight ui-corner-all">
            <p>
                <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-info"></span>
                <span id="message-text">User Added Successfully</span></p>
        </div>
    </div>
    <br />
    <div>
        <a href="#" id="add-user">Add User</a>
    </div>
    <br />
    <div style="width: 100%">
        <div>
            <%= this.Select("roles").Class("user-filter").Attr("data-filter-column", 5).Options(Model.Roles, x=>x.Value, x=>x.Value).FirstOption("-- Filter By Role --") %>
            <%= this.Select("units").Class("user-filter").Attr("data-filter-column", 4).Options(Model.Units.OrderBy(x => x.Value), x => x.Value, x => x.Value).FirstOption("-- Filter By Unit --")%>
        </div>
        <table id="users" class="display">
            <thead>
                <tr>
                    <th>
                        Login
                    </th>
                    <th>
                        FirstName
                    </th>
                    <th>
                        LastName
                    </th>
                    <th>
                        Email
                    </th>
                    <th>
                        Units
                    </th>
                    <th>
                        Roles
                    </th>
                </tr>
            </thead>
            <tbody>
                <% foreach (var item in Model.UserShowModel)
                   { %>
                <tr>
                    <td>
                        <a href="#" id="<%: item.Login %>" class="modify-user" title="Modify <%: item.FullNameAndLogin %>">
                            <%: item.Login %></a>
                    </td>
                    <td>
                        <%: item.FirstName %>
                    </td>
                    <td>
                        <%: item.LastName %>
                    </td>
                    <td>
                        <%: item.Email %>
                    </td>
                    <td>
                        <%: string.Join(", ", item.UnitAssociations.OrderBy(x=>x.UnitName).Select(x=>x.UnitName.Trim())) %>
                    </td>
                    <td>
                        <%: string.Join(", ", item.Permissions.OrderBy(x=>x.RoleName).Select(x=>x.RoleName.Trim())) %>
                    </td>
                </tr>
                <% } %>
            </tbody>
        </table>
    </div>
    <% Html.RenderPartial("NewUserDialog"); %>
    <% Html.RenderPartial("ModifyUserDialog"); %>
</body>
</html>
