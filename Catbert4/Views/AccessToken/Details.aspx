<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Core.Domain.AccessToken>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        
        <table>
            <%: Html.DisplayForModel("DisplayComplexValues") %>
        </table>

    </fieldset>

    <% using (Html.BeginForm("SwitchActiveStatus", "AccessToken", new { id = Model.Id })) { %>
    <%= Html.AntiForgeryToken()%>
    
    <fieldset>
        <legend>Actions</legend>

        <input type="submit" value="<%: Model.Active ? "Deactivate Token" : "Re-Activate Token" %>" />
        
    </fieldset>
    <% } %>
    <p>
        <%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

