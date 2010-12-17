<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit <%: Model.User.LoginId %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Editing: <%: Model.User.FullNameAndLogin %></h2>
    <%= Html.ClientSideValidation<Catbert4.Core.Domain.User>("User") %>
    <% using (Html.BeginForm())
       {%>
    <%= Html.AntiForgeryToken() %>
    <%: Html.ValidationSummary(false) %>
    <fieldset>
        <legend>User Properties</legend>
        <table>
            <tbody>
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.User.FirstName) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x=>x.User.FirstName) %>
                            <%: Html.ValidationMessageFor(x=>x.User.FirstName) %>
                        </div>
                    </td>
                </tr>                
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.User.LastName) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x=>x.User.LastName) %>
                            <%: Html.ValidationMessageFor(x=>x.User.LastName) %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.User.Email) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x => x.User.Email)%>
                            <%: Html.ValidationMessageFor(x => x.User.Email)%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.User.Phone) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x => x.User.Phone)%>
                            <%: Html.ValidationMessageFor(x => x.User.Phone)%>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <%: Html.HiddenFor(x=>x.User.Id) %>
        <%: Html.HiddenFor(x=>x.User.LoginId) %>
        <p>
            <input type="submit" value="Edit" />
        </p>
    </fieldset>
    <% } %>

<fieldset>
        <legend>Permissions</legend>

        <table id="permissions">
            <thead>
                <tr>
                    <th>Application</th>
                    <th>Role</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
            <% foreach(var perm in Model.UserShowModel.Permissions.OrderBy(x=>x.ApplicationName).ThenBy(x=>x.RoleName)) { %>
                <tr>
                    <td><%: perm.ApplicationName %></td>
                    <td><%: perm.RoleName %></td>
                    <td><a class="remove-permission button-trash" href="#" data-id="<%: perm.Id %>">Remove</a></td>
                </tr>    
            <% } %>
            </tbody>
        </table>
        <div>
            <%= this.Select("add-permission-application").Options(Model.UserLookupModel.Applications, x => x.Key, x => x.Value).FirstOption("--Select An Application--")%>
            <select id='add-permission-roles' name='add-permission-roles'>
                <option value="0">--Select A Role--</option>
            </select>
        
            <a class="add-permission button-plus" href="#" data-table="permissions" data-application="add-permission-application" data-role="add-permission-roles">Add</a>
        </div>
    </fieldset>
        <fieldset>
        <legend>Unit Associations</legend>

        <table id="unit-associations">
            <thead>
                <tr>
                    <th>Application</th>
                    <th>Unit</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
            <% foreach(var unitassociation in Model.UserShowModel.UnitAssociations.OrderBy(x=>x.ApplicationName).ThenBy(x=>x.UnitName)) { %>
                <tr>
                    <td><%: unitassociation.ApplicationName %></td>
                    <td><%: unitassociation.UnitName %></td>
                    <td><a class="remove-association button-trash" href="#" data-id="<%: unitassociation.Id %>">Remove</a></td>
                </tr>    
            <% } %>
            </tbody>
        </table>

        <div>
            <%= this.Select("add-association-application").Options(Model.UserLookupModel.Applications, x=>x.Key, x=>x.Value).FirstOption("--Select An Application--") %>
            <%= this.Select("add-association-unit").Options(Model.UserLookupModel.Units, x=>x.Key, x=>x.Value).FirstOption("--Select A Unit--") %>
            <a class="add-association button-plus" href="#" data-table="unit-associations" data-application="add-association-application" data-unit="add-association-unit">Add</a>
        </div>
    </fieldset>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

    <script id="user-association-template" type="text/x-jquery-tmpl">
    <tr>
        <td>${ApplicationName}</td>
        <td>${RoleOrUnitName}</td>
        <td><a class="${CssClass} button-trash" href="#" data-id="${RoleOrUnitId}">Remove</a></td>
    </tr>
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<%: Catbert4.Helpers.HtmlScriptHelpers.IncludeJqueryTemplate()%>

<script src="<%: Url.Script("jquery.jqcascade.min.js") %>" type="text/javascript"></script>

<script type="text/javascript">
    Catbert = { };
    Catbert.UserId = <%: Model.User.Id %>;
    Catbert.Services = {
        RemovePermission: "<%: Url.Action("RemovePermission") %>",
        RemoveAssociation: "<%: Url.Action("RemoveAssociation") %>",
        AddPermission: "<%: Url.Action("AddPermission") %>",
        AddAssociation: "<%: Url.Action("AddAssociation") %>"
        };

        $(function () {
            SetAntiForgeryToken();

            $(".remove-permission").live('click', function (e) {
                e.preventDefault();
                        
                RemoveEntity(this, Catbert.Services.RemovePermission);

            });

            $(".remove-association").live('click', function (e) {
                e.preventDefault();

                RemoveEntity(this, Catbert.Services.RemoveAssociation);
            });

            $(".add-permission").click(function (e) {
                e.preventDefault();
                        
                AddEntity(this, Catbert.Services.AddPermission);

            });

            $(".add-association").click(function (e) {
                e.preventDefault();

                AddEntity(this, Catbert.Services.AddAssociation);
            });

            $("#add-permission-roles").cascading({
                dataUrl: '<%: Url.Action("GetRolesForApplication") %>',
                parentDropDownId: 'add-permission-application',
                noSelectionValue: '0',
                noSelectionText: '--Select A Role--'
            });
        });

        function RemoveEntity(element, removeUrl) {
            var wrappedElement = $(element);

            var id = wrappedElement.data("id");
            var parentRow = wrappedElement.parent().parent();
            parentRow.fadeOut();

            $.post(
                removeUrl,
                { 
                    id: id, 
                    "__RequestVerificationToken": Catbert.AntiForgeryToken 
                },
                null
            );
        }

        function AddEntity(element, addUrl) {
            var wrappedElement = $(element);

            var table = $("#" + wrappedElement.data("table")).find("tbody");
            var applicationElement = $("#" + wrappedElement.data("application"));
            var application = applicationElement.val();
            var roleElement = $("#" + wrappedElement.data("role"));
            var role = roleElement.val();
            var unitElement = $("#" + wrappedElement.data("unit"));
            var unit = unitElement.val();
            
            if (application == 0) {
                alert("Please select an application");
                return;
            }
            else if (role == 0) { 
                alert("Please select a role");
                return;
            }
            else if (unit == 0) { 
                alert("Please select a unit");
                return;
            }

            var data = { 
                    userId: Catbert.UserId,
                    applicationId: application,
                    roleId: role,
                    unitId: unit,
                    "__RequestVerificationToken": Catbert.AntiForgeryToken 
                };

            $.post(
                addUrl,
                data,
                function(data) {
                    if (data.Success == false){
                        alert(data.Comment);
                        return false;
                    }

                    var useRole = unit === undefined;

                    var tmplData = {
                        ApplicationName: applicationElement.find('option:selected').text(),
                        RoleOrUnitName: useRole ? roleElement.find('option:selected').text() : unitElement.find('option:selected').text(),
                        RoleOrUnitId: data.Identifier,
                        CssClass: useRole? 'remove-permission' : 'remove-association'
                    };
                    
                    var newRow = $("#user-association-template").tmpl(tmplData);
                    table.append(newRow);

                    RenderButtons();
                    newRow.effect('highlight', 2000);
                },
                'json'
            );
        }

        function SetAntiForgeryToken() {
            Catbert.AntiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        }
</script>

</asp:Content>
