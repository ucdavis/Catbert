<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.AccessTokenViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create</h2>
    <%= Html.ClientSideValidation<Catbert4.Core.Domain.AccessToken>("AccessToken") %>
    <% using (Html.BeginForm())
       {%>
    <%= Html.AntiForgeryToken() %>
    <%: Html.ValidationSummary(false) %>
    <fieldset>
        <legend>Fields</legend>
        <table>
            <tbody>
                <tr>
                    <td>
                        <div class="editor-label" style="text-align: right;">
                            <%: Html.LabelFor(x=>x.AccessToken.Application) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%= this.Select("AccessToken.Application").Options(Model.Applications, x=>x.Id, x=>x.Name).FirstOption("Select An Application") %>
                            <%: Html.ValidationMessageFor(x=>x.AccessToken.Application) %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="editor-label" style="text-align: right;">
                            <%: Html.LabelFor(x=>x.AccessToken.ContactEmail) %>                        
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x=>x.AccessToken.ContactEmail) %>
                            <%: Html.ValidationMessageFor(x=>x.AccessToken.ContactEmail) %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="editor-label" style="text-align: right;">
                            <%: Html.LabelFor(x=>x.AccessToken.Reason) %>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%: Html.EditorFor(x => x.AccessToken.Reason, "MultilineText")%>
                            <%: Html.ValidationMessageFor(x=>x.AccessToken.Reason) %>
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
