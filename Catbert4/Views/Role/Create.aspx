<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.RoleViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create Role
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create New Role</h2>
    <%= Html.ClientSideValidation<Catbert4.Core.Domain.Role>() %>
    <% using (Html.BeginForm())
       {%>
    <%= Html.AntiForgeryToken() %>
    <%: Html.ValidationSummary(false) %>
    <fieldset>
        <legend>Role</legend>
        <table>
            <tbody>
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.Role.Name) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x=>x.Role.Name) %>
                            <%: Html.ValidationMessageFor(x=>x.Role.Name) %>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>

    <% } %>
    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
