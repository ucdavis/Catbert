<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Core.Domain.User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Add New User
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Add New User</h2>
    <%= Html.ClientSideValidation<Catbert4.Core.Domain.User>() %>
    <% using (Html.BeginForm())
       {%>
    <%= Html.AntiForgeryToken() %>
    <%: Html.ValidationSummary(false) %>
    <fieldset>
        <legend>User Info for <%: Model.FullNameAndLogin %></legend>
        <table>
            <tbody>
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.FirstName) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x=>x.FirstName) %>
                            <%: Html.ValidationMessageFor(x=>x.FirstName) %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.LastName) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x => x.LastName)%>
                            <%: Html.ValidationMessageFor(x => x.LastName)%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.Email) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x => x.Email)%>
                            <%: Html.ValidationMessageFor(x => x.Email)%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="text-align: right;" class="editor-label">
                            <%: Html.LabelFor(x=>x.Phone) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x => x.Phone)%>
                            <%: Html.ValidationMessageFor(x => x.Phone)%>
                        </div>
                    </td>
                </tr>                
            </tbody>
        </table>

        <%: Html.HiddenFor(x=>x.LoginId) %>

        <p>
            <input type="submit" value="Add" />
        </p>
    </fieldset>
    <% } %>
    <div>
        <%: Html.ActionLink("Find A Different User", "Find") %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
