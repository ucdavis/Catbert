<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.SchoolViewModel>" %>
<%@ Import Namespace="Catbert4.Core.Domain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

	<%= Html.ClientSideValidation<School>("School") %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(false) %>

        <fieldset>
            <legend>Fields</legend>
            
            <table>
                <%: Html.EditorFor(x=>x.School) %>
                <tr>
                    <td>
                        <div class="editor-label" style="text-align: right;"><%: Html.Label("SchoolCode") %></div>
                    </td>
                    <td>
                        <%: Html.EditorFor(x=>x.School.Id) %>
                        <%: Html.ValidationMessageFor(x=>x.School.Id) %>
                    </td>
                </tr>
            </table>
                        
            <br />
                <input type="submit" value="Create" />
            
        </fieldset>

    <% } %>

    <p>
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
