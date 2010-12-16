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

    </fieldset>
    <fieldset>
        <legend>Unit Associations</legend>

    </fieldset>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
