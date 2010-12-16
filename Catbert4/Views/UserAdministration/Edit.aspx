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

        <table>
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
    </fieldset>
        <fieldset>
        <legend>Unit Associations</legend>

        <table>
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
            <a class="add-association button-plus" href="#">Add</a>
        </div>
    </fieldset>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<script type="text/javascript">
    Catbert = { };
    Catbert.Services = {
        RemovePermission: "<%: Url.Action("RemovePermission") %>",
        RemoveAssociation: "<%: Url.Action("RemoveAssociation") %>"
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

        function SetAntiForgeryToken() {
            Catbert.AntiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        }
</script>

</asp:Content>
